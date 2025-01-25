using System;
using UnityEngine;

namespace _NueCore.Common.Extensions
{
    public static class TransformExtension
    {
        public static void AddChildren(this Transform transform, GameObject[] children) =>
            Array.ForEach(children, child => child.transform.parent = transform);
        
        public static void AddChildren(this Transform transform, Component[] children) =>
            Array.ForEach(children, child => child.transform.parent = transform);

        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        public static void ResetChildPositions(this Transform transform, bool recursive = false)
        {
            foreach (Transform child in transform)
            {
                child.position = Vector3.zero;

                if (recursive) child.ResetChildPositions(recursive);
            }
        }
        
        public static void SetChildLayers(this Transform transform, string layerName, bool recursive = false)
        {
            var layer = LayerMask.NameToLayer(layerName);
            SetChildLayersHelper(transform, layer, recursive);
        }
        static void SetChildLayersHelper(Transform transform, int layer, bool recursive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = layer;

                if (recursive) SetChildLayersHelper(child, layer, recursive);
            }
        }
        
        public static void SetX(this Transform transform, float x) =>
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        
        public static void SetY(this Transform transform, float y) =>
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        
        public static void SetZ(this Transform transform, float z) =>
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        
        public static int CloserEdge(this Transform transform, Camera camera, int width, int height)
        {
            //edge points according to the screen/camera
            var worldPointTop = camera.ScreenToWorldPoint(new Vector3(width / 2, height));
            var worldPointBot = camera.ScreenToWorldPoint(new Vector3(width / 2, 0));

            //distance from the pivot to the screen edge
            var deltaTop = Vector2.Distance(worldPointTop, transform.position);
            var deltaBottom = Vector2.Distance(worldPointBot, transform.position);

            return deltaBottom <= deltaTop ? 1 : -1;
        }
    }
}