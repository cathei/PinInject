// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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

        public readonly bool HasAnyAttribute;

        private readonly List<InjectableProperty> _injectables = null;
        private readonly List<ResolvableProperty> _resolvables = null;

        private const BindingFlags memberBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private const BindingFlags nameBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        internal ReflectionCache(Type type)
        {
            if (type.BaseType != null)
            {
                // we are reflecting base class so we can get private member
                var baseReflection = Get(type.BaseType);

                if (baseReflection._injectables != null)
                    _injectables = new List<InjectableProperty>(baseReflection._injectables);

                if (baseReflection._resolvables != null)
                    _resolvables = new List<ResolvableProperty>(baseReflection._resolvables);
            }

            var properties = type.GetProperties(memberBindingFlags);
            var fields = type.GetFields(memberBindingFlags);

            foreach (var prop in properties)
            {
                var injectAttr = prop.GetCustomAttribute<InjectAttribute>();
                var resolveAttr = prop.GetCustomAttribute<ResolveAttribute>();

                if (injectAttr != null)
                {
                    if (prop.SetMethod == null)
                        throw new InjectException($"Property {prop.Name} is marked as [Inject] without setter");

                    _injectables ??= new List<InjectableProperty>();
                    _injectables.Add(new InjectableProperty(
                        prop.PropertyType, IdGetter(type, injectAttr), prop.SetValue));
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
                    _injectables.Add(new InjectableProperty(
                        field.FieldType, IdGetter(type, injectAttr), field.SetValue));
                }

                if (resolveAttr != null)
                {
                    _resolvables ??= new List<ResolvableProperty>();
                    _resolvables.Add(new ResolvableProperty(field.GetValue));
                }
            }

            HasAnyAttribute = _injectables != null
                || _resolvables != null
                || typeof(IPostInjectHandler).IsAssignableFrom(type);
        }

        private Func<object, string> IdGetter(Type type, InjectAttribute attr)
        {
            if (attr.FromMember)
            {
                var field = type.GetField(attr.Name, nameBindingFlags);

                if (field != null)
                    return obj => field.GetValue(obj).ToString();

                var prop = type.GetProperty(attr.Name, nameBindingFlags);

                if (prop != null)
                    return obj => prop.GetValue(obj).ToString();

                throw new InjectException("Cannot find member " + attr.Name);
            }
            else
            {
                return _ => attr.Name;
            }
        }

        public struct InjectableProperty
        {
            public readonly Type Type;
            public readonly Func<object, string> IdGetter;
            public readonly Action<object, object> Setter;

            public InjectableProperty(Type type, Func<object, string> idGetter, Action<object, object> setter)
            {
                Type = type;
                IdGetter = idGetter;
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
