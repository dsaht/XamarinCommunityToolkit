﻿using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Core
{
	public sealed class UriMediaSource : MediaSource
	{
		public static readonly BindableProperty UriProperty =
			BindableProperty.Create(nameof(Uri), typeof(Uri), typeof(UriMediaSource), propertyChanged: OnUriSourceChanged, validateValue: UriValueValidator);

		static bool UriValueValidator(BindableObject bindable, object value) =>
			value == null || ((Uri)value).IsAbsoluteUri;

		static void OnUriSourceChanged(BindableObject bindable, object oldValue, object newValue) =>
			((UriMediaSource)bindable).OnSourceChanged();

		[System.ComponentModel.TypeConverter(typeof(Microsoft.Maui.Controls.UriTypeConverter))]
		public Uri? Uri
		{
			get => (Uri?)GetValue(UriProperty);
			set => SetValue(UriProperty, value);
		}

		public override string ToString() => $"Uri: {Uri}";
	}
}