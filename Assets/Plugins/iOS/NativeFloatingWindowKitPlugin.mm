#import <TapSDKSuiteKit/TapSDKSuiteKit.h>

char const *GAME_OBJECT = "PluginBridge";

@interface Utility : NSObject
@end

@implementation Utility

+ (NSString *)dictonaryToJson:(NSDictionary *) dictionary {
    NSError* error;

    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:&error];
    if (!jsonData) {
        NSLog(@"Dictonary stringify error: %@", error);
        return @"";
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

+ (NSDictionary *)dictionaryWithJsonString:(NSString *) jsonString {
    if (jsonString == nil) return nil;
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSError *err;
    NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:jsonData
                            options:NSJSONReadingMutableContainers
                            error:&err];
    if (err) {
        NSLog(@"json解析失败: %@", err);
        return nil;
    }
    return dic;
}

@end


@interface NativeTapSDKSuiteKitPlugin : NSObject<TapSDKSuiteDelegate>
@property (nonatomic, strong) NSArray<TapSDKSuiteComponent *> *componentArray;
@end

@implementation NativeTapSDKSuiteKitPlugin

static NativeTapSDKSuiteKitPlugin *_sharedInstance;


+(NativeTapSDKSuiteKitPlugin*)sharedInstance
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _sharedInstance = [[NativeTapSDKSuiteKitPlugin alloc] init];
    });
    return _sharedInstance;
}

-(id)init
{
    self = [super init];
    if (self)
        [self initHelper];
    return self;
}

-(void)initHelper
{
    NSLog(@"Initialized NativeTapSDKSuiteKitPlugin class");
}

-(NSString *)generateUnityUnifyExtras:(NSDictionary *) extras {
    NSMutableDictionary* result = [[NSMutableDictionary alloc] init];

    
    return [Utility dictonaryToJson:result];
}


-(NSString *)generateResultMessage:(int)type title:(NSString *) title icon:(UIImage *) icon
{
    NSInteger typeParam = (NSInteger) type;
    NSDictionary* result = [[NSDictionary alloc] initWithObjectsAndKeys:
                            [NSNumber numberWithUnsignedLong:typeParam],@"type"
                            ,title, @"componentName"
                            ,nil, @"icon", nil];
    return [Utility dictonaryToJson:result];
}

#pragma mark - delegate
- (void)onItemClick:(TapSDKSuiteComponent *)component {
    UnitySendMessage(GAME_OBJECT, [@"HandleFloatingWindowCallbackDataMsg" UTF8String], [[self generateResultMessage:(int)component.type title:component.title icon:nil] UTF8String]);
}

@end

extern "C"
{
    void configComponents(const char *componentListJsonObject) {
        NSString *componentListJsonObjectParam = [NSString stringWithUTF8String:componentListJsonObject];
        NSLog(@"%@", [NSString stringWithFormat:@"configComponents with params: %@", componentListJsonObjectParam]);
        NSData *jsonData = [componentListJsonObjectParam dataUsingEncoding:NSUTF8StringEncoding];
        NSError* error;
        NSDictionary *jsonObject = [NSJSONSerialization JSONObjectWithData:jsonData options: NSJSONReadingMutableContainers error:&error];
        NSArray *array = [jsonObject valueForKey:@"list"];
        NSMutableArray<TapSDKSuiteComponent *> *resultArray = [[NSMutableArray<TapSDKSuiteComponent *> alloc] init];
        for(int i = 0 ; i < array.count; i ++) {
            NSDictionary *componentParamsDic = array[i];
            NSString *str = [componentParamsDic valueForKey:@"type"];
            NSString *title = [componentParamsDic valueForKey:@"componentName"];
            NSString *imageName = [componentParamsDic valueForKey:@"imageName"];

            int type = (int)[str integerValue];
            TapSDKSuiteComponent* component = nil;
            if (type >=0 && type <= 4) {
                component = [[TapSDKSuiteComponent alloc] initWithType:(TapSDKSuiteComponentType)type];
            } else {
                component = [[TapSDKSuiteComponent alloc] initWithType:(TapSDKSuiteComponentType)type title:title icon:[TapSDKSuiteUtils getImageFromBundle:imageName]];
            }
            [resultArray addObject:component];
        }
        [[NativeTapSDKSuiteKitPlugin sharedInstance] setComponentArray:[resultArray mutableCopy]];
    }

    void enable() {
        [[TapSDKSuite shareInstance] setComponentArray:[[NativeTapSDKSuiteKitPlugin sharedInstance] componentArray]];
        [[TapSDKSuite shareInstance] setDelegate:[NativeTapSDKSuiteKitPlugin sharedInstance]];
        [TapSDKSuite enable];
    }

    void disable() {
        [TapSDKSuite disable];
    }
}

