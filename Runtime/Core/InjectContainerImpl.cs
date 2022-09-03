using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public class InjectContainerImpl : IInjectContainer, IInjectBinder
    {
        // type -> constructor
        // public Dictionary<Type, Func<object>> _builders = new Dictionary<Type, Func<object>>();

        // type -> instance
        public Dictionary<(Type, string), object> _instances = new Dictionary<(Type, string), object>();

        // direct parent to current container
        private IInjectContainer _parent;

        public InjectContainerImpl()
        {
            Reset();
        }

        internal void SetParent(IInjectContainer parent)
        {
            _parent = parent;
        }

        internal void Reset()
        {
            _parent = null;
            // _builders.Clear();
            _instances.Clear();

            // container itself is always binded
            Bind<IInjectContainer>(this);
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

        public void Bind<T>(T instance)
        {
            _instances.Add((typeof(T), null), instance);
        }

        public void Bind<T>(string name, T instance)
        {
            _instances.Add((typeof(T), name), instance);
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