﻿using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void PlatformShowPopup(BasePopup popup) =>
			Platform.CreateRenderer(popup);

		static Task<T?> PlatformShowPopupAsync<T>(Popup<T> popup)
		{
			PlatformShowPopup(popup);
			return popup.Result;
		}
	}
}