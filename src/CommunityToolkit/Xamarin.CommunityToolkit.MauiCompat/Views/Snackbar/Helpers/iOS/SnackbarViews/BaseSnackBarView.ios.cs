﻿using System;using Microsoft.Extensions.Logging;
using System.Linq;
using CoreGraphics;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar
{
	abstract class BaseSnackBarView : UIView
	{
		protected BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public UIView? AnchorView { get; set; }

		public UIView ParentView => UIApplication.SharedApplication.Windows.First(x => x.IsKeyWindow);

		protected NativeSnackBar SnackBar { get; }

		protected NativeRoundedStackView? StackView { get; set; }

		public void Dismiss() => RemoveFromSuperview();

		public void Setup(CGRect cornerRadius)
		{
			Initialize(cornerRadius);
			ConstraintInParent();
		}

		void ConstraintInParent()
		{
			_ = ParentView ?? throw new InvalidOperationException($"{nameof(BaseSnackBarView)}.{nameof(Initialize)} not called");
			_ = StackView ?? throw new InvalidOperationException($"{nameof(BaseSnackBarView)}.{nameof(Initialize)} not called");

			if (AnchorView is null)
			{
				this.SafeBottomAnchor().ConstraintEqualTo(ParentView.SafeBottomAnchor(), -SnackBar.Layout.MarginBottom).Active = true;
				this.SafeTopAnchor().ConstraintGreaterThanOrEqualTo(ParentView.SafeTopAnchor(), SnackBar.Layout.MarginTop).Active = true;
			}
			else
			{
				this.SafeBottomAnchor().ConstraintEqualTo(AnchorView.SafeBottomAnchor(), -SnackBar.Layout.MarginBottom).Active = true;
			}

			this.SafeLeadingAnchor().ConstraintGreaterThanOrEqualTo(ParentView.SafeLeadingAnchor(), SnackBar.Layout.MarginLeft).Active = true;
			this.SafeTrailingAnchor().ConstraintLessThanOrEqualTo(ParentView.SafeTrailingAnchor(), -SnackBar.Layout.MarginRight).Active = true;
			this.SafeCenterXAnchor().ConstraintEqualTo(ParentView.SafeCenterXAnchor()).Active = true;

			StackView.SafeLeadingAnchor().ConstraintEqualTo(this.SafeLeadingAnchor(), SnackBar.Layout.PaddingLeft).Active = true;
			StackView.SafeTrailingAnchor().ConstraintEqualTo(this.SafeTrailingAnchor(), -SnackBar.Layout.PaddingRight).Active = true;
			StackView.SafeBottomAnchor().ConstraintEqualTo(this.SafeBottomAnchor(), -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.SafeTopAnchor().ConstraintEqualTo(this.SafeTopAnchor(), SnackBar.Layout.PaddingTop).Active = true;
		}

		protected virtual void Initialize(CGRect cornerRadius)
		{
			StackView = new NativeRoundedStackView(cornerRadius.X, cornerRadius.Y, cornerRadius.Width, cornerRadius.Height)
			{
				Alignment = UIStackViewAlignment.Fill,
				Distribution = UIStackViewDistribution.EqualCentering
			};
			AddSubview(StackView);

			StackView.Axis = UILayoutConstraintAxis.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = SnackBar.Layout.Spacing;

			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor)
			{
				StackView.BackgroundColor = SnackBar.Appearance.Background;
			}

			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}