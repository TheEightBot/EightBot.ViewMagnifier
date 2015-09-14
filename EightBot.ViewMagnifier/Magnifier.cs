using System;
using UIKit;
using CoreGraphics;
using EightBot.ViewMagnifier.Extensions;

namespace EightBot.ViewMagnifier
{

	public enum MagnifierPosition {
		FollowTouch,
		Static,
		Docked
	}

	public enum DockPosition {
		TopLeft,
		TopRight,
	}


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

		public MagnifierPosition MagnifierPosition {
			get;
			set;
		}

		public DockPosition DockPosition {
			get;
			set;
		}

		public CGPoint TouchPointOffset {
			get;
			set;
		}

		float _dockPadding = 20f;
		public float DockPadding {
			get {
				return _dockPadding;
			}
			set {
				_dockPadding = value;
			}
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

				_radius = (float)Frame.Width / 2f;

				this.Layer.CornerRadius = value.Size.Width / 2f;
			}
		}
			
		CGPoint _touchPoint;
		public CGPoint TouchPoint {
			get {
				return _touchPoint;
			}
			set {
				_touchPoint = value;

				if (MagnifierPosition == MagnifierPosition.FollowTouch)
					Center = new CGPoint (value.X + TouchPointOffset.X, value.Y + TouchPointOffset.Y);
				else if (MagnifierPosition == MagnifierPosition.Docked) {
					if (this.ViewToMagnify != null) {
						if (this.Frame.Contains (_touchPoint)) {
							this.DockPosition = (DockPosition)this.DockPosition.Previous ();
							System.Diagnostics.Debug.WriteLine ("Magnifier Contains Point - View To Magnify: {0} TouchPoint: {1}", this.ViewToMagnify.Frame, _touchPoint);
						}

						CGPoint newCenter = CGPoint.Empty;

						switch (DockPosition) {
						case DockPosition.TopLeft:
							newCenter = new CGPoint (Radius + DockPadding, Radius + DockPadding);
							break;
						case DockPosition.TopRight:
							newCenter = new CGPoint (this.ViewToMagnify.Frame.Width - Radius - DockPadding, Radius + DockPadding);
							break;
//						case DockPosition.BottomRight:
//							newCenter = new CGPoint (
//								this.ViewToMagnify.Frame.Width - Radius - DockPadding, 
//								this.ViewToMagnify.Frame.Height - Radius - DockPadding);
//							break;
//						case DockPosition.BottomLeft:
//							newCenter = new CGPoint (Radius + DockPadding, this.ViewToMagnify.Frame.Height - Radius - DockPadding);
//							break;
						}

						if(newCenter != Center)
							UIView.Animate (.2d, () => this.Center = newCenter);
					}
				}
			}
		}

		public Magnifier () : this(new CGRect(0f, 0f, DefaultRadius * 2f, DefaultRadius * 2f))
		{
			
		}

		public Magnifier (CGRect frame) : base(frame){

			this.ExclusiveTouch = false;

			this.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.Layer.BorderWidth = 3f;
			this.Layer.MasksToBounds = true;

			this.TouchPointOffset = new CGPoint (0, DefaultOffset);
			this.Scale = DefaultScale;
			this.ViewToMagnify = null;
			this.ScaleAtTouchPoint = true;

			this._radius = (float)frame.Width / 2f;
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

