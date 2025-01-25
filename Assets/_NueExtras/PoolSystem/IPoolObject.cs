using System;

namespace _NueExtras.PoolSystem
{
    public interface IPoolObject<out T>
    {
        void BuildPoolObject(Action<T> returnAction);
        void ReturnPoolObject();
    }
}