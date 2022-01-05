//
//  TapSDKSuiteComponent.h
//  TapSDKSuiteKit
//
//  Created by Bottle K on 2022/1/4.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM (NSInteger, TapSDKSuiteComponentType) {
    TapSDKSuiteComponentTypeMoment,
    TapSDKSuiteComponentTypeFirend,
    TapSDKSuiteComponentTypeAchievement,
    TapSDKSuiteComponentTypeChat,
    TapSDKSuiteComponentTypeLeaderboard
};

FOUNDATION_EXPORT NSString *const TapFloatCellClickedNotification;

@interface TapSDKSuiteComponent : NSObject
@property (nonatomic, assign) TapSDKSuiteComponentType type;
@property (nonatomic, strong) NSString *title;
@property (nonatomic, strong) UIImage *icon;

- (instancetype)initWithType:(TapSDKSuiteComponentType)type;

- (instancetype)initWithType:(TapSDKSuiteComponentType)type title:(NSString *_Nullable)title;

- (instancetype)initWithType:(TapSDKSuiteComponentType)type title:(NSString *_Nullable)title icon:(UIImage *_Nullable)icon;
@end

NS_ASSUME_NONNULL_END
