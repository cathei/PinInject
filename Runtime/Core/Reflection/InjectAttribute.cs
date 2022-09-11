using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
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