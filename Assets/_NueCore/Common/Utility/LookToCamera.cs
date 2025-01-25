using System;
using _NueCore.UpdateSystem;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public class LookToCamera : MonoBehaviour,ILateUpdateable
    {
        [SerializeField] private bool rotateAround;
        [SerializeField] private float rotateSpeed = 120f;
        [SerializeField] private Vector3 rotateAxis = Vector3.up;
        private Camera Camera => CameraStatic.MainCamera;
        private Transform _transform;

        private void OnEnable()
        {
            UpdateManager.Register(this);
        }
        
        private void OnDisable()
        {
            UpdateManager.Unregister(this);
        }

        public void ManagedLateUpdate()
        {
            if (!_transform) _transform = transform;

            _transform.LookAt(_transform.position + Camera.transform.rotation * Vector3.forward,
                Camera.transform.rotation * Vector3.up);

            if (rotateAround)
            {
                _transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
            }
        }
    }
}