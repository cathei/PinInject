// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.Internal
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PreserveAttribute : Attribute { }
}