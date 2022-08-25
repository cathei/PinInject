using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Cathei.PinInject.Internal
{
    public class ReflectionCache
    {
        private static Dictionary<Type, ReflectionCache> _cachePerType = new Dictionary<Type, ReflectionCache>();

        public static ReflectionCache Get(Type type)
        {
            if (_cachePerType.TryGetValue(type, out var cache))
                return cache;

            cache = new ReflectionCache(type);
            _cachePerType.Add(type, cache);
            return cache;
        }

        private List<InjectableProperty> _injectables = null;
        private List<ResolvableProperty> _resolvables = null;

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
                    if (prop.SetMethod != null)
                        throw new InjectException($"Property {prop.Name} is marked as [Inject] without setter");

                    _injectables ??= new List<InjectableProperty>();
                    _injectables.Add(new InjectableProperty((obj, container) =>
                        prop.SetValue(obj, container.Resolve(prop.PropertyType))));
                }

                if (resolveAttr != null)
                {
                    if (prop.GetMethod != null)
                        throw new InjectException($"Property {prop.Name} is marked as [Resolve] without getter");

                    _resolvables ??= new List<ResolvableProperty>();
                    _resolvables.Add(new ResolvableProperty((obj, container) =>
                        Get(prop.PropertyType).Inject(prop.GetValue(obj), container)));
                }
            }

            foreach (var field in fields)
            {
                var injectAttr = field.GetCustomAttribute<InjectAttribute>();
                var resolveAttr = field.GetCustomAttribute<ResolveAttribute>();

                if (injectAttr != null)
                {
                    _injectables ??= new List<InjectableProperty>();
                    _injectables.Add(new InjectableProperty((obj, container) =>
                        field.SetValue(obj, container.Resolve(field.FieldType))));
                }

                if (resolveAttr != null)
                {
                    _resolvables ??= new List<ResolvableProperty>();
                    _resolvables.Add(new ResolvableProperty((obj, container) =>
                        Get(field.FieldType).Inject(field.GetValue(obj), container)));
                }
            }
        }

        public void Inject(object obj, InjectContainer container)
        {
            if (_injectables != null)
            {
                foreach (var injectable in _injectables)
                    injectable.Inject(obj, container);
            }

            if (_resolvables != null)
            {
                foreach (var resolvable in _resolvables)
                    resolvable.Resolve(obj, container);
            }
        }

        public struct InjectableProperty
        {
            public readonly Action<object, InjectContainer> Inject;

            public InjectableProperty(Action<object, InjectContainer> inject)
            {
                Inject = inject;
            }
        }

        public struct ResolvableProperty
        {
            public readonly Action<object, InjectContainer> Resolve;

            public ResolvableProperty(Action<object, InjectContainer> resolve)
            {
                Resolve = resolve;
            }
        }
    }

}