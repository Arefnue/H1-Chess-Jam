using _NueCore.ManagerSystem.Core;
using UnityEngine;

namespace _NueCore.FaderSystem
{
    public class FaderManager : NManagerBase
    {
        [SerializeField] private Animator faderAnimator;
        [SerializeField] private Transform faderRoot;
        
        public static FaderManager Instance { get; private set; }

        public Animator FaderAnimator => faderAnimator;

        public Transform FaderRoot => faderRoot;

        #region Setup
        public override void NAwake()
        {
            Instance = InitSingleton<FaderManager>();
            base.NAwake();
        }

        public override void NStart()
        {
            base.NStart();
            var fadeParams = new NFader.FaderParams
            {
                fadeInDuration = 0f,
                fadeOutDuration = 1f,
                waitDuration = 0.25f
            };
            NFader.Fade(FaderTypes.Default,fadeParams);
        }

        #endregion


        public void SetFaderLayer(FaderTypes faderType)
        {
            for (int i = 0; i < faderAnimator.layerCount; i++)
                faderAnimator.SetLayerWeight(i,0);
            faderAnimator.SetLayerWeight((int)faderType,1);
        }
    }
}