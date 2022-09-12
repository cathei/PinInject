// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject
{
    public abstract class InjectKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, IPostInjectHandler
        where TItem : class
    {
        [Inject]
        private IInjectContainer _container;

        protected InjectKeyedCollection() : base() { }
        protected InjectKeyedCollection(IEqualityComparer<TKey> comparer) : base(comparer) { }
        protected InjectKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) { }

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

