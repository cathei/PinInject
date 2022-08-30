using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject
{
    public class InjectCollection<TItem> : Collection<TItem>, IPostInjectHandler
        where TItem : class
    {
        [Inject]
        private IInjectContainer _container;

        public InjectCollection() : base() { }
        public InjectCollection(IList<TItem> list) : base(list) { }

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
