using System;
using _NueCore.Common.ReactiveUtils;
using _NueCore.FaderSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NueCore.SceneSystem
{
    public static class SceneStatic
    {
        public const string LobbyScene = "Main Menu";
        public const string GameScene = "Game Scene";
        public const string LoadingScene = "LoadingScene";
        public const string TransitionScene = "TransitionScene";
        
        public static SceneEnums TransitionTarget { get; private set; }

        #region Commons
        private static string GetSceneName(SceneEnums targetEnum)
        {
            return targetEnum switch
            {
                SceneEnums.LobbyScene => LobbyScene,
                SceneEnums.GameScene => GameScene,
                SceneEnums.LoadingScene => LoadingScene,
                SceneEnums.TransitionScene => TransitionScene,
                _ => null
            };
        }
        #endregion
        
        #region Direct
        public static void ChangeScene(string sceneName,SceneChangeParams @params = default)
        {
            RBuss.Publish(new SceneREvents.SceneChangeStartedREvent(sceneName));
            SceneManager.LoadScene(sceneName,@params.loadSceneMode);
            RBuss.Publish(new SceneREvents.SceneChangeFinishedREvent(sceneName));
        }
        public static void ChangeScene(SceneEnums sceneEnum,SceneChangeParams @params = default)
        {
            var sceneName = GetSceneName(sceneEnum);
            ChangeScene(sceneName,@params);
        }

        #endregion

        #region Async
        public static AsyncOperation ChangeSceneAsync(string sceneName,
            SceneChangeParams @params = default)
        {
            RBuss.Publish(new SceneREvents.SceneChangeStartedREvent(sceneName));
            var asyncOps =SceneManager.LoadSceneAsync(sceneName,@params.loadSceneMode);
            asyncOps.completed += operation =>
            {
                @params.onSceneChangedAction?.Invoke();
                RBuss.Publish(new SceneREvents.SceneChangeFinishedREvent(sceneName));
            };
            return asyncOps;
        }
        public static AsyncOperation ChangeSceneAsync(SceneEnums sceneEnum,
            SceneChangeParams @params = default)
        {
            var sceneName = GetSceneName(sceneEnum);
            return ChangeSceneAsync(sceneName, @params);
        }
        public static AsyncOperation ChangeSceneAsyncWithTransition(SceneEnums targetScene,
            SceneChangeParams @params = default)
        {
            TransitionTarget = targetScene;
            var asyncOps = ChangeSceneAsync(SceneEnums.TransitionScene,@params);
            return asyncOps;
        }
        public static bool CheckActiveScene(string sceneName)
        {
            return SceneManager.GetActiveScene().name == sceneName;
        }
        #endregion

        #region Fader
        public static void ChangeSceneAsyncWithFader(SceneEnums sceneEnum,
            NFader.FaderParams faderParams = default,
            SceneChangeParams sceneParams = default)
        {
            var sceneName = GetSceneName(sceneEnum);
            ChangeSceneAsyncWithFader(sceneName,faderParams,sceneParams);
        }
        public static void ChangeSceneAsyncWithFader(string sceneName,
            NFader.FaderParams faderParams = default,
            SceneChangeParams sceneParams = default)
        {
            if (faderParams.fadeInDuration <=0 || faderParams.fadeOutDuration <=0)
                faderParams = NFader.FaderParams.Default;
            
            faderParams.fadeInFinishedAction += () =>
            {
                var ops =ChangeSceneAsync(sceneName, sceneParams);
                ops.completed += operation =>
                {
                    NFader.FadeOut(faderParams.fadeOutType, faderParams);
                };
            };
            NFader.FadeIn(faderParams.fadeInType,faderParams);
            
        }
        public static void ChangeSceneWithFader(string sceneName,
            NFader.FaderParams faderParams = default,
            SceneChangeParams sceneParams = default)
        {
            faderParams.fadeInFinishedAction += () =>
            {
                ChangeScene(sceneName, sceneParams);
            };
            NFader.Fade(FaderTypes.Default,faderParams);
            
        }
        
        public static void ChangeSceneWithFader(SceneEnums sceneEnum,
            NFader.FaderParams faderParams = default,
            SceneChangeParams sceneParams = default)
        { 
            if (faderParams.fadeInDuration <=0 || faderParams.fadeOutDuration <=0)
                faderParams = NFader.FaderParams.Default;
            var sceneName = GetSceneName(sceneEnum);
           ChangeSceneAsyncWithFader(sceneName,faderParams,sceneParams);
        }
        #endregion
        
        #region Structs
        public struct SceneChangeParams
        {
            public LoadSceneMode loadSceneMode;
            public Action onSceneChangedAction;

            public SceneChangeParams(LoadSceneMode loadSceneMode = LoadSceneMode.Single)
            {
                this.loadSceneMode = loadSceneMode;
                onSceneChangedAction = null;
            }
        }
        #endregion
    }
}