using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Cathei.PinInject.Internal
{
    public class ReflectionCache
    {
        private static readonly Dictionary<Type, ReflectionCache> _cachePerType = new Dictionary<Type, ReflectionCache>();

        public static ReflectionCache Get(Type type)
        {
            if (_cachePerType.TryGetValue(type, out var cache))
                return cache;

            cache = new ReflectionCache(type);
            _cachePerType.Add(type, cache);
            return cache;
        }

        public IEnumerable<InjectableProperty> Injectables => _injectables;
        public IEnumerable<ResolvableProperty> Resolvables => _resolvables;

        public bool HasAnyAttribute => _injectables != null || _resolvables != null;

        private readonly List<InjectableProperty> _injectables = null;
        private readonly List<ResolvableProperty> _resolvables = null;

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
                    _resolvables.Add(new ResolvableProperty(prop.GetValue));
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
                    _resolvables.Add(new ResolvableProperty(field.GetValue));
                }
            }
        }

        public struct InjectableProperty
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
        }

        public struct ResolvableProperty
        {
            public readonly Func<object, object> Getter;

            public ResolvableProperty(Func<object, object> getter)
            {
                Getter = getter;
            }
        }
    }
}
