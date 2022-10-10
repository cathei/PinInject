// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public class DefaultStrategy : IStrategy
    {
        private static readonly HashSet<object> _recursiveCheck = new HashSet<object>();

        public virtual void Inject(object obj, IDependencyContainer container)
        {
            // entry point of injection
            _recursiveCheck.Clear();

            InjectInternal(obj, container);
        }

        private void InjectInternal(object obj, IDependencyContainer baseContainer)
        {
            if (_recursiveCheck.Contains(obj))
                throw new InjectionException($"Circular dependency injection on {obj.GetType()} {obj}");

            _recursiveCheck.Add(obj);

            IDependencyContainer container = baseContainer;
            DependencyRegistry registry = default;

            // another depth of injection
            if (obj is IContext)
            {
                var childContainer = new DependencyContainer();
                childContainer.SetParent(baseContainer);

                container = childContainer;
                registry = new DependencyRegistry(childContainer);
            }

            InjectBindResolve(obj, container, registry);
        }

        internal void InjectBindResolve(object obj, IDependencyContainer container, DependencyRegistry registry)
        {
            var reflection = ReflectionCache.Get(obj.GetType());

            InjectProperties(reflection, obj, container);

            if (obj is IContext context)
                context.Configure(registry);

            ResolveProperties(reflection, obj, container);

            if (obj is IPostInjectHandler postInjectHandler)
                postInjectHandler.PostInject();
        }

        private void InjectProperties(ReflectionCache reflection, object obj, IDependencyContainer container)
        {
            if (reflection.Injectables == null)
                return;

            foreach (var injectable in reflection.Injectables)
            {
                var value = container.Resolve(injectable.Type, injectable.IdGetter(obj));

                if (value == null)
                    throw new InjectionException($"Type {injectable.Type} on {obj} cannot be resolved");

                injectable.Setter(obj, value);
            }
        }

        private void ResolveProperties(ReflectionCache reflection, object obj, IDependencyContainer container)
        {
            if (reflection.Resolvables == null)
                return;

            foreach (var resolvable in reflection.Resolvables)
            {
                var child = resolvable.Getter(obj);

                if (child == null)
                    continue;

                // even for game object, nested properties should use default inject strategy
                // also resolving other game object with [Resolve] is not allowed
                InjectInternal(child, container);
            }
        }
    }
}
