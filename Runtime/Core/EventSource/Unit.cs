// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    [Serializable]
    public readonly struct Unit : IEquatable<Unit>
    {
        public static Unit Default => default;

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit other && Equals(other);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
