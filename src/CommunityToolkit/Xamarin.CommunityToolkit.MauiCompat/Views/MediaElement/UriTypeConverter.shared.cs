using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;
using System.ComponentModel;
using System.Globalization;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[System.ComponentModel.TypeConverter(typeof(Uri))]
	public class UriTypeConverter : System.ComponentModel.TypeConverter
	{
		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			if (value is not string valueStr) { throw new InvalidOperationException("Only typeof(string) allowed"); }
			return string.IsNullOrWhiteSpace(valueStr) ? null : new Uri(valueStr, UriKind.RelativeOrAbsolute);
		}
	}
}