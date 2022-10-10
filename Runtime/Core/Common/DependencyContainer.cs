// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public class DependencyContainer : IDependencyContainer
    {
        // type -> constructor
        // public Dictionary<Type, Func<object>> _builders = new Dictionary<Type, Func<object>>();

        // type -> instance
        private readonly Dictionary<(Type, string), object> _instances = new Dictionary<(Type, string), object>();

        // direct parent to current container
        private IDependencyContainer _parent;

        public DependencyContainer()
        {
            Reset();
        }

        internal void SetParent(IDependencyContainer parent)
        {
            _parent = parent;
        }

        internal void Reset()
        {
            _parent = null;
            // _builders.Clear();
            _instances.Clear();

            // container itself is always binded
            Add<IDependencyContainer>(this);
        }

        public object Resolve(Type type, string id)
        {
            if (_instances.TryGetValue((type, id), out var instance))
                return instance;

            // if (_builders.TryGetValue(type, out var builder))
            // {
            //     instance = builder();
            //     _instances.Add(type, instance);
            //     return instance;
            // }

            // failed to resolve dependency
            if (_parent == null)
                return null;

            // tail call would be optimized
            return _parent.Resolve(type, id);
        }

        internal void Add<T>(T instance)
        {
            Add(typeof(T), null, instance);
        }

        internal void Add<T>(string name, T instance)
        {
            Add(typeof(T), name, instance);
        }

        private void Add(Type type, string name, object instance)
        {
            _instances.Add((type, name), instance);
        }

        // public void Bind<T>() where T : new()
        // {
        //     _builders.Add(typeof(T), () => new T());
        // }

        // public void Bind<T, TImpl>() where TImpl : T, new()
        // {
        //     _builders.Add(typeof(T), () => new TImpl());
        // }
    }
}