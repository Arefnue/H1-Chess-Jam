using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.NComponentSystem.TransformComponents
{
    public abstract class NComponentTransformBase : NComponentBase
    {
        [SerializeField,TabGroup("References")] private Transform targetTransform;

        public Transform TargetTransform => targetTransform ? targetTransform : transform;
        
    }
}