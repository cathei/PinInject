using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public class DefaultInjectStrategy : IInjectStrategy
    {
        private static readonly HashSet<object> _recursiveCheck = new HashSet<object>();

        public void Inject(object obj, InjectContainer container)
        {
            // entry point of injection
            _recursiveCheck.Clear();

            InjectInternal(obj, container);

            // var cache = ReflectionCache.Get(obj.GetType());
            // cache.Inject(obj, container);
        }

        private void InjectInternal(object obj, InjectContainer container)
        {
            if (_recursiveCheck.Contains(obj))
                throw new InjectException($"Circular dependency injection on {obj.GetType()} {obj}");

            _recursiveCheck.Add(obj);

            var reflection = ReflectionCache.Get(obj.GetType());
            var context = obj as IInjectContext;

            // another depth of injection
            if (context != null)
            {
                var childContainer = new InjectContainer();
                childContainer.SetParent(container);
                container = childContainer;
            }

            InjectProperties(reflection, obj, container);

            if (context != null)
                context.Configure(container);

            ResolveProperties(reflection, obj, container);

            if (obj is IPostInjectHandler postInjectHandler)
                postInjectHandler.PostInject();
        }

        public void InjectProperties(ReflectionCache reflection, object obj, InjectContainer container)
        {
            if (reflection.Injectables == null)
                return;

            foreach (var injectable in reflection.Injectables)
                injectable.Setter(obj, container.Resolve(injectable.Type, injectable.Id));
        }

        public void ResolveProperties(ReflectionCache reflection, object obj, InjectContainer container)
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
