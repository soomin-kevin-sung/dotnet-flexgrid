using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class VirtualCheckBoxBand : VirtualBand
	{
		public VirtualCheckBoxBand()
		{
			_checkBinding = null;

		}

		#region Dependency Properties

		public static readonly DependencyProperty IsThreeStateProperty =
			DependencyProperty.Register(
				"IsThreeState",
				typeof(bool),
				typeof(VirtualCheckBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(
				"Content",
				typeof(object),
				typeof(VirtualCheckBoxBand),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty CheckBoxHorizontalAlignmentProperty =
			DependencyProperty.Register(
				"CheckBoxHorizontalAlignment",
				typeof(HorizontalAlignment),
				typeof(VirtualCheckBoxBand),
				new FrameworkPropertyMetadata(HorizontalAlignment.Center));

		#endregion

		#region Private Variables

		BindingBase _checkBinding;
		BindingBase _contentBinding;

		#endregion

		#region Public Properties

		public BindingBase CheckBinding
		{
			get => _checkBinding;
			set
			{
				if (_checkBinding != value)
				{
					_checkBinding = value;
					SetCheckBinding(value);
				}
			}
		}

		public bool IsThreeState
		{
			get => (bool)GetValue(IsThreeStateProperty);
			set => SetValue(IsThreeStateProperty, value);
		}

		public object Content
		{
			get => GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public BindingBase ContentBinding
		{
			get => _contentBinding;
			set
			{
				if (_contentBinding != value)
				{
					_contentBinding = value;
					SetContentBinding(value);
				}
			}
		}

		public HorizontalAlignment CheckBoxHorizontalAlignment
		{
			get => (HorizontalAlignment)GetValue(CheckBoxHorizontalAlignmentProperty);
			set => SetValue(CheckBoxHorizontalAlignmentProperty, value);
		}

		#endregion

		#region Private Methods

		private void SetCheckBinding(BindingBase newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is CheckBoxBand checkBoxBand)
					checkBoxBand.CheckBinding = newValue;
			}
		}

		private void SetIsThreeState(bool newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is CheckBoxBand checkBoxBand)
					checkBoxBand.IsThreeState = newValue;
			}
		}

		private void SetContent(object newValue)
		{
			if (ContentBinding != null)
				return;

			foreach (var band in VirtualizedBands)
			{
				if (band is CheckBoxBand checkBoxBand)
					checkBoxBand.Content = newValue;
			}
		}

		private void SetContentBinding(BindingBase newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (newValue != null)
					BindingOperations.SetBinding(band, CheckBoxBand.ContentProperty, newValue);
				else
					BindingOperations.ClearBinding(band, CheckBoxBand.ContentProperty);
			}

			if (newValue == null)
				SetContent(Content);
		}

		private void SetCheckBoxHorizontalAlignment(HorizontalAlignment newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is CheckBoxBand checkBoxBand)
					checkBoxBand.CheckBoxHorizontalAlignment = newValue;
			}
		}

		#endregion

		#region Protected Override Methods

		protected override Band CreateVirtualizedBand()
		{
			return new CheckBoxBand(this);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == IsThreeStateProperty)
				SetIsThreeState((bool)e.NewValue);
			else if (e.Property == ContentProperty)
				SetContent(e.NewValue);
			else if (e.Property == CheckBoxHorizontalAlignmentProperty)
				SetCheckBoxHorizontalAlignment((HorizontalAlignment)e.NewValue);
		}

		#endregion
	}
}
