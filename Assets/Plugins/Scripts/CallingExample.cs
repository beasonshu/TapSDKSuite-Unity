using System.Collections;
using System.Collections.Generic;
using System;
using Plugins.TapSDKSuiteKit;
using UnityEngine;
using TapTap.Common;
using TapTap.Login;
using TapTap.Moment;


public class CallingExample : MonoBehaviour
{

    private string componentNum = "5";

    private string logText = "";

    private bool isInit = false;

    private string gameIdentifier = "0RiAlMny7jiz086FaU";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        int xOffset = 50;
        int yOffset = 50;

        int standardButtonWidth = 200;
        int standardButtonHeight = 80;

        int standardLabelWidth = 200;
        int standardLabelHeight = 80;

        int standardVerticalGap = 40;

        string[] componentNames = { "Moments", "Friends", "Achievement", "Chat", "Leaderboard" };


        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 30
        };

        GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 30
        };


        if (GUI.Button(new Rect(xOffset, yOffset, standardButtonWidth, standardButtonHeight), "横屏初始化", myButtonStyle))
        {
            Screen.orientation = ScreenOrientation.Landscape;
            InitConfig();
        }

        yOffset += (standardButtonHeight + standardVerticalGap);

        if (GUI.Button(new Rect(xOffset, yOffset, standardButtonWidth, standardButtonHeight), "竖屏初始化", myButtonStyle))
        {
            Screen.orientation = ScreenOrientation.Portrait;
            InitConfig();
        }

        yOffset += (standardButtonHeight + standardVerticalGap);

        componentNum = GUI.TextArea(new Rect(xOffset, yOffset, standardLabelWidth, standardLabelHeight)
            , componentNum, myButtonStyle);

        yOffset += (standardLabelHeight + standardVerticalGap);

        if (GUI.Button(new Rect(xOffset, yOffset, standardButtonWidth, standardButtonHeight), "启动浮层", myButtonStyle))
        {
            if (!checkInit())
            {
                return;
            }

            long num = long.Parse(componentNum);
            SerializableList<TapComponent> componentParamsListWrapper = new SerializableList<TapComponent>();
            componentParamsListWrapper.list = new List<TapComponent>();
            for (int i = 0; i < num; i++)
            {
                if (i != num - 1)
                {
                    TapComponent tapComponent = new TapComponent();
                    tapComponent.type = i % 5;
                    tapComponent.componentName = componentNames[i % 5];
                    componentParamsListWrapper.list.Add(tapComponent);
                } else
                {
                    TapComponent tapComponent = new TapComponent();
                    tapComponent.type = 6;
                    tapComponent.componentName = "custom name";
                    tapComponent.drawableName = "tapsdk_suite_ic_friends_default";
                    tapComponent.imageName = "ic_friend";
                    componentParamsListWrapper.list.Add(tapComponent);
                }
            }

            TapSDKSuiteKit.ConfigComponents(componentParamsListWrapper, (floatingWindowCallbackData) =>
            {
                Debug.Log($"FloatingWindow: click componentName = {floatingWindowCallbackData.componentName}");

                logText = "FloatingWindow: click componentName = " + floatingWindowCallbackData.componentName;
                if (floatingWindowCallbackData.type == 0) {
                    Debug.Log($"FloatingWindow: click componentName = {floatingWindowCallbackData.type}");
                    logText = "FloatingWindow: click componentName real = " + floatingWindowCallbackData.componentName;
                    TapMoment.Open(Orientation.ORIENTATION_DEFAULT);
                }
            }, (exception) =>
            {
                Debug.Log($"AntiAddictionCallback: exception = {exception}");
                logText = "FloatingWindow: exception = " + exception;
            });
            TapSDKSuiteKit.Enable();
        }

        yOffset += (standardLabelHeight + standardVerticalGap);
        if (GUI.Button(new Rect(xOffset, yOffset, standardButtonWidth, standardButtonHeight), "收起浮层", myButtonStyle))
        {
            if (!checkInit())
            {
                return;
            }
            TapSDKSuiteKit.Disable();
        }

        // 提示消息框
        GUI.Label(new Rect(550, 600, 500, 300), logText, myLabelStyle);
    }


    private void InitConfig()
    {
        if (isInit)
        {
            logText = "请勿重复初始化";
            return;
        }
        try
        {
            isInit = true;
            Debug.Log("CallingExample - started");

            TapLogin.Init(gameIdentifier);
            TapMoment.Init(gameIdentifier);
        }
        catch (Exception exception)
        {
            isInit = false;
            Debug.LogError(exception);
        }
    }

    private bool checkInit()
    {
        if (!isInit)
        {
            logText = "请先初始化";
        }

        return isInit;
    }
}
