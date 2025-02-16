﻿using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// The <see cref="RequiredStringValidationBehavior"/> is a behavior that allows the user to determine if text input is equal to specific text. For example, an <see cref="Entry"/> control can be styled differently depending on whether a valid or an invalid text input is provided. Additional properties handling validation are inherited from <see cref="ValidationBehavior"/>.
	/// </summary>
	public class RequiredStringValidationBehavior : ValidationBehavior
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="RequiredString"/> property.
		/// </summary>
		public static readonly BindableProperty RequiredStringProperty
			= BindableProperty.Create(nameof(RequiredString), typeof(string), typeof(RequiredStringValidationBehavior));

		/// <summary>
		/// The string that will be compared to the value provided by the user. This is a bindable property.
		/// </summary>
		public string? RequiredString
		{
			get => (string?)GetValue(RequiredStringProperty);
			set => SetValue(RequiredStringProperty, value);
		}

		protected override ValueTask<bool> ValidateAsync(object? value, CancellationToken token)
			=> new ValueTask<bool>(value?.ToString() == RequiredString);
	}
}