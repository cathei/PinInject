// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using UnityEngine;

namespace Cathei.PinInject
{
    [Serializable]
    public struct StringEnum<T> : IEquatable<T>, IEquatable<StringEnum<T>>, ISerializationCallbackReceiver
        where T : struct, Enum
    {
        public StringEnum(T value) : this()
        {
            _value = value;
        }

        [SerializeField]
        private string _stringValue;

        private T _value;

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void OnBeforeSerialize()
        {
            _stringValue = _value.ToString();
        }

        public void OnAfterDeserialize()
        {
            if (Enum.TryParse(_stringValue, out T value))
                _value = value;
        }

        public static implicit operator T(StringEnum<T> obj)
        {
            return obj._value;
        }

        public static implicit operator StringEnum<T>(T value)
        {
            return new StringEnum<T>(value);
        }

        public bool Equals(T other)
        {
            return _value.Equals(other);
        }

        public bool Equals(StringEnum<T> other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (obj is StringEnum<T> other)
                return Equals(other);

            if (obj is T otherEnum)
                return Equals(otherEnum);

            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}