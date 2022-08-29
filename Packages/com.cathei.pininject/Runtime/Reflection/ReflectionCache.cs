using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Cathei.PinInject.Internal
{
    public class ReflectionCache
    {
        private static readonly Dictionary<Type, ReflectionCache> _cachePerType = new Dictionary<Type, ReflectionCache>();

        private static readonly HashSet<object> _recursiveCheck = new HashSet<object>();

        public static ReflectionCache Get(Type type)
        {
            if (_cachePerType.TryGetValue(type, out var cache))
                return cache;

            cache = new ReflectionCache(type);
            _cachePerType.Add(type, cache);
            return cache;
        }

        private readonly List<InjectableProperty> _injectables = null;
        private readonly List<ResolvableProperty> _resolvables = null;

        public bool HasAnyAttribute => _injectables != null || _resolvables != null;

        internal ReflectionCache(Type type)
        {
            var properties = type.GetProperties(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var fields = type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var injectAttr = prop.GetCustomAttribute<InjectAttribute>();
                var resolveAttr = prop.GetCustomAttribute<ResolveAttribute>();

                if (injectAttr != null)
                {
                    if (prop.SetMethod == null)
                        throw new InjectException($"Property {prop.Name} is marked as [Inject] without setter");

                    _injectables ??= new List<InjectableProperty>();
                    _injectables.Add(new InjectableProperty(prop.PropertyType, injectAttr.Id, prop.SetValue));
                }

                if (resolveAttr != null)
                {
                    if (prop.GetMethod == null)
                        throw new InjectException($"Property {prop.Name} is marked as [Resolve] without getter");

                    _resolvables ??= new List<ResolvableProperty>();
                    _resolvables.Add(new ResolvableProperty(prop.PropertyType, prop.GetValue));
                }
            }

            foreach (var field in fields)
            {
                var injectAttr = field.GetCustomAttribute<InjectAttribute>();
                var resolveAttr = field.GetCustomAttribute<ResolveAttribute>();

                if (injectAttr != null)
                {
                    _injectables ??= new List<InjectableProperty>();
                    _injectables.Add(new InjectableProperty(field.FieldType, injectAttr.Id, field.SetValue));
                }

                if (resolveAttr != null)
                {
                    _resolvables ??= new List<ResolvableProperty>();
                    _resolvables.Add(new ResolvableProperty(field.FieldType, field.GetValue));
                }
            }
        }

        public void Inject(object obj, InjectContainer container)
        {
            // entry point
            _recursiveCheck.Clear();

            InjectInternal(obj, container);
        }

        private void InjectInternal(object obj, InjectContainer container)
        {
            if (_recursiveCheck.Contains(obj))
                throw new InjectException($"Circular dependency injection on {obj.GetType()} {obj}");

            _recursiveCheck.Add(obj);

            var context = obj as IInjectContext;

            // another depth of injection
            if (context != null)
            {
                var childContainer = new InjectContainer();
                childContainer.SetParent(container);
                container = childContainer;
            }

            if (_injectables != null)
            {
                foreach (var injectable in _injectables)
                    injectable.Inject(obj, container);
            }

            if (context != null)
            {
                context.Configure(container);
            }

            if (_resolvables != null)
            {
                foreach (var resolvable in _resolvables)
                    resolvable.Resolve(obj, container);
            }

            if (obj is IPostInjectHandler postInjectHandler)
                postInjectHandler.PostInject();
        }

        private struct InjectableProperty
        {
            public readonly Type Type;
            public readonly string Id;
            public readonly Action<object, object> Setter;

            public InjectableProperty(Type type, string id, Action<object, object> setter)
            {
                Type = type;
                Id = id;
                Setter = setter;
            }

            public void Inject(object obj, InjectContainer container)
            {
                Setter(obj, container.Resolve(Type, Id));
            }
        }

        private struct ResolvableProperty
        {
            public readonly Type Type;
            public readonly Func<object, object> Getter;

            public ResolvableProperty(Type type, Func<object, object> getter)
            {
                Type = type;
                Getter = getter;
            }

            public void Resolve(object obj, InjectContainer container)
            {
                object value = Getter(obj);

                if (value == null)
                    return;

                Get(Type).InjectInternal(value, container);
            }
        }
    }
}
