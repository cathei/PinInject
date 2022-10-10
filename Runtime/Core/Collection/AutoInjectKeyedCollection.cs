// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject
{
    public abstract class AutoInjectKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, IPostInjectHandler
        where TItem : class
    {
        [Inject]
        private IDependencyContainer _container;

        protected AutoInjectKeyedCollection() : base() { }
        protected AutoInjectKeyedCollection(IEqualityComparer<TKey> comparer) : base(comparer) { }
        protected AutoInjectKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) { }

        public virtual void PostInject()
        {
            foreach (TItem item in this)
                Pin.Inject(item, _container);
        }

        protected override void InsertItem(int index, TItem item)
        {
            if (_container != null)
                Pin.Inject(item, _container);

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, TItem item)
        {
            if (_container != null)
                Pin.Inject(item, _container);

            base.SetItem(index, item);
        }
    }
}

