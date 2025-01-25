using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _NueCore.Common.Utility
{
    public static class UIHelper
    {
        public static bool IsMouseOverUI()
        {
            return RayCastAllMouse().Count > 0;
        }
        public static List<RaycastResult> RayCastAllMouse(){
		
            PointerEventData pointerData = new PointerEventData (EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return results;
        }
        
        public static T GetFirstComponentUnderMouse<T>() where T : Component
        {
            List<RaycastResult> results = RayCastAllMouse();
            foreach (var result in results)
            {
                T component = result.gameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
        
        public static Vector2 RotatePoint(Vector2 pos, float angleInDegrees)
        {
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            float cosTheta = Mathf.Cos(angleInRadians);
            float sinTheta = Mathf.Sin(angleInRadians);

            float x = pos.x * cosTheta - pos.y * sinTheta;
            float y = pos.x * sinTheta + pos.y * cosTheta;

            return new Vector2(x, y);
        }

        public static Vector2 RotateWidthHeight(RectTransform rectTransform1, float rotationAngle)
        {
            float originalWidth = rectTransform1.rect.width;
            float originalHeight = rectTransform1.rect.height;

            float angleRad = rotationAngle * Mathf.Deg2Rad;

            float halfWidth = originalWidth / 2;
            float halfHeight = originalHeight / 2;

            float newWidth = Mathf.Abs(originalWidth * Mathf.Cos(angleRad)) +
                             Mathf.Abs(originalHeight * Mathf.Sin(angleRad));
            float newHeight = Mathf.Abs(originalWidth * Mathf.Sin(angleRad)) +
                              Mathf.Abs(originalHeight * Mathf.Cos(angleRad));


            return new Vector2(newWidth, newHeight);
        }

        public static bool IsOverlapping(RectTransform rectTransform1, RectTransform rectTransform2)
        {
            Rect rect1 = GetWorldRect(rectTransform1);
            Rect rect2 = GetWorldRect(rectTransform2);

            return rect1.Overlaps(rect2);
        }

        private static readonly Vector3[] TempCorners = new Vector3[4];
        private static Rect GetWorldRect(RectTransform rt)
        {
            rt.GetWorldCorners(TempCorners);

            Vector3 bottomLeft = TempCorners[0];
            Vector3 topRight = TempCorners[2];

            return new Rect(bottomLeft, topRight - bottomLeft);
        }
        public static float GetOverlapPercentage(RectTransform rectTransform1, RectTransform rectTransform2)
        {
            Rect rect1 = GetWorldRect(rectTransform1);
            Rect rect2 = GetWorldRect(rectTransform2);

            Rect intersection = GetIntersection(rect1, rect2);

            float intersectionArea = intersection.width * intersection.height;
            float rect1Area = rect1.width * rect1.height;

            return intersectionArea / rect1Area;
        }
        
        public static bool IsMouseIntersectingRect(RectTransform rectTransform)
        {
            Rect rect = GetWorldRect(rectTransform);
            Vector2 mousePosition = Input.mousePosition;
            return rect.Contains(mousePosition);
        }

        private static Rect GetIntersection(Rect rect1, Rect rect2)
        {
            float xMin = Mathf.Max(rect1.xMin, rect2.xMin);
            float xMax = Mathf.Min(rect1.xMax, rect2.xMax);
            float yMin = Mathf.Max(rect1.yMin, rect2.yMin);
            float yMax = Mathf.Min(rect1.yMax, rect2.yMax);

            if (xMax >= xMin && yMax >= yMin)
            {
                return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
            }

            return Rect.zero;
        }
    }
}