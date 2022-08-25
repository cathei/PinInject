using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResolveAttribute : PreserveAttribute { }
}