// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject
{
    /// <summary>
    /// The collection that automatically injects Item on Add or Insert.
    /// Should be used with [Resolve] tag to operate properly.
    /// </summary>
    public class AutoInjectCollection<TItem> : Collection<TItem>, IPostInjectHandler
        where TItem : class
    {
        [Inject]
        private IDependencyContainer _container;

        public AutoInjectCollection() : base() { }
        public AutoInjectCollection(IList<TItem> list) : base(list) { }

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
