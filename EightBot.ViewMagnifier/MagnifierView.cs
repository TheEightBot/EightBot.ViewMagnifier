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
		}

		public override async void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			var touch = touches.AnyObject as UITouch;

			if (Magnifier == null)
				Magnifier = new Magnifier ();

			if (Magnifier.ViewToMagnify == null)
				Magnifier.ViewToMagnify = this;

			Magnifier.Alpha = 0f;
			Magnifier.TouchPoint = touch.LocationInView (this);
			this.Superview.AddSubview (Magnifier);
			Magnifier.SetNeedsDisplay ();

			UIView.AnimateAsync (MagnifyingGlassShowDelay, () => Magnifier.Alpha = 1f);

			if (Magnifier.Frame.Contains (touch.LocationInView (this)))
				System.Diagnostics.Debug.WriteLine ("touching");
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

			var touch = touches.AnyObject as UITouch;

			Magnifier.TouchPoint = touch.LocationInView (this);
			Magnifier.SetNeedsDisplay ();
		}

		public override async void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);

			await UIView.AnimateAsync (MagnifyingGlassShowDelay, () => {
				Magnifier.Alpha = 0f;
			});
			Magnifier.RemoveFromSuperview ();
		}

		public override async void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

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

