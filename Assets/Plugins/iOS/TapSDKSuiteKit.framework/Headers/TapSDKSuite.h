//
//  TapSDKSuite.h
//  TapSDKSuiteKit
//
//  Created by Bottle K on 2022/1/4.
//

#import <Foundation/Foundation.h>
#import <TapSDKSuiteKit/TapSDKSuiteDelegate.h>

NS_ASSUME_NONNULL_BEGIN

@interface TapSDKSuite : NSObject
@property (nonatomic, strong) NSArray<TapSDKSuiteComponent *> *componentArray;
@property (nonatomic, weak) id<TapSDKSuiteDelegate> delegate;

+ (instancetype)new NS_UNAVAILABLE;

- (instancetype)init NS_UNAVAILABLE;

+ (instancetype)shareInstance;

+ (void)enable;

+ (void)disable;

+ (BOOL)isShowing;
@end

NS_ASSUME_NONNULL_END
