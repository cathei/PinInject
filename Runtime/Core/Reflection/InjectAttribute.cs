// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
    /// <summary>
    /// Specify a field or property that will be injected from context.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : PreserveAttribute
    {
        public readonly string Name;
        public readonly bool FromMember;

        public InjectAttribute()
        {
            Name = null;
        }

        public InjectAttribute(string name, bool fromMember = false)
        {
            Name = name;
            FromMember = fromMember;
        }
    }
}