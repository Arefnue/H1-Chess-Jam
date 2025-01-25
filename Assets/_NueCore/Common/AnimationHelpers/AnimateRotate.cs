using DG.Tweening;
using UnityEngine;

namespace _NueCore.Common.AnimationHelpers
{
    public class AnimateRotate : AnimateBase
    {
        [SerializeField] private Vector3 rotateVector;
        [SerializeField] private float angle = 120;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private UpdateType updateType =UpdateType.Normal;
        protected override Sequence PlaySequence()
        {
            var seq = base.PlaySequence();
            var lastTick = 0f;
            seq.Append(DOVirtual.Float(0,1,1, value =>
            {
                var t = value - lastTick;
                lastTick = value;
                transform.Rotate(rotateVector, angle * t);
            }).SetEase(ease).SetUpdate(updateType));
            seq.OnStepComplete(() =>
            {
                lastTick = 0;
            });
            return seq;
        }
    }
}