using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace _NueCore.ManagerSystem.SingletonSystems
{
    public static class SingletonExtension
    {
        private static readonly object SyncLockObjects = new object();
        private static readonly object SyncLockMono = new object();
        private static readonly object SyncLockInterfaces = new object();

        private static readonly Dictionary<Type, object> ObjectInstances =
            new Dictionary<Type, object>();

        private static readonly Dictionary<Type, Object> MonoInstances =
            new Dictionary<Type, Object>();

        private static readonly Dictionary<Type, ICollection<object>> InterfaceInstances =
            new Dictionary<Type, ICollection<object>>();


        #region Methods
        public static T Get<T>()
        {
            var type = typeof(T);
            if (type.IsUnityObject())
                return (T) Convert.ChangeType(GetMono(type), type);

            if (type.IsDotNetObject())
                return GetObject<T>();

            throw new NotSupportedException($"{type}_{nameof(SingletonExtension)} mismatched!");
        }

        public static void Remove<T>()
        {
            var type = typeof(T);
            if (type == typeof(Object))
            {
                lock (SyncLockMono)
                {
                    if (!ObjectInstances.ContainsKey(type)) return;
                    ObjectInstances.Remove(type);
                }
            }
            else
            {
                lock (SyncLockObjects)
                {
                    if (!MonoInstances.ContainsKey(type)) return;
                    MonoInstances.Remove(type);
                }
            }
        }

        public static void RemoveAll()
        {
            ObjectInstances.Clear();
            MonoInstances.Clear();
            lock (SyncLockInterfaces)
                InterfaceInstances.Clear();
        }

        private static T GetObject<T>()
        {
            lock (SyncLockObjects)
            {
                var type = typeof(T);
                if (ObjectInstances.TryGetValue(type, out var instance))
                    return (T) instance;

                instance = Activator.CreateInstance<T>();
                
                ObjectInstances.Add(type, instance);

                return (T) instance;
            }
        }

        private static UnityEngine.Object GetMono(Type type)
        {
            lock (SyncLockMono)
            {
                if (MonoInstances.TryGetValue(type, out var instance))
                    return instance;

                instance = Object.FindObjectOfType(type);
                if (instance == null)
                    return null;
                
                MonoInstances.Add(type, instance);

                return instance;
            }
        }
        #endregion
        
        #region Type Checks
        private static readonly Type UnityObject = typeof(UnityEngine.Object);
        private static readonly Type UnityMonoBehavior = typeof(UnityEngine.MonoBehaviour);
        private static readonly Type DotNetObject = typeof(object);

        private static bool IsUnityObject(this Type type)
        {
            return type.IsSubclassOf(UnityObject);
        }

        private static bool IsUnityMonoBehavior(this Type type)
        {
            return type.IsSubclassOf(UnityMonoBehavior);
        }

        private static bool IsDotNetObject(this Type type)
        {
            return type.IsSubclassOf(DotNetObject);
        }

        private static bool CheckIfImplements<T>(this Type type)
        {
            return type.IsAssignableFrom(typeof(T));
        }
        #endregion
    }
}