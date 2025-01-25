using UnityEngine;

namespace _NueCore.Common.Attributes
{
    public class RestrictAttribute : PropertyAttribute
    {
        public System.Type RequiredType { get; private set; }
    
        public RestrictAttribute(System.Type type)
        {
            this.RequiredType = type;
        }
    }
}