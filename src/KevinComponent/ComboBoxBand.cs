using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using KevinComponent.Assist;

namespace KevinComponent
{
	public sealed class ComboBoxBand : Band
	{
		static ComboBoxBand()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxBand), new FrameworkPropertyMetadata(typeof(ComboBoxBand)));
		}

		public ComboBoxBand() { }

		internal ComboBoxBand(VirtualComboBoxBand virtualBand) : base(virtualBand)
		{
			SelectedItemBinding = virtualBand.SelectedItemBinding;
			ItemsSource = virtualBand.ComboBoxItemsSource;
			IsEditable = virtualBand.IsEditable;
			IsTextSearchEnabled = virtualBand.IsTextSearchEnabled;
			IsTextSearchCaseSensitive = virtualBand.IsTextSearchCaseSensitive;
			MaxDropDownHeight = virtualBand.MaxDropDownHeight;
			ItemTemplate = virtualBand.ItemTemplate;
		}

		#region Dependency Properties

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register(
				"ItemsSource",
				typeof(IEnumerable),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register(
				"IsEditable",
				typeof(bool),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsTextSearchEnabledProperty =
			DependencyProperty.Register(
				"IsTextSearchEnabled",
				typeof(bool),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsTextSearchCaseSensitiveProperty =
			DependencyProperty.Register(
				"IsTextSearchCaseSensitive",
				typeof(bool),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty MaxDropDownHeightProperty =
			DependencyProperty.Register(
				"MaxDropDownHeight",
				typeof(double),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(double.NaN));

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register(
				"ItemTemplate",
				typeof(DataTemplate),
				typeof(ComboBoxBand),
				new FrameworkPropertyMetadata(null, OnItemTemplateChanged));

		private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = (ComboBoxBand)d;
			band.OnItemTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		#endregion

		#region Private Variables

		BindingBase _selectedItemBinding;

		#endregion

		#region Public Properties

		public BindingBase SelectedItemBinding
		{
			get => _selectedItemBinding;
			set
			{
				if (_selectedItemBinding != value)
				{
					_selectedItemBinding = value;
					OwnerFlexGrid?.RefreshCells(this);
				}
			}
		}

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
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

		private void OnItemTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
		{
			OwnerFlexGrid?.RefreshCells(this);
		}

		private void SetComboBoxSelectedItem(ComboBox comboBox)
		{
			if (SelectedItemBinding != null)
				BindingOperations.SetBinding(comboBox, ComboBox.SelectedItemProperty, SelectedItemBinding);
			else
				BindingOperations.ClearBinding(comboBox, ComboBox.SelectedItemProperty);
		}

		private void SetComboBoxItemsSource(ComboBox comboBox)
		{
			var extBinding = BindingOperations.GetBinding(this, ItemsSourceProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, extBinding);
			}
			else
			{
				var itemsSourceBinding = new Binding("ItemsSource") { Source = this };
				BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, itemsSourceBinding);
			}
		}

		private void SetComboBoxIsEditable(ComboBox comboBox)
		{
			var extBinding = BindingOperations.GetBinding(this, IsEditableProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(comboBox, ComboBox.IsEditableProperty, extBinding);
			}
			else
			{
				var isEditableBinding = new Binding("IsEditable") { Source = this };
				BindingOperations.SetBinding(comboBox, ComboBox.IsEditableProperty, isEditableBinding);
			}
		}

		private void SetComboBoxIsTextSearchEnabled(ComboBox comboBox)
		{
			var extBinding = BindingOperations.GetBinding(this, IsTextSearchEnabledProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(comboBox, ComboBox.IsTextSearchEnabledProperty, extBinding);
			}
			else
			{
				var isTextSearchEnabledBinding = new Binding("IsTextSearchEnabled") { Source = this };
				BindingOperations.SetBinding(comboBox, ComboBox.IsTextSearchEnabledProperty, isTextSearchEnabledBinding);
			}
		}

		private void SetComboBoxIsTextSearchCaseSensitive(ComboBox comboBox)
		{
			var extBinding = BindingOperations.GetBinding(this, IsTextSearchCaseSensitiveProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(comboBox, ComboBox.IsTextSearchCaseSensitiveProperty, extBinding);
			}
			else
			{
				var isTextSearchCaseSensitiveBinding = new Binding("IsTextSearchCaseSensitive") { Source = this };
				BindingOperations.SetBinding(comboBox, ComboBox.IsTextSearchCaseSensitiveProperty, isTextSearchCaseSensitiveBinding);
			}
		}

		private void SetComboBoxMaxDropDownHeight(ComboBox comboBox)
		{
			var extBinding = BindingOperations.GetBinding(this, MaxDropDownHeightProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(comboBox, ComboBox.MaxDropDownHeightProperty, extBinding);
			}
			else
			{
				var maxDropDownHeightBinding = new Binding("MaxDropDownHeight") { Source = this };
				BindingOperations.SetBinding(comboBox, ComboBox.MaxDropDownHeightProperty, maxDropDownHeightBinding);
			}
		}

		private void SetComboBoxDataTemplate(ComboBox comboBox)
		{
			comboBox.ItemTemplate = ItemTemplate;
		}

		#endregion

		#region Protected Override Methods

		protected override void OnCellTemplateLoaded(ContentPresenter contentPresenter)
		{
			base.OnCellTemplateLoaded(contentPresenter);

			var comboBox = Utils.FindVisualChild<ComboBox>(contentPresenter);
			if (comboBox != null)
			{
				SetComboBoxDataTemplate(comboBox);
				SetComboBoxItemsSource(comboBox);
				SetComboBoxSelectedItem(comboBox);
				SetComboBoxIsEditable(comboBox);
				SetComboBoxIsTextSearchEnabled(comboBox);
				SetComboBoxIsTextSearchCaseSensitive(comboBox);
				SetComboBoxMaxDropDownHeight(comboBox);

				comboBox.Width = double.NaN;
				comboBox.Height = double.NaN;
			}
		}

		#endregion
	}
}
