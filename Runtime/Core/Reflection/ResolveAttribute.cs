// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
    /// <summary>
    /// Specify a field or property that will be recursively resolved by context.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResolveAttribute : PreserveAttribute { }
}