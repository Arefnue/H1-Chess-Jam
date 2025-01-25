using UnityEngine;

namespace _NueCore.Common.Extensions
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component =>
            gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component =>
            gameObject.GetComponent<T>() != null;
    }
}
