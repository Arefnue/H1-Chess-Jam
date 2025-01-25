using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.NComponentSystem.TransformComponents
{
    public class NTransformRotator : NComponentTransformBase
    {
        [SerializeField,TabGroup("Settings")] private Vector3 rotateVector = Vector3.up;
        [SerializeField,TabGroup("Settings")] private float value = 120f;
        
        public override void OnComponentActivated()
        {
            
        }

        public override void OnComponentDeActivated()
        {
            
        }

        public override void OnComponentTicked(float delta)
        {
            TargetTransform.Rotate(rotateVector, value * delta);
        }
    }
}