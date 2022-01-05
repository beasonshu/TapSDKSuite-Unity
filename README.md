# TapSDKSuite-Unity

# 说明
Unity 模块是通过引入 iOS 和 Android 模块后增加桥接文件打包出的 `.unitypackage`，方便以 Unity 开发的游戏直接引入。其他引擎/平台的游戏可以通过 iOS/Android 原生的方式接入，详见 iOS/Android 各模块接入文档。

## 1.接入SDK
Unity 开发环境:2018.4.36f1

导入 `TapSDKSuiteForUnity.unitypackage`

**检查 Unity 输出的 Xcode 工程**

1. 请确保设置 `Xcode` - `General` - `Frameworks, Libraries, and Embedded Content` 中的 `TapSDKSuiteKit.framework` 为 `Do Not Embed`。
2. 如果编译报错找不到头文件或者模块，请确保 `Xcode`-`Build Settings` - `Framework Search Paths` 中的路径以保证 Xcode 正常编译。
3. 开始代码接入
4. 将 TapSDKSuite-Unity/Assets/Plugins/iOS/Resource/TapSDKSuiteResource.bundle 拷贝到游戏项目下 (如果unity项目没有正确导入 TapSDKSuiteResource.bundle)

> 请确保以上步骤正确执行。

### 1.1 iOS
- iOS Deployment Target 最低支持 iOS 10.0
- Xcode 13.0 beta 5 编译 

### 1.2 Android
最低支持安卓版本 5.0

## 2.使用说明
TapSDKSuite提供了一个可以快速展示TapSDK功能的悬浮窗口。

### 2.1 配置需要使用的功能列表和回调
TapComponent 参数说明：
<a name="参数说明"></a>
参数名称 | 参数说明 
--- | ---
type | enum {MOMENTS(0), FRIENDS(1), ACHIEVEMENT(2), CHAT(3), LEADERBOARD(4), Other(5 ~ Integer.MAX)}
componetName | const {"Moments", "Friends", "Achievement", "Chat", "Leaderboard", ${anyOtherWords}...}
drawableName | Android use (the file name in drawable folder)
imageName | iOS use (the file name in TapSDKSuiteResource.bundle)

TapSDKSuite 回调参数说明:

<a name="参数说明"></a>
参数名称 | 参数说明
--- | ---
type |  参考TapComponent的定义
componetName |  参考TapComponent的定义

示例代码:
```
    SerializableList<TapComponent> componentParamsListWrapper = new SerializableList<TapComponent>();
    
    //eg1. 创建已经定义的功能类型
    TapComponent tapComponent1 = new TapComponent();
    tapComponent1.type = i % 5;
    componentParamsListWrapper.list.Add(tapComponent1);

    //eg2. 创建自定义的功能类型
    TapComponent tapComponent2 = new TapComponent();
    tapComponent2.type = 6;
    tapComponent2.componentName = "custom name";
    // Android use (the file name in drawable folder)
    tapComponent2.drawableName = "tapsdk_suite_ic_friends_default";
    iOS use (the file name in TapSDKSuiteResource.bundle)
    tapComponent2.imageName = "ic_friend";
    componentParamsListWrapper.list.Add(tapComponent2);

    TapSDKSuiteKit.ConfigComponents(componentParamsListWrapper, (floatingWindowCallbackData) => {
        // handle callback
    }, (exception) => {
        // handle exception 
    });
```

### 2.2 启动悬浮窗
悬浮窗分两种状态（1、收起，2、展开）
- 收起状态下点触小滑块展示详细的功能列表
- 展开状态下点击屏幕空白区域收起详细列表

示例代码:
```
TapSDKSuiteKit.Enable();
```

### 2.3 关闭悬浮窗
```
TapSDKSuiteKit.Disable();
```

> 注意点
SDK 暂时不支持自由旋转，在 enable 后只能保证当前展示情况，如果旋转的话需要 调用 diable 后重新 enable.

## License
TapSDKSuite is released under the MIT license. See [LICENSE](LICENSE) for details.