namespace _NueExtras.PoolSystem
{
    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }
}