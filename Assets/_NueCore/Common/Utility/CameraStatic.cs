using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class CameraStatic
    {
        private static Camera _camera;
        public static Camera MainCamera
        {
            get
            {
                if (!_camera)
                {
                    _camera = Camera.main;
                }
                return _camera;
            }
        }
        
        public static Vector3 GetMouseWorldPosition()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = MainCamera.nearClipPlane;
            return MainCamera.ScreenToWorldPoint(mousePos);
        }
        
        public static Vector3 GetMouseWorldPositionWithZ(float z)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = z;
            return MainCamera.ScreenToWorldPoint(mousePos);
        }
        
        public static Ray GetMouseRay()
        {
            return MainCamera.ScreenPointToRay(Input.mousePosition);
        }
        
        public static bool IsPositionOnScreen(Vector3 position)
        {
            Vector3 viewportPoint = MainCamera.WorldToViewportPoint(position);
            return viewportPoint.x is > 0 and < 1 && viewportPoint.y is > 0 and < 1;
        }
    }
}