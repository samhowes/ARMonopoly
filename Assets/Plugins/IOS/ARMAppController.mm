#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "UI/UnityView.h"

@interface ARMAppController : UnityAppController
{
}
- (void)createViewHierarchyImpl;
@end
@implementation ARMAppController
- (void)createViewHierarchyImpl;
{
	_rootController = [[ARMViewController alloc] init];
	_rootView = _unityView;
}
@end

IMPL_APP_CONTROLLER_SUBCLASS(ARMAppController)

