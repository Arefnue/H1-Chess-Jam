using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.Common.AnimationHelpers
{
    public abstract class AnimateBase : MonoBehaviour
    {
        [SerializeField,TabGroup("Settings")] private float duration = 1f;
        [SerializeField,TabGroup("Settings")] private bool loop = true;
        [SerializeField,TabGroup("Settings")] private LoopType loopType = LoopType.Restart;

        public float Duration => duration;
        
        protected Sequence Seq { get; private set; }

        protected virtual Sequence PlaySequence()
        {
            Seq?.Kill();
            Seq = DOTween.Sequence();
            Seq.SetLoops(loop ? -1 : 0,loopType);
            Seq.SetLink(gameObject);
            return Seq;
        }
        
        protected virtual void OnEnable()
        {
            PlaySequence();
        }
        
        protected virtual void OnDisable()
        {
            Seq?.Kill();
        }
    }
}