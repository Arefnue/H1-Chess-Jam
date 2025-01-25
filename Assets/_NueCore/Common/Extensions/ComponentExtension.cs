using UnityEngine;

namespace _NueCore.Common.Extensions
{
    public static class ComponentExtension
    {
        public static T AddComponent<T>(this Component component) where T : Component =>
            component.gameObject.AddComponent<T>();
        
        public static T GetOrAddComponent<T>(this Component component) where T : Component =>
            component.GetComponent<T>() ?? component.AddComponent<T>();
        
        public static bool HasComponent<T>(this Component component) where T : Component =>
            component.GetComponent<T>() != null;
    }
}