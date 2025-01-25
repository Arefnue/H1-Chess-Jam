using System;
using System.Collections.Generic;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class CastHelper
    {
        private static Vector3 DefaultCastPos => Vector3.zero;
        private static readonly LayerMask BlockLayers = LayerMask.GetMask("Ground");
        
        public struct NearestCastInfo<T>
        {
            public T Target;
            public Vector3 CastPos;
            public Vector3 Direction;
            public float MaxRange;
            public float CastRadius;
            public float Delta;
            public LayerMask TargetLayer;
        }
        
        #region RayCast
        public static bool TryRayCast(out RaycastHit hit, Vector3 castPos, Vector3 direction, float range, LayerMask targetLayer)
        {
            return Physics.Raycast(castPos, direction, out hit, range, targetLayer);
        }
        
        public static bool TryRayCast(out RaycastHit hit, Ray ray, float range, LayerMask targetLayer)
        {
            return Physics.Raycast(ray, out hit, range, targetLayer);
        }
        #endregion

        #region Nearest
        public static bool FindNearest<T>(out T target, Vector3 castPos, float radius, LayerMask targetLayer)
        {
            target = default;
            var hits = Physics.SphereCastAll(castPos + new Vector3(0, 1, 0),
                radius,
                Vector3.down,
                Mathf.Infinity,
                targetLayer);
            if (hits.Length <= 0)
                return false;

            var nearest = Mathf.Infinity;
            Collider targetCol = null;
            foreach (var hit in hits)
            {
                var dist = Vector3.Distance(hit.transform.position, castPos);
                if (dist < nearest)
                {
                    nearest = dist;
                    targetCol = hit.collider;
                }
            }

            if (targetCol == null)
            {
                return false;
            }

            target = targetCol.attachedRigidbody.GetComponent<T>();
            if (target == null)
            {
                return false;
            }

            return true;
        }
        public static bool FindNearest<T>(out T target,float radius,LayerMask targetLayer)
        {
            return FindNearest(out target,DefaultCastPos, radius, targetLayer);
        }
        public static bool FindNearestDirection<T>(out T target,Vector3 direction,float range,float radius,LayerMask targetLayer)
        {
            target = default;
            var hits = Physics.SphereCastAll(DefaultCastPos + new Vector3(0, -1, 0),
                radius,
                direction,
                range,
                targetLayer);
            if (hits.Length<=0)
                return false;
            
            var nearest = Mathf.Infinity;
            Collider targetCol = null;
            foreach (var hit in hits)
            {
                var dist = Vector3.Distance(hit.transform.position,DefaultCastPos);
                if (dist<nearest)
                {
                    nearest = dist;
                    targetCol = hit.collider;
                }
            }

            if (targetCol == null)
            {
                return false;
            }
            target = targetCol.attachedRigidbody.GetComponent<T>();
            if (target == null)
            {
                return false;
            }
            return true;
        }
        #endregion
        
        #region FindAll
        public static bool FindAll(ref Collider[] cols, Vector3 castPos, float radius, LayerMask targetLayer,bool clearOlds = true)
        {
            if (clearOlds)
            {
                Array.Clear(cols,0,cols.Length);
            }
            var size = Physics.OverlapSphereNonAlloc(castPos,
                radius, cols, targetLayer);
            if (size <= 0)
                return false;
            
            return true;
        }
        public static bool FindAll<T>(ref List<T> targets, ref Collider[] cols, Vector3 castPos, float radius, LayerMask targetLayer,bool clearOlds = true)
        {
            if (clearOlds)
            {
                targets.Clear();
                Array.Clear(cols,0,cols.Length);
            }
            var size = Physics.OverlapSphereNonAlloc(castPos,
                radius, cols, targetLayer);
            if (size <= 0)
                return false;

            foreach (var col in cols)
            {
                if (col == null)
                    continue;
                if (col.attachedRigidbody == null)
                    continue;
                var comp = col.attachedRigidbody.GetComponent<T>();
                if (comp == null)
                    continue;
                targets.Add(comp);
            }

            if (targets.Count <= 0)
            {
                return false;
            }

            return true;
        }
        public static bool FindAll<T>(ref List<T> targets,ref Collider[] cols, float radius,LayerMask targetLayer,bool clearOlds = true)
        {
            return FindAll(ref targets, ref cols, DefaultCastPos, radius, targetLayer,clearOlds);
        }
        public static bool FindAllCheckObstacle<T>(ref List<T> targets,ref Collider[] cols, float radius,LayerMask targetLayer) where T : MonoBehaviour
        {
            var hasTarget =FindAll(ref targets, ref cols, radius, targetLayer);
            if (!hasTarget)
            {
                return false;
            }

            for (var i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target == null)
                    continue;
                var targetPos = target.transform.position + new Vector3(0, 1.35f, 0);
                var playerPos = DefaultCastPos + new Vector3(0, 1.35f, 0);
                var direction = targetPos - playerPos;
              
                var hasHit = Physics.Raycast(playerPos, direction.normalized, out var hitInfo , direction.magnitude, BlockLayers);
                if (hasHit)
                {
                    if (hitInfo.collider)
                    {
                        targets.Remove(target);
                        i--;
                    }
                }
            }

            if (targets.Count<=0)
            {
                return false;
            }
            return true;
        }
        #endregion
        
    }
}