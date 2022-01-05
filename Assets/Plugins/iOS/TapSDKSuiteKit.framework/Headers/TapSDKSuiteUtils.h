//
//  TapSDKSuiteUtils.h
//  TapSDKSuiteKit
//
//  Created by Bottle K on 2022/1/4.
//

#import <UIKit/UIKit.h>
#import <TapSDKSuiteKit/TapSDKSuiteComponent.h>

NS_ASSUME_NONNULL_BEGIN

@interface TapSDKSuiteUtils : NSObject

+ (NSArray <TapSDKSuiteComponent *> *)currentConfig;

+ (UIImage *)getImageFromBundle:(NSString *)imageName;

+ (UIEdgeInsets)safeAreaInset:(UIView *)view;

+ (UIEdgeInsets)safeAreaInsetOfKeyWindow;

+ (BOOL)isLandscape;

+ (CGFloat)screenShortLength;

+ (CGFloat)screenLongLength;
@end

NS_ASSUME_NONNULL_END
