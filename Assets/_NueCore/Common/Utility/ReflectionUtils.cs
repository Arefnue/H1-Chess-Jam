using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _NueCore.Common.Utility
{
    public static class ReflectionUtils
    {
        #region Type Finders
        public static IEnumerable<Type> GetSubtypes(Type parentType)
        {
            Assembly assembly = Assembly.GetAssembly(parentType);
            return (object) assembly == null ? (IEnumerable<Type>) null : ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(parentType)));
        }

        public static IEnumerable<Type> GetSubtypes<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof (T));
            return (object) assembly == null ? (IEnumerable<Type>) null : ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof (T))));
        }

        public static IEnumerable<Type> GetTypesDirect<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof (T));
            return (object) assembly == null ? null: assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && t.IsAbstract == false);
        }
        #endregion
        
        #region Item Getters
        public static T GetItem<T>(Type type) where T : class
        {
            T item = Activator.CreateInstance(type) as T;
            return item;
        }
        
        public static List<T> GetItemsList<T>() where T : class
        {
            var types = GetSubtypes<T>();
            List<T> refList = new List<T>();
            foreach (var t in types)
            {
                T item = Activator.CreateInstance(t) as T;
                refList.Add(item);
            }
            return refList;
        }
        
        public static List<Type> GetItemsList(Type parentType)
        {
            var types = GetSubtypes(parentType);
            List<Type> refList = new List<Type>();
            foreach (var t in types)
            {
                Type item = Activator.CreateInstance(t) as Type;
                refList.Add(item);
            }
            return refList;
        }
        #endregion
    }
}