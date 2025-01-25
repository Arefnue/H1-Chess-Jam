using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _NueCore.SceneSystem
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private bool useTransitionTarget;
        [SerializeField,HideIf(nameof(useTransitionTarget))] private SceneEnums targetScene;
        [SerializeField] private bool changeAtStart;
        [SerializeField] private bool isAsync;
        [SerializeField,ShowIf(nameof(isAsync))] private UnityEvent<float> asyncProgressUnityEvent;

        public SceneEnums TargetScene => useTransitionTarget ? SceneStatic.TransitionTarget: targetScene;

        private void Start()
        {
            if (changeAtStart)
                ChangeScene();
        }

        public void ChangeScene()
        {
            if (isAsync)
            {
                var asyncOps = SceneStatic.ChangeSceneAsync(TargetScene);
                StartCoroutine(LoadYourAsyncScene(asyncOps));
            }
            else
            {
                SceneStatic.ChangeScene(TargetScene);
            }
        }
        
        private IEnumerator LoadYourAsyncScene(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                asyncProgressUnityEvent?.Invoke(asyncOperation.progress);
                yield return null;
            }
        }
        
    }
}