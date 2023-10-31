// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public sealed class DefaultInjectionStrategy : IInjectionStrategy<object>
    {
        private static readonly HashSet<object> RecursiveCheck = new();

        public void Inject(object obj, IDependencyContainer container, Pin.ContextConfiguration config)
        {
            // entry point of injection
            RecursiveCheck.Clear();

            if (config != null)
            {
                var localContainer = new DependencyContainer(container);
                config(new DependencyBinder(localContainer));
                container = localContainer;
            }

            InjectInternal(obj, container);
        }

        private void InjectInternal(object obj, IDependencyContainer baseContainer)
        {
            if (RecursiveCheck.Contains(obj))
                throw new InjectionException($"Circular dependency injection on {obj.GetType()} {obj}");

            RecursiveCheck.Add(obj);

            IDependencyContainer container = baseContainer;
            DependencyBinder binder = default;

            // another depth of injection
            if (obj is IInjectionContext)
            {
                var childContainer = new DependencyContainer();
                childContainer.SetParent(baseContainer);

                container = childContainer;
                binder = new DependencyBinder(childContainer);
            }

            InjectBindResolve(obj, container, binder);
        }

        internal void InjectBindResolve(object obj, IDependencyContainer container, DependencyBinder binder)
        {
            var reflection = ReflectionCache.Get(obj.GetType());

            InjectProperties(reflection, obj, container);

            if (obj is IInjectionContext context)
                context.Configure(binder);

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

                if (value == null && !injectable.Optional)
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

                // even for Unity components, nested properties should use default inject strategy
                // also resolving other game object with [Resolve] is not allowed
                InjectInternal(child, container);
            }
        }
    }
}
