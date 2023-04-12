using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class VirtualTextBand : VirtualBand
	{
		#region Dependency Properties

		public static readonly DependencyProperty IsNumericProperty =
			DependencyProperty.Register(
				"IsNumeric",
				typeof(bool),
				typeof(VirtualTextBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty AllowNegativeProperty =
			DependencyProperty.Register(
				"AllowNegative",
				typeof(bool),
				typeof(VirtualTextBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty TextAlignmentProperty =
			DependencyProperty.Register(
				"TextAlignment",
				typeof(TextAlignment),
				typeof(VirtualTextBand),
				new FrameworkPropertyMetadata(TextAlignment.Left));

		public static readonly DependencyProperty TextVerticalAlignmentProperty =
			DependencyProperty.Register(
				"TextVerticalAlignment",
				typeof(VerticalAlignment),
				typeof(VirtualTextBand),
				new FrameworkPropertyMetadata(VerticalAlignment.Center));

		public static readonly DependencyProperty OverrideTextAlignmentProperty =
			DependencyProperty.Register(
				"OverrideTextAlignment",
				typeof(bool),
				typeof(VirtualTextBand),
				new FrameworkPropertyMetadata(false));

		#endregion

		#region Private Variables

		BindingBase _textBinding;
		BindingBase _editingTextBinding;

		#endregion

		#region Public Properties

		public BindingBase TextBinding
		{
			get => _textBinding;
			set
			{
				if (_textBinding != value)
				{
					_textBinding = value;
					SetTextBinding(value);
				}
			}
		}

		public BindingBase EditingTextBinding
		{
			get => _editingTextBinding;
			set
			{
				if (_editingTextBinding != value)
				{
					_editingTextBinding = value;
					SetEditingTextBinding(value);
				}
			}
		}

		public bool IsNumeric
		{
			get => (bool)GetValue(IsNumericProperty);
			set => SetValue(IsNumericProperty, value);
		}

		public bool AllowNegative
		{
			get => (bool)GetValue(AllowNegativeProperty);
			set => SetValue(AllowNegativeProperty, value);
		}

		public TextAlignment TextAlignment
		{
			get => (TextAlignment)GetValue(TextAlignmentProperty);
			set => SetValue(TextAlignmentProperty, value);
		}

		public VerticalAlignment TextVerticalAlignment
		{
			get => (VerticalAlignment)GetValue(TextVerticalAlignmentProperty);
			set => SetValue(TextVerticalAlignmentProperty, value);
		}

		public bool OverrideTextAlignment
		{
			get => (bool)GetValue(OverrideTextAlignmentProperty);
			set => SetValue(OverrideTextAlignmentProperty, value);
		}

		#endregion

		#region Private Methods

		private void SetTextBinding(BindingBase newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.TextBinding = newValue;
			}
		}

		private void SetEditingTextBinding(BindingBase newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.EditingTextBinding = newValue;
			}
		}

		private void SetIsNumeric(bool newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.IsNumeric = newValue;
			}
		}

		private void SetAllowNegative(bool newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.AllowNegative = newValue;
			}
		}

		private void SetTextAlignment(TextAlignment newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.TextAlignment = newValue;
			}
		}

		private void SetTextVerticalAlignment(VerticalAlignment newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.TextVerticalAlignment = newValue;
			}
		}

		private void SetOverrideTextAlignment(bool newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TextBand textBand)
					textBand.OverrideTextAlignment = newValue;
			}
		}

		#endregion

		#region Protected Override Methods

		protected override Band CreateVirtualizedBand()
		{
			return new TextBand(this);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == IsNumericProperty)
				SetIsNumeric((bool)e.NewValue);
			else if (e.Property == AllowNegativeProperty)
				SetAllowNegative((bool)e.NewValue);
			else if (e.Property == TextAlignmentProperty)
				SetTextAlignment((TextAlignment)e.NewValue);
			else if (e.Property == TextVerticalAlignmentProperty)
				SetTextVerticalAlignment((VerticalAlignment)e.NewValue);
			else if (e.Property == OverrideTextAlignmentProperty)
				SetOverrideTextAlignment((bool)e.NewValue);
		}

		#endregion
	}
}
