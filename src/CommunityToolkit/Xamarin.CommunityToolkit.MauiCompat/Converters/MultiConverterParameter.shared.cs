﻿using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Represents a parameter to be used in the <see cref="MultiConverter"/>.
	/// </summary>
	public class MultiConverterParameter : BindableObject
	{
		/// <summary>
		/// The type of object of this parameter.
		/// </summary>
		public Type? ConverterType { get; set; }

		/// <summary>
		/// The value of this parameter.
		/// </summary>
		public object? Value { get; set; }
	}
}