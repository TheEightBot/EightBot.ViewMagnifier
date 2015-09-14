using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace EightBot.ViewMagnifier
{
	public class MagnifierView : UIView
	{
		const double DefaultShowDelay = .2d;

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

		public override async void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			var touch = touches.AnyObject as UITouch;

//			if (this.Subviews != null) {
//				foreach (var child in this.Subviews) {
//					child.TouchesBegan (touches, evt);
//				}
//			}

			if (Magnifier == null)
				Magnifier = new Magnifier ();

			if (Magnifier.ViewToMagnify == null)
				Magnifier.ViewToMagnify = this;

			Magnifier.Alpha = 0f;
			var touchPoint = touch.LocationInView (this);

			if (touchPoint.X - Magnifier.Radius < 0)
				touchPoint.X = Magnifier.Radius;
			else if (touchPoint.X + Magnifier.Radius > this.Frame.Width)
				touchPoint.X = this.Frame.Width - Magnifier.Radius;

			if (touchPoint.Y - Magnifier.Radius < 0)
				touchPoint.Y = Magnifier.Radius;
			else if (touchPoint.Y + Magnifier.Radius > this.Frame.Height)
				touchPoint.Y = this.Frame.Height - Magnifier.Radius;
			

			Magnifier.TouchPoint = touchPoint;
			this.Superview.AddSubview (Magnifier);
			Magnifier.SetNeedsDisplay ();

			UIView.AnimateAsync (MagnifyingGlassShowDelay, () => Magnifier.Alpha = 1f);

			if (Magnifier.Frame.Contains (touch.LocationInView (this)))
				System.Diagnostics.Debug.WriteLine ("touching");
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

//			if (this.Subviews != null) {
//				foreach (var child in this.Subviews) {
//					child.TouchesMoved (touches, evt);
//				}
//			}

			var touch = touches.AnyObject as UITouch;

			var touchPoint = touch.LocationInView (this);

			if (touchPoint.X - Magnifier.Radius < 0)
				touchPoint.X = Magnifier.Radius;
			else if (touchPoint.X + Magnifier.Radius > this.Frame.Width)
				touchPoint.X = this.Frame.Width - Magnifier.Radius;

			if (touchPoint.Y - Magnifier.Radius < 0)
				touchPoint.Y = Magnifier.Radius;
			else if (touchPoint.Y + Magnifier.Radius > this.Frame.Height)
				touchPoint.Y = this.Frame.Height - Magnifier.Radius;


			Magnifier.TouchPoint = touchPoint;
			Magnifier.SetNeedsDisplay ();
		}

		public override async void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);

//			if (this.Subviews != null) {
//				foreach (var child in this.Subviews) {
//					child.TouchesCancelled (touches, evt);
//				}
//			}

			await UIView.AnimateAsync (MagnifyingGlassShowDelay, () => {
				Magnifier.Alpha = 0f;
			});

			Magnifier.RemoveFromSuperview ();
		}

		public override async void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

//			if (this.Subviews != null) {
//				foreach (var child in this.Subviews) {
//					child.TouchesEnded (touches, evt);
//				}
//			}

			await UIView.AnimateAsync (MagnifyingGlassShowDelay, () => {
				Magnifier.Alpha = 0f;
			});

			Magnifier.RemoveFromSuperview ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
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

