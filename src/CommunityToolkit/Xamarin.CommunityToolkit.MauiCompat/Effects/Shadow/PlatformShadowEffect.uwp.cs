﻿using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Shapes;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui.Platform;using Application = Microsoft.Maui.Controls.Application;
using Image = Microsoft.UI.Xaml.Controls.Image;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformShadowEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		enum ShadowEffectState
		{
			Initialized,
			PanelCreated,
			Attached
		}

		const float defaultRadius = 10f;

		const float defaultOpacity = 1f;

		ShadowEffectState state;

		SpriteVisual? spriteVisual;

		Microsoft.Maui.Controls.StackLayout? shadowPanel;

		DropShadow? shadow;

		FrameworkElement? View => Control ?? Container;

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			switch (args.PropertyName)
			{
				case ShadowEffect.ColorPropertyName:
				case ShadowEffect.OpacityPropertyName:
				case ShadowEffect.RadiusPropertyName:
				case ShadowEffect.OffsetXPropertyName:
				case ShadowEffect.OffsetYPropertyName:
				case nameof(VisualElement.Width):
				case nameof(VisualElement.Height):
				case nameof(VisualElement.BackgroundColor):
				case nameof(IBorderElement.CornerRadius):
					UpdateShadow();
					break;
			}
		}

		protected override void OnAttached()
		{
			if (Element is not View elementView)
				return;

			switch (state)
			{
				case ShadowEffectState.Initialized:
					shadowPanel = new 	Microsoft.Maui.Controls.StackLayout()
					{
						Children = { new Microsoft.Maui.Controls.Compatibility.Grid() }
					};

					state = ShadowEffectState.PanelCreated;
					MoveElementTo(elementView, shadowPanel);
					break;
				case ShadowEffectState.PanelCreated:
					AppendShadow();
					state = ShadowEffectState.Attached;
					break;
				default:
					break;
			}
		}

		protected override void OnDetached()
		{
			if (state != ShadowEffectState.Attached)
				return;

			if (View != null)
			{
				View.SizeChanged -= ViewSizeChanged;
			}

			shadow?.Dispose();
			shadow = null;
			spriteVisual?.Dispose();
			spriteVisual = null;

			state = ShadowEffectState.PanelCreated;
		}

		void AppendShadow()
		{
			if (View == null)
				return;

			var view = ElementCompositionPreview.GetElementVisual(View);

			if (view == null)
				return;

			var compositor = view.Compositor;
			shadow ??= compositor.CreateDropShadow();
			UpdateShadow();
			spriteVisual = compositor.CreateSpriteVisual();
			spriteVisual.Shadow = shadow;
			spriteVisual.Size = View.ActualSize;

			View.SizeChanged += ViewSizeChanged;

			var renderer = shadowPanel?.Children.First().ToPlatform(Application.Current.MainPage.Handler?.MauiContext);
			spriteVisual.ParentForTransform = ElementCompositionPreview.GetElementVisual(View);
			ElementCompositionPreview.SetElementChildVisual(renderer, spriteVisual);
		}

		void MoveElementTo(View element, Microsoft.Maui.Controls.StackLayout to)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (Element.Parent is Microsoft.Maui.Controls.StackLayout layout)
				{
					var index = layout.Children.IndexOf(element);
					layout.Children.Insert(index, to);

					if (layout is Microsoft.Maui.Controls.Compatibility.Grid)
					{
						var row = Microsoft.Maui.Controls.Compatibility.Grid.GetRow(element);
						var rowSpan = Microsoft.Maui.Controls.Compatibility.Grid.GetRowSpan(element);
						var column = Microsoft.Maui.Controls.Compatibility.Grid.GetColumn(element);
						var columnSpan = Microsoft.Maui.Controls.Compatibility.Grid.GetColumnSpan(element);

						Microsoft.Maui.Controls.Compatibility.Grid.SetRow(to, row);
						Microsoft.Maui.Controls.Compatibility.Grid.SetRowSpan(to, rowSpan);
						Microsoft.Maui.Controls.Compatibility.Grid.SetColumn(to, column);
						Microsoft.Maui.Controls.Compatibility.Grid.SetColumnSpan(to, columnSpan);
					}
				}
				else if (Element.Parent is ScrollView scrollView)
				{
					scrollView.Content = to;
				}
				else if (Element.Parent is ContentView contentView)
				{
					contentView.Content = to;
				}

				to.Children.Add(element);
			});
		}

		void UpdateShadow()
		{
			if (shadow == null)
				return;

			var radius = (float)ShadowEffect.GetRadius(Element);
			var opacity = (float)ShadowEffect.GetOpacity(Element);
			var color = ShadowEffect.GetColor(Element).ToWindowsColor();
			var offsetX = (float)ShadowEffect.GetOffsetX(Element);
			var offsetY = (float)ShadowEffect.GetOffsetY(Element);

			shadow.Color = color;
			shadow.BlurRadius = radius < 0 ? defaultRadius : radius;
			shadow.Opacity = opacity < 0 ? defaultOpacity : opacity;
			shadow.Offset = new Vector3(offsetX, offsetY, 0);

			UpdateShadowMask();
		}

		void UpdateShadowMask()
		{
			if (shadow == null)
				return;

			shadow.Mask = View switch
			{
				TextBlock textBlock => textBlock.GetAlphaMask(),
				Shape shape => shape.GetAlphaMask(),
				Image image => image.GetAlphaMask(),
				_ => shadow.Mask
			};
		}

		void ViewSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (spriteVisual == null || View == null)
				return;

			spriteVisual.Size = View.ActualSize;

			UpdateShadowMask();
		}
	}
}