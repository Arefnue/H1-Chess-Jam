using DG.Tweening;
using UnityEngine;

namespace _NueCore.Common.AnimationHelpers
{
    public class AnimateScale : AnimateBase
    {
        [SerializeField] private float targetScale = 1.1f;
        [SerializeField] private Ease outEase = Ease.Linear;
        [SerializeField] private Ease inEase = Ease.Linear;
        
        protected override Sequence PlaySequence()
        {
            var seq = base.PlaySequence();
            var halfDuration = Duration / 2f;
            seq.Append(transform.DOScale(Vector3.one * targetScale, halfDuration)).SetEase(outEase);
            seq.Append(transform.DOScale(Vector3.one, halfDuration)).SetEase(inEase);
            seq.OnComplete(() =>
            {
                transform.localScale = Vector3.one;
            });
            seq.OnKill(() =>
            {
                transform.localScale = Vector3.one;
            });
            return seq;
        }
    }
}
