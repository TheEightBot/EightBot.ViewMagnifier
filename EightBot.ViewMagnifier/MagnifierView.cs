using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;
using System.Linq;

namespace EightBot.ViewMagnifier
{
	public class MagnifierView : UIView
	{
		const double DefaultShowDelay = .2d;

		bool _shouldDisplay;
		public bool ShouldDisplay {
			get {
				return _shouldDisplay;
			}
			set {
				_shouldDisplay = value;

				if (_shouldDisplay)
					UIView.Animate(MagnifyingGlassShowDelay, 0d, 
						UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseIn,
						() => Magnifier.Alpha = 1f,
						null);
				else
					UIView.Animate (MagnifyingGlassShowDelay, 0d, 
						UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseOut,
						() => Magnifier.Alpha = 0f,
						null);
			}
		}

		public double MagnifyingGlassShowDelay {
			get;
			set;
		}

		public Magnifier Magnifier {
			get;
			set;
		}

		NSTimer _touchTimer;

		public MagnifierView ()
		{
			MagnifyingGlassShowDelay = DefaultShowDelay;
			Magnifier = new Magnifier ();
			this.ExclusiveTouch = false;
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			var touch = touches.AnyObject as UITouch;

			if (Magnifier == null)
				Magnifier = new Magnifier ();

			if (Magnifier.ViewToMagnify == null)
				Magnifier.ViewToMagnify = this;

			var touchPoint = touch.LocationInView (this);

			Magnifier.TouchPoint = touchPoint;
			this.Superview.AddSubview (Magnifier);
			Magnifier.SetNeedsDisplay ();

			//if (Magnifier.Frame.Contains (touch.LocationInView (this)))
				System.Diagnostics.Debug.WriteLine ("touching");
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

			var touch = touches.AnyObject as UITouch;

			var touchPoint = touch.LocationInView (this);

			Magnifier.TouchPoint = touchPoint;
			Magnifier.SetNeedsDisplay ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (this.Subviews.Any ()) {
					for (int i = this.Subviews.Count() - 1; i >= 0; i--) {
						this.Subviews [i].RemoveFromSuperview ();
					}
				}

				if (Magnifier != null) {
					Magnifier.RemoveFromSuperview ();
					Magnifier.Dispose ();
					Magnifier = null;
				}
			}

			base.Dispose (disposing);
		}
	}
}

