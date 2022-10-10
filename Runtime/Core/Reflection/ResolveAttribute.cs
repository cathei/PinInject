// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResolveAttribute : PreserveAttribute { }
}