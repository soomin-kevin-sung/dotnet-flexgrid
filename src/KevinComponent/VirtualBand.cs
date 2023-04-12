using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace KevinComponent
{
	public abstract class VirtualBand : Band
	{
		protected VirtualBand()
		{
			_virtualizedBands = new ObservableCollection<Band>();
		}

		#region Dependency Properties

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register(
				"ItemsSource",
				typeof(IEnumerable),
				typeof(VirtualBand),
				new FrameworkPropertyMetadata(null, OnItemsSourcePropertyChanged));

		private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = (VirtualBand)d;

			if (e.OldValue is INotifyCollectionChanged oldCollection)
				band.DetachEventHandlers(oldCollection);

			if (e.NewValue is INotifyCollectionChanged newCollection)
				band.AttachEventHandlers(newCollection);

			if (band.ItemsSource != null)
				band.VirtualizeBands();

			band.ItemsSourceChanged?.Invoke(d, e);
		}

		#endregion

		#region Private Variables

		BindingBase _headerBinding;
		Band _dummyBand;
		ObservableCollection<Band> _virtualizedBands;

		#endregion

		#region Public Events

		public event DependencyPropertyChangedEventHandler ItemsSourceChanged;
		public event NotifyCollectionChangedEventHandler ItemsSourceCollectionChanged;

		#endregion

		#region Internal Properties

		internal Band DummyBand
		{
			get
			{
				if (_dummyBand == null)
				{
					_dummyBand = new TextBand(this);
					_dummyBand.IsReadOnly = true;
				}

				return _dummyBand;
			}
		}

		#endregion

		#region Public Properties

		public BindingBase HeaderBinding
		{
			get => _headerBinding;
			set
			{
				if (_headerBinding != value)
				{
					_headerBinding = value;
					SetHeaderBinding(value);
				}
			}
		}

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public Band[] VirtualizedBands
		{
			get
			{
				if (_virtualizedBands.Count == 0)
					return new Band[] { DummyBand };
				else
					return _virtualizedBands.ToArray();
			}
		}

		#endregion

		#region Private Methods

		private void AttachEventHandlers(INotifyCollectionChanged collection)
		{
			DetachEventHandlers(collection);
			collection.CollectionChanged += OnItemsSourceCollectionChanged;
		}

		private void DetachEventHandlers(INotifyCollectionChanged collection)
		{
			collection.CollectionChanged -= OnItemsSourceCollectionChanged;
		}

		private void VirtualizeBands()
		{
			if (ItemsSource == null)
			{
				_virtualizedBands.Clear();
				return;
			}

			var items = ItemsSource.Cast<object>().ToArray();
			SyncVirtualizedBandsCount(items);
			for (int i = 0; i < items.Length; i++)
			{
				if (!Utils.IsEqualValue(_virtualizedBands[i].DataContext, items[i]))
				{
					_virtualizedBands[i].DataContext = items[i];
					OwnerFlexGrid.RefreshCells(_virtualizedBands[i]);
				}
			}

			OwnerFlexGrid.SyncColumnsWithBands();
		}

		private void SyncVirtualizedBandsCount(object[] items)
		{
			while (_virtualizedBands.Count > items.Length)
				_virtualizedBands.RemoveAt(_virtualizedBands.Count - 1);

			while (_virtualizedBands.Count < items.Length)
			{
				var band = CreateVirtualizedBand();
				_virtualizedBands.Add(band);
			}
		}

		private void SetParentBand(Band newValue)
		{
			foreach (var band in VirtualizedBands)
				band.ParentBand = newValue;
		}

		private void SetHeader(object newValue)
		{
			if (HeaderBinding != null)
				return;

			foreach (var band in VirtualizedBands)
			{
				band.Header = newValue;
			}	
		}

		private void SetHeaderBinding(BindingBase newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (newValue != null)
					BindingOperations.SetBinding(band, HeaderProperty, newValue);
				else
					BindingOperations.ClearBinding(band, HeaderProperty);
			}

			if (newValue == null)
				SetHeader(Header);
		}

		private void SetCellTemplate(DataTemplate newValue)
		{
			foreach (var band in VirtualizedBands)
				band.CellTemplate = newValue;
		}

		private void SetCellEditingTemplate(DataTemplate newValue)
		{
			foreach (var band in VirtualizedBands)
				band.CellEditingTemplate = newValue;
		}

		private void SetCellStyle(Style newValue)
		{
			foreach (var band in VirtualizedBands)
				band.CellStyle = newValue;
		}

		private void SetWidth(double newValue)
		{
			foreach (var band in VirtualizedBands)
				band.Width = newValue;
		}

		private void SetMaxWidth(double newValue)
		{
			foreach (var band in VirtualizedBands)
				band.MaxWidth = newValue;
		}

		private void SetMinWidth(double newValue)
		{
			foreach (var band in VirtualizedBands)
				band.MinWidth = newValue;
		}

		private void SetHorizontalHeaderAlignment(HorizontalAlignment newValue)
		{
			foreach (var band in VirtualizedBands)
				band.HorizontalHeaderAlignment = newValue;
		}

		private void SetVerticalHeaderAlignment(VerticalAlignment newValue)
		{
			foreach (var band in VirtualizedBands)
				band.VerticalHeaderAlignment = newValue;
		}

		#endregion

		#region Private EventHandlers

		private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			VirtualizeBands();
			ItemsSourceCollectionChanged?.Invoke(this, e);
		}

		#endregion

		#region Protected Abstract Methods

		protected abstract Band CreateVirtualizedBand();

		#endregion

		#region Protected Override Methods

		protected override void OnParentBandChanged(Band oldParent, Band newParent)
		{
			base.OnParentBandChanged(oldParent, newParent);

			SetParentBand(newParent);
		}

		protected override void OnOwnerFlexGridChanged(FlexGrid oldOwnerFlexGrid, FlexGrid newOwnerFlexGrid)
		{
			base.OnOwnerFlexGridChanged(oldOwnerFlexGrid, newOwnerFlexGrid);

			foreach (var vband in VirtualizedBands)
				vband.OwnerFlexGrid = newOwnerFlexGrid;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == WidthProperty)
				SetWidth((double)e.NewValue);
			else if (e.Property == MaxWidthProperty)
				SetMaxWidth((double)e.NewValue);
			else if (e.Property == MinWidthProperty)
				SetMinWidth((double)e.NewValue);
			else if (e.Property == HeaderProperty)
				SetHeader(e.NewValue);
			else if (e.Property == CellTemplateProperty)
				SetCellTemplate((DataTemplate)e.NewValue);
			else if (e.Property == CellEditingTemplateProperty)
				SetCellEditingTemplate((DataTemplate)e.NewValue);
			else if (e.Property == CellStyleProperty)
				SetCellStyle((Style)e.NewValue);
			else if (e.Property == HorizontalHeaderAlignmentProperty)
				SetHorizontalHeaderAlignment((HorizontalAlignment)e.NewValue);
			else if (e.Property == VerticalHeaderAlignmentProperty)
				SetVerticalHeaderAlignment((VerticalAlignment)e.NewValue);
		}

		#endregion
	}
}
