using System;using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Abstract class used to implement converters that implements the ConvertBack logic.
	/// </summary>
	/// <typeparam name="TFrom">Type of the input value.</typeparam>
	/// <typeparam name="TTo">Type of the output value.</typeparam>
	public abstract class BaseConverter<TFrom, TTo> : BaseConverterOneWay<TFrom, TTo> where TFrom : notnull where TTo : notnull
	{
		/// <summary>
		/// Converts the incoming value from <see cref="TTo"/>[] and returns the object of a type <see cref="TFrom"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>An object of the type <see cref="TFrom"/></returns>
		public sealed override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value), $"value needs to be of type {typeof(TFrom)}, but is null. If type {typeof(TFrom)} should be nullable, inherit from {nameof(BaseNullableConverter<TFrom, TTo>)} instead");

			if (value is not TTo valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TTo)}", nameof(value));

			if (targetType != typeof(TFrom) && !(typeof(TFrom) != typeof(string)))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TFrom)}", nameof(targetType));

			return ConvertBackTo(valueFrom);
		}

		/// <summary>
		/// Method that will be called by <see cref="ConvertBack(object, Type, object, CultureInfo)"/>.
		/// </summary>
		/// <param name="value">Value to be converted from <see cref="TTo"/> to <see cref="TFrom"/>.</param>
		/// <returns>An object of type <see cref="TFrom"/>.</returns>
		public abstract TFrom ConvertBackTo(TTo value);
	}

	/// <summary>
	/// Abstract class used to implement converters that supports null and implements the ConvertBack logic.
	/// </summary>
	/// <typeparam name="TFrom">Type of the input value.</typeparam>
	/// <typeparam name="TTo">Type of the output value.</typeparam>
	public abstract class BaseNullableConverter<TFrom, TTo> : BaseNullableConverterOneWay<TFrom, TTo>
	{
		/// <summary>
		/// Converts the incoming value from <see cref="TTo"/>[] and returns the object of a type <see cref="TFrom"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>An object of the type <see cref="TFrom"/></returns>
		public sealed override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is not TTo valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TTo)}", nameof(value));

			if (targetType != typeof(TFrom) && !(typeof(TFrom) != typeof(string)))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TFrom)}", nameof(targetType));

			return ConvertBackTo(valueFrom);
		}

		/// <summary>
		/// Method that will be called by <see cref="ConvertBack(object, Type, object, CultureInfo)"/>.
		/// </summary>
		/// <param name="value">Value to be converted from <see cref="TTo"/> to <see cref="TFrom"/>.</param>
		/// <returns>An object of type <see cref="TFrom"/>.</returns>
		public abstract TFrom? ConvertBackTo(TTo? value);
	}
}