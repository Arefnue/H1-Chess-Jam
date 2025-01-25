using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _NueCore.Common.NueLogger
{
    public static class NLogger
    {
    
#if UNITY_EDITOR
        public const bool EnableDebug =  true;
#else
    public const bool EnableDebug =  false;
#endif
    
        private static bool CheckNLogStatus()
        {
            return EnableDebug;
        }

        #region Object

        #region Normal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Object content, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            UnityEngine.Debug.Log(msg, content);


            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            UnityEngine.Debug.unityLogger.Log(LogType.Log, msg);


            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Object content, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            switch (logType)
            {
                case LogType.Error:
                    UnityEngine.Debug.LogError(msg, content);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(msg, content);
                    break;
                case LogType.Log:
                    UnityEngine.Debug.Log(msg, content);
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            UnityEngine.Debug.unityLogger.Log(logType, msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Object content, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            switch (logType)
            {
                case LogType.Error:
                    UnityEngine.Debug.LogError(msg, content);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(msg, content);
                    break;
                case LogType.Log:
                    UnityEngine.Debug.Log(msg, content);
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj);
            UnityEngine.Debug.unityLogger.Log(logType, msg);

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        #endregion

        #region Full Color

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Color color, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj, color);
            UnityEngine.Debug.unityLogger.Log(LogType.Log, msg);


            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Color color, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj, color);
            UnityEngine.Debug.unityLogger.Log(logType, msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Color color, Object content)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj, color);
            UnityEngine.Debug.Log(msg, content);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this object obj, Color color, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(obj, color);
            UnityEngine.Debug.unityLogger.Log(logType, msg);

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        #endregion

        #endregion

        #region String

        #region Normal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Object content, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            UnityEngine.Debug.Log(msg, content);

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            UnityEngine.Debug.unityLogger.Log(LogType.Log, msg);


            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Object content, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            switch (logType)
            {
                case LogType.Error:
                    UnityEngine.Debug.LogError(msg, content);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(msg, content);
                    break;
                case LogType.Log:
                    UnityEngine.Debug.Log(msg, content);
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            UnityEngine.Debug.unityLogger.Log(logType, msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Object content, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            switch (logType)
            {
                case LogType.Error:
                    UnityEngine.Debug.LogError(msg, content);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(msg, content);
                    break;
                case LogType.Log:
                    UnityEngine.Debug.Log(msg, content);
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str);
            UnityEngine.Debug.unityLogger.Log(logType, msg);

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        #endregion

        #region Full Color

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Color color, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str, color);
            UnityEngine.Debug.unityLogger.Log(LogType.Log, msg);


            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Color color, LogType logType)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str, color);
            UnityEngine.Debug.unityLogger.Log(logType, msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Color color, LogType logType, bool editorPause = false)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str, color);
            UnityEngine.Debug.unityLogger.Log(logType, msg);

            if (editorPause)
                UnityEngine.Debug.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NLog(this string str, Color color, Object content)
        {
            if (!CheckNLogStatus())
                return;

            var msg = LogHelper.GetMessage(str, color);
            UnityEngine.Debug.Log(msg, content);
        }

        #endregion

        #endregion
    }
}