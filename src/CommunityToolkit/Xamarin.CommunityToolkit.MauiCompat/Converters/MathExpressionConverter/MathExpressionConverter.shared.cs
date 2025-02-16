﻿using System;using Microsoft.Extensions.Logging;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	public class MathExpressionConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// The expression to calculate.
		/// </summary>
		public string? Expression { get; set; }

		/// <summary>
		/// Calculate the incoming expression string with one variable.
		/// </summary>
		/// <param name="value">The variable X for an expression</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">The expression to calculate.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>A <see cref="double"/> The result of calculating an expression.</returns>
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
		{
			if ((parameter ?? Expression) is not string expression)
				throw new ArgumentException("The parameter should be of type String.");

			if (value == null || !double.TryParse(value.ToString(), out var xValue))
				return null;

			var math = new MathExpression(expression, new[] { xValue });

			var result = math.Calculate();
			return result;
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
			=> throw new NotImplementedException();
	}
}