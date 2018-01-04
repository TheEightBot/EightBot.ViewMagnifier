using System;

using UIKit;

namespace EightBot.ViewMagnifier.Sample
{
	public partial class ViewController : UIViewController
	{
		UIImageView _viewToMagnify;

		MagnifierView _magnifierView;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			_magnifierView = new MagnifierView ();
			_magnifierView.Frame = View.Frame;
			_magnifierView.Magnifier.ScaleAtTouchPoint = true;
			_magnifierView.Magnifier.Radius = 80;
			_magnifierView.Magnifier.Scale = 2.5f;
            _magnifierView.ShowOnTouch = true;

			_magnifierView.Magnifier.Center = new CoreGraphics.CGPoint (100, 100);


			this.Add (_magnifierView);

			_viewToMagnify = new UIImageView (UIImage.FromFile ("Culumnodingus.png"));
			_viewToMagnify.Frame = View.Frame;
			_viewToMagnify.ContentMode = UIViewContentMode.ScaleAspectFill;
			_magnifierView.Add (_viewToMagnify);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

