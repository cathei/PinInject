// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject
{
    public interface IDependencyContainer
    {
        object Resolve(Type type, string id);
    }
}