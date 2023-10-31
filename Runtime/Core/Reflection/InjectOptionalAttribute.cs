// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject;

namespace Core.Reflection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectOptionalAttribute : InjectAttribute
    {
        public InjectOptionalAttribute() : base(optional: true)
        {
        }
    }
}