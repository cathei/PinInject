#if !UNITY_2021_1_OR_NEWER

namespace UnityEngine
{
    public interface IObjectPool<T>
    {
        int CountInactive { get; }

        T Get();
        void Release(T instance);
        void Clear();
    }
}

#endif


