using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.TapSDKSuiteKit
{
    [Serializable]
    public class TapComponent
    {
        public int type;
        public string componentName;
        // android use (the file name in drawable folder)
        public string drawableName;
        // ios use (the file name in TapSDKSuiteResource.bundle)
        public string imageName;
    }

    [System.Serializable]
    public class SerializableList<T>
    {
        public List<T> list;
    }

    [Serializable]
    public class FloatingWindowCallbackData
    {
        public int type;
        public string componentName;
    }

    public static class TapComponentType
    {
        public const int MOMENTS = 0;
        public const int FRIENDS = 1;
        public const int ACHIEVEMENT = 2;
        public const int CHAT = 3;
        public const int LEADERBOARD = 4;
    }

    public static class TapSDKSuiteKit
    {
        private const string UNITY_SDK_VERSION = "1.0.0";
        private const string GAME_OBJECT_NAME = "PluginBridge";

        private static GameObject gameObject;

        private const string JAVA_OBJECT_NAME = "com.tapsdk.suite.NativeTapSDKSuiteKitPlugin";

        private static AndroidJavaObject androidJavaNativeTapFloatingWindow;

        private static Action<FloatingWindowCallbackData> handleAsyncFloatingWindowMsg;
        private static Action<string> handleAsyncFloatingWindowMsgException;

        #if UNITY_IOS
        [DllImport("__Internal")]
        #endif
        private static extern void configComponents(string componentListJsonObject);

        #if UNITY_IOS
        [DllImport("__Internal")]
        #endif
        private static extern void enable();

        #if UNITY_IOS
        [DllImport("__Internal")]
        #endif
        private static extern void disable();

        private class PlatformNotSupportedException : Exception
        {
            public PlatformNotSupportedException() : base()
            {

            }
        }

        static TapSDKSuiteKit()
        {
            gameObject = new GameObject();
            gameObject.name = GAME_OBJECT_NAME;

            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            gameObject.AddComponent<NativeFloatingWindowCallbackHandler>();

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    androidJavaNativeTapFloatingWindow = new AndroidJavaObject(JAVA_OBJECT_NAME);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    break;
                default:
                    throw new PlatformNotSupportedException();

            }
        }

        private class NativeFloatingWindowCallbackHandler : MonoBehaviour
        {
            private void HandleException(string exception)
            {
                Debug.Log("HandleException floatingWindowCallbackDataJSON:" + exception);
                handleAsyncFloatingWindowMsgException?.Invoke(exception);
            }

            private void HandleFloatingWindowCallbackDataMsg(string floatingWindowCallbackDataJSON)
            {
                Debug.Log("HandleFloatingWindowCallbackMsg floatingWindowCallbackDataJSON:" + floatingWindowCallbackDataJSON);
                var floatingWindowCallbackOriginData = JsonUtility.FromJson<FloatingWindowCallbackData>(floatingWindowCallbackDataJSON);
                Debug.Log("HandleFloatingWindowCallbackMsg type:" + floatingWindowCallbackOriginData.type);
                Debug.Log("HandleFloatingWindowCallbackMsg componentName:" + floatingWindowCallbackOriginData.componentName);
                
                var result = new FloatingWindowCallbackData();
                result.type = floatingWindowCallbackOriginData.type;
                result.componentName = floatingWindowCallbackOriginData.componentName;

                
                handleAsyncFloatingWindowMsg?.Invoke(result);
            }
        }

        public static void ConfigComponents(SerializableList<TapComponent> componentParamsListWrapper
            , Action<FloatingWindowCallbackData> handleAsyncFloatingWindowMsg
            , Action<string> handleAsyncFloatingWindowMsgException

            )
        {
            TapSDKSuiteKit.handleAsyncFloatingWindowMsg = handleAsyncFloatingWindowMsg;
            TapSDKSuiteKit.handleAsyncFloatingWindowMsgException = handleAsyncFloatingWindowMsgException;

            string componentListJsonObject = JsonUtility.ToJson(componentParamsListWrapper);
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PerformAndroidConfigComponents(componentListJsonObject);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    PerformiOSConfigComponents(componentListJsonObject);
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void Enable()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PerforAndroidEnableFloatingWindow();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    PerformiOSEnableFloatingWindow();
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void Disable()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PerformAndroidDisableFloatingWindow();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    PerformiOSDisableFloatingWindow();
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        /*
         * ------------------
         * Internal Metthods(Android)
         * ------------------
         */
        private static void PerformAndroidConfigComponents(string componentListJsonObject)
        {
            Debug.Log("Android configuration component list:" + componentListJsonObject);
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            androidJavaNativeTapFloatingWindow.Call("configComponents"
                , componentListJsonObject);
        }

        private static void PerforAndroidEnableFloatingWindow()
        {
            Debug.Log("Android enable floating window calling");
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            androidJavaNativeTapFloatingWindow.Call("enableFloatingWindow", unityActivity);
        } 

        private static void PerformAndroidDisableFloatingWindow()
        {
            Debug.Log("Android disable floating window calling");
            androidJavaNativeTapFloatingWindow.Call("disableFloatingWindow");
        }

        /*
         * ------------------
         * Internal Metthods(iOS)
         * ------------------
         */
         private static void PerformiOSConfigComponents(string componentListJsonObject)
         {
             Debug.Log("iOS configuration calling list:" + componentListJsonObject);
             configComponents(componentListJsonObject);
         }

         private static void PerformiOSEnableFloatingWindow()
         {
             Debug.Log("iOS enable floating window");
             enable();
         }

         private static void PerformiOSDisableFloatingWindow()
         {
             Debug.Log("iOS disable floating window");
             disable();
         }
    }
}
