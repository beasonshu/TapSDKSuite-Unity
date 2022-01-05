//
//  TapSDKSuiteDelegate.h
//  TapSDKSuiteKit
//
//  Created by Bottle K on 2022/1/4.
//

#import <Foundation/Foundation.h>
#import <TapSDKSuiteKit/TapSDKSuiteComponent.h>
NS_ASSUME_NONNULL_BEGIN

@protocol TapSDKSuiteDelegate <NSObject>

- (void)onItemClick:(TapSDKSuiteComponent*)component;

@end

NS_ASSUME_NONNULL_END
