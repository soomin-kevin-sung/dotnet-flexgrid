using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using KevinComponent.Assist;

namespace KevinComponent
{
	public sealed class TextBand : Band
	{
		static TextBand()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBand), new FrameworkPropertyMetadata(typeof(TextBand)));
		}

		public TextBand() { }

		internal TextBand(VirtualBand virtualBand) : base(virtualBand) { }

		internal TextBand(VirtualTextBand virtualBand) : base(virtualBand)
		{
			TextBinding = virtualBand.TextBinding;
			IsNumeric = virtualBand.IsNumeric;
			AllowNegative = virtualBand.AllowNegative;
		}

		#region Dependency Properties

		public static readonly DependencyProperty IsNumericProperty =
			DependencyProperty.Register(
				"IsNumeric",
				typeof(bool),
				typeof(TextBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty AllowNegativeProperty =
			DependencyProperty.Register(
				"AllowNegative",
				typeof(bool),
				typeof(TextBand),
				new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty TextAlignmentProperty =
			DependencyProperty.Register(
				"TextAlignment",
				typeof(TextAlignment),
				typeof(TextBand),
				new FrameworkPropertyMetadata(TextAlignment.Left));

		public static readonly DependencyProperty TextVerticalAlignmentProperty =
			DependencyProperty.Register(
				"TextVerticalAlignment",
				typeof(VerticalAlignment),
				typeof(TextBand),
				new FrameworkPropertyMetadata(VerticalAlignment.Center));

		public static readonly DependencyProperty OverrideTextAlignmentProperty =
			DependencyProperty.Register(
				"OverrideTextAlignment",
				typeof(bool),
				typeof(TextBand),
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
					OwnerFlexGrid.RefreshCells(this);
				}
			}
		}

		public BindingBase EditingTextBinding
		{
			get => _editingTextBinding;
			set
			{
				if (_editingTextBinding != value)
					_editingTextBinding = value;
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

		private BindingBase GetTextBinding(bool isEditing)
		{
			if (isEditing && EditingTextBinding != null)
				return EditingTextBinding;

			return TextBinding;
		}

		private void SetTextBlockText(TextBlock textBlock)
		{
			var textBinding = GetTextBinding(false);

			// Set TextBinding.
			if (textBinding != null)
				BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, textBinding);
			else
				BindingOperations.ClearBinding(textBlock, TextBlock.TextProperty);
		}

		private void SetTextBlockTextAlignment(TextBlock textBlock)
		{
			// Set TextAlignment.
			if (IsNumeric && !OverrideTextAlignment)
				textBlock.TextAlignment = TextAlignment.Right;
			else
				textBlock.TextAlignment = TextAlignment;
		}

		private void SetTextBlockTextVerticalAlignment(TextBlock textBlock)
		{
			// Set VerticalAlignment;
			textBlock.VerticalAlignment = TextVerticalAlignment;
		}

		private void SetTextBoxText(TextBox textBox)
		{
			var textBinding = GetTextBinding(true);

			// Set TextBinding.
			if (textBinding != null)
				BindingOperations.SetBinding(textBox, TextBox.TextProperty, textBinding);
			else
				BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
		}

		private void SetTextBoxIsNumeric(TextBox textBox)
		{
			// Set IsNumeric.
			TextBoxAssist.SetIsNumericTextBox(textBox, IsNumeric);
		}

		private void SetTextBoxAllowNegative(TextBox textBox)
		{
			// Set AllowNegative.
			TextBoxAssist.SetAllowNegative(textBox, AllowNegative);
		}

		private void SetTextBoxTextAlignment(TextBox textBox)
		{
			// Set Text Alignment.
			if (!IsNumeric || (IsNumeric && OverrideTextAlignment))
				textBox.TextAlignment = TextAlignment;
		}

		private void SetTextBoxTextVerticalAlignment(TextBox textBox)
		{
			textBox.VerticalContentAlignment = TextVerticalAlignment;
		}

		#endregion

		#region Protected Override Methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == IsNumericProperty
				|| e.Property == AllowNegativeProperty
				|| e.Property == TextAlignmentProperty
				|| e.Property == TextVerticalAlignmentProperty
				|| e.Property == OverrideTextAlignmentProperty)
				OwnerFlexGrid.RefreshCells(this);
		}

		protected override void OnCellTemplateLoaded(ContentPresenter contentPresenter)
		{
			base.OnCellTemplateLoaded(contentPresenter);

			var textBlock = Utils.FindVisualChild<TextBlock>(contentPresenter);
			if (textBlock != null)
			{
				SetTextBlockText(textBlock);
				SetTextBlockTextAlignment(textBlock);
				SetTextBlockTextVerticalAlignment(textBlock);

				textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
				textBlock.Width = double.NaN;
			}
		}

		protected override void OnCellEditingTemplateLoaded(ContentPresenter contentPresenter)
		{
			base.OnCellEditingTemplateLoaded(contentPresenter);

			var textBox = Utils.FindVisualChild<TextBox>(contentPresenter);
			if (textBox != null)
			{
				SetTextBoxText(textBox);
				SetTextBoxIsNumeric(textBox);
				SetTextBoxAllowNegative(textBox);
				SetTextBoxTextAlignment(textBox);
				SetTextBoxTextVerticalAlignment(textBox);

				// Set Vertical Alignment.
				textBox.VerticalAlignment = VerticalAlignment.Stretch;
				textBox.HorizontalAlignment = HorizontalAlignment.Stretch;

				// Set Width & Height.
				textBox.Width = double.NaN;
				textBox.Height = double.NaN;
			}
		}

		protected override void PrepareEdit(ContentPresenter contentPresenter)
		{
			base.PrepareEdit(contentPresenter);

			var textBox = Utils.FindVisualChild<TextBox>(contentPresenter);
			if (textBox != null)
			{
				textBox.Focus();
				textBox.SelectAll();
			}
		}

		#endregion
	}
}