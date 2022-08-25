using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PreserveAttribute : Attribute { }
}