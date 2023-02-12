using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KevinComponent.Assist
{
	public static class TextBoxAssist
	{
		#region Dependency Attached Properties

		#region CaretIndexAlwaysEndOfText

		public static readonly DependencyProperty CaretIndexAlwaysEndOfTextProperty =
			DependencyProperty.RegisterAttached("CaretIndexAlwaysEndOfText",
												typeof(bool),
												typeof(TextBoxAssist),
												new FrameworkPropertyMetadata(OnSetCaretIndexAlwaysEndOfText));

		public static bool GetCaretIndexAlwaysEndOfText(DependencyObject element) => (bool)element.GetValue(CaretIndexAlwaysEndOfTextProperty);
		public static void SetCaretIndexAlwaysEndOfText(DependencyObject element, bool value) => element.SetValue(CaretIndexAlwaysEndOfTextProperty, value);

		private static void OnSetCaretIndexAlwaysEndOfText(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is TextBox textBox)
				|| !(e.NewValue is bool value))
				return;

			if (value)
			{
				textBox.TextChanged += OnTextBoxTextChanged;
				SetCaretIndexEndOfText(textBox);
			}
			else
			{
				textBox.TextChanged -= OnTextBoxTextChanged;
			}
		}

		private static void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!(sender is TextBox textBox))
				return;

			SetCaretIndexEndOfText(textBox);
		}

		private static void SetCaretIndexEndOfText(TextBox textBox)
		{
			textBox.Focus();
			textBox.CaretIndex = textBox.Text.Length;
		}

		#endregion

		#region IsNumericTextBox

		public static readonly DependencyProperty IsNumericTextBoxProperty =
			DependencyProperty.RegisterAttached(
				"IsNumericTextBox",
				typeof(bool),
				typeof(TextBoxAssist),
				new FrameworkPropertyMetadata(OnSetIsNumericTextBox));

		public static bool GetIsNumericTextBox(DependencyObject element) => (bool)element.GetValue(IsNumericTextBoxProperty);
		public static void SetIsNumericTextBox(DependencyObject element, bool value) => element.SetValue(IsNumericTextBoxProperty, value);

		private static Regex _numericTextRegexAllowNegative = new Regex("^[0-9.-]+");
		private static Regex _numericTextRegexDisallowNegative = new Regex("^[0-9.]+");
		private static Dictionary<TextBox, HorizontalAlignment> _originAlignments = new Dictionary<TextBox, HorizontalAlignment>();

		private static void OnSetIsNumericTextBox(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is TextBox textBox)
				|| !(e.NewValue is bool value))
				return;

			if (value)
			{
				if (!_originAlignments.ContainsKey(textBox))
					_originAlignments.Add(textBox, HorizontalAlignment.Left);

				textBox.HorizontalContentAlignment = HorizontalAlignment.Right;

				textBox.PreviewTextInput += OnTextBoxPreviewInput;
				DataObject.AddPastingHandler(textBox, OnTextBoxDataObjectPasting);
			}
			else
			{
				if (_originAlignments.ContainsKey(textBox))
					textBox.HorizontalAlignment = _originAlignments[textBox];

				textBox.PreviewTextInput -= OnTextBoxPreviewInput;
				DataObject.RemovePastingHandler(textBox, OnTextBoxDataObjectPasting);
			}
		}

		private static bool IsNumericText(string text, bool allowNegative)
		{
			if (allowNegative)
				return _numericTextRegexAllowNegative.IsMatch(text);
			else
				return _numericTextRegexDisallowNegative.IsMatch(text);
		}

		#endregion

		#region AllowNegative

		public static readonly DependencyProperty AllowNegativeProperty =
			DependencyProperty.RegisterAttached("AllowNegative",
												typeof(bool),
												typeof(TextBoxAssist),
												new FrameworkPropertyMetadata(true));
		public static bool GetAllowNegative(DependencyObject element) => (bool)element.GetValue(AllowNegativeProperty);
		public static void SetAllowNegative(DependencyObject element, bool value) => element.SetValue(AllowNegativeProperty, value);

		private static void OnTextBoxPreviewInput(object sender, TextCompositionEventArgs e)
		{
			bool allowNegative = true;
			if (sender is DependencyObject d)
				allowNegative = GetAllowNegative(d);

			e.Handled = !IsNumericText(e.Text, allowNegative);
		}

		private static void OnTextBoxDataObjectPasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				bool allowNegative = true;
				if (sender is DependencyObject d)
					allowNegative = GetAllowNegative(d);

				string text = (string)e.DataObject.GetData(typeof(string));

				if (!IsNumericText(text, allowNegative))
					e.CancelCommand();
			}
			else
			{
				e.CancelCommand();
			}
		}

		#endregion

		#endregion
	}
}
