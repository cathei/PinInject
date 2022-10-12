namespace Cathei.PinInject
{
    public interface IGenericObjectPool<T>
    {
        int CountInactive { get; }

        T Get();
        void Release(T instance);
        void Clear();
    }
}


