using System;
using UIKit;
using CoreGraphics;

namespace EightBot.ViewMagnifier
{
	/// <summary>
	/// Magnifier.
	/// </summary>
	/// <remarks>
	/// Based off of implementation found here: https://github.com/acoomans/iOS-MagnifyingGlass/blob/master/MagnifyingGlass/ACMagnifyingGlass.m
	/// </remarks>
	public class Magnifier : UIView
	{

		const float DefaultRadius = 40f;
		const float DefaultOffset = -40f;
		const float DefaultScale = 1.5f;

		public CGPoint TouchPointOffset {
			get;
			set;
		}

		public nfloat Scale {
			get;
			set;
		}

		public UIView ViewToMagnify {
			get;
			set;
		}

		public bool ScaleAtTouchPoint {
			get;
			set;
		}
			
		float _radius;
		public float Radius {
			get {
				return _radius;
			}
			set {
				_radius = value;

				Frame = new CGRect (0f, 0f, value * 2f, value * 2f);
			}
		}

		public override CGRect Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;

				this.Layer.CornerRadius = value.Size.Width / 2f;
			}
		}
			
		bool _followTouch = true;
		public bool FollowTouch {
			get {
				return _followTouch;
			}
			set {
				_followTouch = value;
			}
		}

		CGPoint _touchPoint;
		public CGPoint TouchPoint {
			get {
				return _touchPoint;
			}
			set {
				_touchPoint = value;

				if(FollowTouch)
					Center = new CGPoint(value.X + TouchPointOffset.X, value.Y + TouchPointOffset.Y);
			}
		}

		public Magnifier () : this(new CGRect(0f, 0f, DefaultRadius * 2f, DefaultRadius * 2f))
		{
			
		}

		public Magnifier (CGRect frame) : base(frame){

			this.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.Layer.BorderWidth = 3f;
			this.Layer.MasksToBounds = true;

			this.TouchPointOffset = new CGPoint (0, DefaultOffset);
			this.Scale = DefaultScale;
			this.ViewToMagnify = null;
			this.ScaleAtTouchPoint = true;
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			using (var context = UIGraphics.GetCurrentContext()){
				context.TranslateCTM (this.Frame.Size.Width / 2f, this.Frame.Size.Height / 2f);
				context.ScaleCTM (Scale, Scale);
				context.TranslateCTM (-TouchPoint.X, -TouchPoint.Y + (ScaleAtTouchPoint ? 0 : this.Bounds.Size.Height / 2f));

				if(ViewToMagnify != null)
					this.ViewToMagnify.Layer.RenderInContext (context);
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				ViewToMagnify = null;
			}

			base.Dispose (disposing);
		}
	}
}

