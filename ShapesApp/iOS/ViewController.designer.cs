// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ShapesApp.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton helloButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView logoImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton shapeButton { get; set; }

        [Action ("HelloButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void HelloButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("ShapeButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ShapeButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (helloButton != null) {
                helloButton.Dispose ();
                helloButton = null;
            }

            if (logoImageView != null) {
                logoImageView.Dispose ();
                logoImageView = null;
            }

            if (shapeButton != null) {
                shapeButton.Dispose ();
                shapeButton = null;
            }
        }
    }
}