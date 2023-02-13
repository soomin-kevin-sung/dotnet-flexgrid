using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class VirtualComboBoxBand : VirtualBand
	{
		#region Dependency Properties

		public static readonly DependencyProperty ComboBoxItemsSourceProperty =
			DependencyProperty.Register(
				"ComboBoxItemsSource",
				typeof(IEnumerable),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register(
				"IsEditable",
				typeof(bool),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsTextSearchEnabledProperty =
			DependencyProperty.Register(
				"IsTextSearchEnabled",
				typeof(bool),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsTextSearchCaseSensitiveProperty =
			DependencyProperty.Register(
				"IsTextSearchCaseSensitive",
				typeof(bool),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty MaxDropDownHeightProperty =
			DependencyProperty.Register(
				"MaxDropDownHeight",
				typeof(double),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(double.NaN));

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register(
				"ItemTemplate",
				typeof(DataTemplate),
				typeof(VirtualComboBoxBand),
				new FrameworkPropertyMetadata(null));

		#endregion

		#region Private Variables

		BindingBase? _selectedItemBinding;

		#endregion

		#region Public Properties

		public BindingBase? SelectedItemBinding
		{
			get => _selectedItemBinding;
			set
			{
				if (_selectedItemBinding != value)
				{
					_selectedItemBinding = value;
					SetSelectedItemBinding(value);
				}
			}
		}

		public IEnumerable ComboBoxItemsSource
		{
			get => (IEnumerable)GetValue(ComboBoxItemsSourceProperty);
			set => SetValue(ComboBoxItemsSourceProperty, value);
		}

		public bool IsEditable
		{
			get => (bool)GetValue(IsEditableProperty);
			set => SetValue(IsEditableProperty, value);
		}

		public bool IsTextSearchEnabled
		{
			get => (bool)GetValue(IsTextSearchEnabledProperty);
			set => SetValue(IsTextSearchEnabledProperty, value);
		}

		public bool IsTextSearchCaseSensitive
		{
			get => (bool)GetValue(IsTextSearchCaseSensitiveProperty);
			set => SetValue(IsTextSearchCaseSensitiveProperty, value);
		}

		public double MaxDropDownHeight
		{
			get => (double)GetValue(MaxDropDownHeightProperty);
			set => SetValue(MaxDropDownHeightProperty, value);
		}

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		#endregion

		#region Private Methods
		
		private void SetSelectedItemBinding(BindingBase? newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is ComboBoxBand comboBoxBand)
					comboBoxBand.SelectedItemBinding = newValue;
			}
		}

		private void SetItemsSource(IEnumerable newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is ComboBoxBand comboBoxBand)
					comboBoxBand.ItemsSource = newValue;
			}
		}

		private void SetIsEditable(bool newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is ComboBoxBand comboBoxBand)
					comboBoxBand.IsEditable = newValue;
			}
		}

		private void SetIsTextSearchEnabled(bool newValue)
		{
			foreach (ComboBoxBand band in VirtualizedBands)
				band.IsTextSearchEnabled = newValue;
		}

		private void SetIsTextSearchCaseSensitive(bool newValue)
		{
			foreach (ComboBoxBand band in VirtualizedBands)
				band.IsTextSearchCaseSensitive = newValue;
		}

		private void SetMaxDropDownHeight(double newValue)
		{
			foreach (ComboBoxBand band in VirtualizedBands)
				band.MaxDropDownHeight = newValue;
		}

		private void SetItemTemplate(DataTemplate newValue)
		{
			foreach (ComboBoxBand band in VirtualizedBands)
				band.ItemTemplate = newValue;
		}

		#endregion

		#region Protected Override Methods

		protected override Band CreateVirtualizedBand()
		{
			return new ComboBoxBand(this);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == ComboBoxItemsSourceProperty)
				SetItemsSource((IEnumerable)e.NewValue);
			else if (e.Property == IsEditableProperty)
				SetIsEditable((bool)e.NewValue);
			else if (e.Property == IsTextSearchEnabledProperty)
				SetIsTextSearchEnabled((bool)e.NewValue);
			else if (e.Property == IsTextSearchCaseSensitiveProperty)
				SetIsTextSearchCaseSensitive((bool)e.NewValue);
			else if (e.Property == MaxDropDownHeightProperty)
				SetMaxDropDownHeight((double)e.NewValue);
			else if (e.Property == ItemTemplateProperty)
				SetItemTemplate((DataTemplate)e.NewValue);
		}

		#endregion
	}
}
