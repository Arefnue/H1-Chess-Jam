using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.AnimationHelpers
{
    public class AnimatePosition : AnimateBase
    {
        [SerializeField] private List<Vector3> moveVectors;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private UpdateType updateType =UpdateType.Normal;
        [SerializeField] private bool reverse;
        
        private Vector3 _initalVector;
        private void Awake()
        {
            _initalVector = transform.position;
        }

        protected override Sequence PlaySequence()
        {
            var seq = base.PlaySequence();
            if (moveVectors.Count>0)
            {
                var perDuration = Duration / moveVectors.Count;
                for (int i = 0; i < moveVectors.Count; i++)
                {
                    var vector = moveVectors[i] + _initalVector;
                    var tw = transform.DOMove(vector, perDuration).SetEase(ease).SetUpdate(updateType);
                    seq.Append(tw);
                }
                
                if (reverse)
                {
                    var tw = transform.DOMove(_initalVector, perDuration).SetEase(ease).SetUpdate(updateType);
                    seq.Append(tw);
                }
            }
            return seq;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (moveVectors == null || moveVectors.Count == 0) return;
            var basePos = transform.position;
            var startPos = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(basePos, 0.25f);
            Handles.color = Color.black;
            Handles.Label(basePos+Vector3.up,"0");
            for (var i = 0; i < moveVectors.Count; i++)
            {
                var vector = moveVectors[i];
                Gizmos.color = Color.green;
                Gizmos.DrawLine(basePos, basePos + vector);
                basePos += vector;
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(basePos, 0.25f);
                Handles.Label(basePos+Vector3.up,(i+1).ToString());
            }

            if (reverse)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(basePos, startPos);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(startPos, 0.25f);
                Handles.Label(startPos+Vector3.up,moveVectors.Count.ToString());
            }
        }
#endif
    }
}