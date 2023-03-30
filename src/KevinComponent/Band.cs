using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;
using KevinComponent.Assist;

namespace KevinComponent
{
	public abstract class Band : FrameworkElement
	{
		static Band()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Band), new FrameworkPropertyMetadata(typeof(Band)));
			WidthProperty.OverrideMetadata(typeof(Band), new FrameworkPropertyMetadata(20d));
			MinWidthProperty.OverrideMetadata(typeof(Band), new FrameworkPropertyMetadata(20d));
		}

		protected Band()
		{
			SyncDataGridColumn = new FlexGridColumn(this);

			Bands = new BandCollection(this);
			Bands.CollectionChanged += OnBandsCollectionChanged;
			Bands.VirtualBandItemsSourceChanged += OnVirtualBandItemsSourceChanged;
			Bands.VirtualBandItemsSourceCollectionChanged += OnVirtualBandItemsSourceCollectionChanged;
			
			UpdateDefaultStyle();
		}

		protected Band(VirtualBand virtualBand) : this()
		{
			OwnerFlexGrid = virtualBand.OwnerFlexGrid;
			ParentBand = virtualBand.ParentBand;
			CellStyle = virtualBand.CellStyle;
			MinWidth = virtualBand.MinWidth;
			MaxWidth = virtualBand.MaxWidth;
			Width = virtualBand.Width;
			HorizontalHeaderAlignment = virtualBand.HorizontalHeaderAlignment;
			VerticalHeaderAlignment = virtualBand.VerticalHeaderAlignment;
			DerivationFrom = virtualBand;

			if (virtualBand.HeaderBinding != null)
				BindingOperations.SetBinding(this, HeaderProperty, virtualBand.HeaderBinding);
			else
				Header = virtualBand.Header;
		}

		#region Dependency Properties

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register(
				"Header",
				typeof(object),
				typeof(Band),
				new FrameworkPropertyMetadata(null, OnHeaderPropertyChanged));
		public static readonly DependencyProperty HeaderStyleProperty =
			DependencyProperty.Register(
				"HeaderStyle",
				typeof(Style),
				typeof(Band),
				new FrameworkPropertyMetadata(null, OnHeaderStylePropertyChanged));
		public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
			DependencyProperty.Register(
				"HorizontalHeaderAlignment",
				typeof(HorizontalAlignment),
				typeof(Band),
				new FrameworkPropertyMetadata(HorizontalAlignment.Center));
		public static readonly DependencyProperty VerticalHeaderAlignmentProperty =
			DependencyProperty.Register(
				"VerticalHeaderAlignment",
				typeof(VerticalAlignment),
				typeof(Band),
				new FrameworkPropertyMetadata(VerticalAlignment.Center));
		public static readonly DependencyProperty CellTemplateProperty =
			DependencyProperty.Register(
				"CellTemplate",
				typeof(DataTemplate),
				typeof(Band),
				new FrameworkPropertyMetadata(null, OnCellTemplatePropertyChanged));
		public static readonly DependencyProperty CellEditingTemplateProperty =
			DependencyProperty.Register(
				"CellEditingTemplate",
				typeof(DataTemplate),
				typeof(Band),
				new FrameworkPropertyMetadata(null, OnCellEditingTemplatePropertyChanged));
		public static readonly DependencyProperty CellStyleProperty =
			DependencyProperty.Register(
				"CellStyle",
				typeof(Style),
				typeof(Band),
				new FrameworkPropertyMetadata(null, OnCellStylePropertyChanged));
		public static readonly DependencyProperty IsReadOnlyProperty =
			DependencyProperty.Register(
				"IsReadOnly",
				typeof(bool),
				typeof(Band),
				new FrameworkPropertyMetadata(false));
		public static readonly DependencyProperty CanUserSortProperty =
			DependencyProperty.Register(
				"CanUserSort",
				typeof(bool),
				typeof(Band),
				new FrameworkPropertyMetadata(false));
		public static readonly DependencyProperty SortDirectionProperty =
			DependencyProperty.Register(
				"SortDirection",
				typeof(ListSortDirection?),
				typeof(Band),
				new FrameworkPropertyMetadata(null));
		public static readonly DependencyProperty SortMemberPathProperty =
			DependencyProperty.Register(
				"SortMemberPath",
				typeof(string),
				typeof(Band),
				new FrameworkPropertyMetadata(string.Empty));

		private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = d as Band;
			if (band == null)
				return;

			bool useHeaderTemplate = false;
			var newValue = e.NewValue;
			if (newValue is DataTemplate dt)
			{
				var cp = new ContentPresenter();

				cp.ContentTemplate = dt;
				BindingOperations.SetBinding(cp, ContentPresenter.ContentProperty, new Binding());

				newValue = cp;
				useHeaderTemplate = true;
			}

			band.SyncDataGridColumn.Header = newValue;
			band.BandHeader.Content = newValue;
			band._useHeaderTemplate = useHeaderTemplate;
		}

		private static void OnHeaderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = d as Band;
			if (band == null)
				return;

			var newValue = e.NewValue as Style;
			if (newValue == null)
				return;

			band.BandHeader.Style = newValue;
		}

		private static void OnCellTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = d as Band;
			band?.OnCellTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		private static void OnCellEditingTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = d as Band;
			band?.OnCellEditingTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		private static void OnCellStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var band = d as Band;
			band?.OnCellStyleChanged((Style)e.OldValue, (Style)e.NewValue);
		}

		#endregion

		#region Private Variables

		BandHeader? _bandHeader;
		bool _useHeaderTemplate;
		FlexGrid? _ownerFlexGrid;
		bool _settingWithoutParentBand;
		bool _settingWithoutSubBands;
		Band? _parentBand;

		#endregion

		#region Internal Properties

		internal VirtualBand? DerivationFrom { get; }

		internal Band? ParentBand
		{
			get => _parentBand;
			set
			{
				if (_parentBand != value)
				{
					var oldParentBand = _parentBand;
					_parentBand = value;
					OnParentBandChanged(oldParentBand, value);
				}
			}
		}

		internal FlexGrid? OwnerFlexGrid
		{
			get => _ownerFlexGrid;
			set
			{
				if (_ownerFlexGrid != value)
				{
					var oldOwnerFlexGrid = _ownerFlexGrid;
					_ownerFlexGrid = value;
					OnOwnerFlexGridChanged(oldOwnerFlexGrid, value);
				}
			}
		}

		internal BandHeader BandHeader
		{
			get
			{
				if (_bandHeader == null)
					_bandHeader = new BandHeader(this);

				return _bandHeader;
			}
		}

		internal FlexGridColumn SyncDataGridColumn { get; }

		#endregion

		#region Public Properties

		public int Depth
			=> Bands.MaxDepth + 1;

		public BandCollection Bands { get; }

		public bool HasChildBands
			=> Bands.Count > 0;

		public object Header
		{
			get => GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public Style HeaderStyle
		{
			get => (Style)GetValue(HeaderStyleProperty);
			set => SetValue(HeaderStyleProperty, value);
		}

		public HorizontalAlignment HorizontalHeaderAlignment
		{
			get => (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty);
			set => SetValue(HorizontalHeaderAlignmentProperty, value);
		}

		public VerticalAlignment VerticalHeaderAlignment
		{
			get => (VerticalAlignment)GetValue(VerticalHeaderAlignmentProperty);
			set => SetValue(VerticalHeaderAlignmentProperty, value);
		}

		public DataTemplate CellTemplate
		{
			get => (DataTemplate)GetValue(CellTemplateProperty);
			set => SetValue(CellTemplateProperty, value);
		}

		public DataTemplate CellEditingTemplate
		{
			get => (DataTemplate)GetValue(CellEditingTemplateProperty);
			set => SetValue(CellEditingTemplateProperty, value);
		}

		public Style CellStyle
		{
			get => (Style)GetValue(StyleProperty);
			set => SetValue(StyleProperty, value);
		}

		public bool IsReadOnly
		{
			get => (bool)GetValue(IsReadOnlyProperty);
			set => SetValue(IsReadOnlyProperty, value);
		}

		public bool CanUserSort
		{
			get => (bool)GetValue(CanUserSortProperty);
			set => SetValue(CanUserSortProperty, value);
		}

		public ListSortDirection? SortDirection
		{
			get => (ListSortDirection?)GetValue(SortDirectionProperty);
			set => SetValue(SortDirectionProperty, value);
		}

		public string SortMemberPath
		{
			get => (string)GetValue(SortMemberPathProperty);
			set => SetValue(SortMemberPathProperty, value);
		}

		#endregion

		#region Private Methods

		private void SetBandsOwnerFlexGrid(FlexGrid? flexGrid)
		{
			Bands.OwnerFlexGrid = flexGrid;
		}

		private DataTemplate GetTemplate(bool isEditing)
		{
			if (isEditing && !IsReadOnly && CellEditingTemplate != null)
				return CellEditingTemplate;
			else
				return CellTemplate;
		}

		private void AttachEventHandlers(ContentPresenter contentPresenter, bool isEditing)
		{
			DetachEventHandlers(contentPresenter, isEditing);

			if (isEditing && !IsReadOnly)
			{
				contentPresenter.Loaded += OnCellEditingTemplateLoaded;
				contentPresenter.Unloaded += OnCellEditingTemplateUnloaded;
			}
			else
			{
				contentPresenter.Loaded += OnCellTemplateLoaded;
				contentPresenter.Unloaded += OnCellTemplateUnloaded;
			}
		}

		private void DetachEventHandlers(ContentPresenter contentPresenter, bool isEditing)
		{
			if (isEditing && !IsReadOnly)
			{
				contentPresenter.Loaded -= OnCellEditingTemplateLoaded;
				contentPresenter.Unloaded -= OnCellEditingTemplateUnloaded;
			}
			else
			{
				contentPresenter.Loaded -= OnCellTemplateLoaded;
				contentPresenter.Unloaded -= OnCellTemplateUnloaded;
			}
		}

		private void AttachEventHandlers(INotifyPropertyChanged dataContext)
		{
			DetachEventHandlers(dataContext);
			dataContext.PropertyChanged += OnDataContextPropertyChanged;
		}

		private void DetachEventHandlers(INotifyPropertyChanged dataContext)
		{
			dataContext.PropertyChanged -= OnDataContextPropertyChanged;
		}

		private double LimitWidthForPreventOverflow(ref double totalWidth, double newWidth)
		{
			totalWidth -= newWidth;
			if (totalWidth < 0)
			{
				var result = newWidth + totalWidth;
				totalWidth = 0;

				return result;
			}

			return newWidth;
		}

		private void SetSubBandsWidth(double newTotalWidth)
		{
			if (_settingWithoutSubBands)
				return;

			var unusedWidth = newTotalWidth;
			var prevTotalWidth = Bands.TotalWidth;
			foreach (var band in Bands)
			{
				if (band is VirtualBand vband)
				{
					foreach (var subBand in vband.VirtualizedBands)
					{
						var calcWidth = Math.Max(subBand.MinWidth, newTotalWidth * (subBand.Width / prevTotalWidth));
						calcWidth = LimitWidthForPreventOverflow(ref unusedWidth, calcWidth);
						subBand.SetWidthWithoutParentBand(calcWidth);
					}
				}
				else
				{
					var calcWidth = Math.Max(band.MinWidth, newTotalWidth * (band.Width / prevTotalWidth));
					calcWidth = LimitWidthForPreventOverflow(ref unusedWidth, calcWidth);
					band.SetWidthWithoutParentBand(calcWidth);
				}
			}
		}

		private void SetParentBandWidth(double addedWidth)
		{
			if (ParentBand == null || _settingWithoutParentBand)
				return;

			var newWidth = ParentBand.Width + addedWidth;
			newWidth = Math.Max(MinWidth, newWidth);
			newWidth = Math.Min(MaxWidth, newWidth);

			ParentBand.SetWidthWithoutSubBands(newWidth);
		}

		private void SetWidthWithoutParentBand(double width)
		{
			_settingWithoutParentBand = true;

			Width = width;

			_settingWithoutParentBand = false;
		}

		private void SetWidthWithoutSubBands(double width)
		{
			// Lock Setting SubBands.
			_settingWithoutSubBands = true;

			Width = width;

			// Unlock Setting SubBands.
			_settingWithoutSubBands = false;
		}

		private void UpdateAllWidths()
		{
			double totalWidth = 0d;
			double totalMinWidth = 0d;
			double totalMaxWidth = 0d;

			foreach (var band in Bands)
			{
				if (band is VirtualBand vband)
				{
					foreach (var subBand in vband.VirtualizedBands)
					{
						totalWidth += subBand.Width;
						totalMinWidth += subBand.MinWidth;
						totalMaxWidth += subBand.MaxWidth;
					}
				}
				else
				{
					totalWidth += band.Width;
					totalMinWidth += band.MinWidth;
					totalMaxWidth += band.MaxWidth;
				}
			}

			MinWidth = totalMinWidth;
			MaxWidth = totalMaxWidth;
			Width = totalWidth;

			ParentBand?.UpdateAllWidths();
		}

		private void SetBindingSource(BindingBase bindingBase, object? source)
		{
			if (bindingBase is Binding newBinding)
			{
				if (newBinding.Source == null
					&& newBinding.ElementName == null
					&& newBinding.RelativeSource == null)
					newBinding.Source = source;
			}
			else if (bindingBase is MultiBinding newMultiBinding)
			{
				foreach (var child in newMultiBinding.Bindings)
					SetBindingSource(child, source);
			}
			else if (bindingBase is PriorityBinding newPrioBinding)
			{
				foreach (var child in newPrioBinding.Bindings)
					SetBindingSource(child, source);
			}
		}

		private void UpdateVirtualBandBindings(ContentPresenter cp)
		{
			if (DerivationFrom == null && DataContext == null)
				return;

			var propertyAndBindings = Utils.GetBindings<IVirtualBandBindable>(cp);
			foreach (var (element, property, bindingBase) in propertyAndBindings)
			{
				var cell = FlexGridAssist.GetParentCell(cp);
				if (cell == null)
					continue;

				if (Utils.GetIndexerValue(cell.DataContext, new object[] { DataContext }, out object? bindingSource))
				{
					if (cell.DataContext is INotifyPropertyChanged dataContext)
						AttachEventHandlers(dataContext);

					var newBindingBase = Utils.CloneBinding((BindingBase)bindingBase);
					SetBindingSource(newBindingBase, bindingSource);

					BindingOperations.SetBinding(element, property, newBindingBase);
				}
				else
				{
					var newBindingBase = Utils.CloneBinding((BindingBase)bindingBase);
					SetBindingSource(newBindingBase, null);

					BindingOperations.SetBinding(element, property, newBindingBase);
				}
			}
		}

		private void SetWidth(double oldValue, double newValue)
		{
			if (this is VirtualBand)
				return;

			newValue = Math.Max(MinWidth, newValue);
			newValue = Math.Min(MaxWidth, newValue);

			BandHeader.Width = newValue;
			SetSubBandsWidth(newValue);
			SetParentBandWidth(newValue - oldValue);
		}

		private void SetSyncDataGridColumnMinWidth(double newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.MinWidth = newValue;
		}

		private void SetSyncDataGridColumnMaxWidth(double newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.MaxWidth = newValue;
		}

		private void SetSyncDataGridColumnCanUserSort(bool newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.CanUserSort = newValue;
		}

		private void SetSyncDataGridColumnSortDirection(ListSortDirection? newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.SortDirection = newValue;

			if (BandHeader != null)
				BandHeader.SortDirection = newValue;
		}

		private void SetSyncDataGridColumnSortMemberPath(string newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.SortMemberPath = newValue;
		}

		private void SetBandHeaderDataContext(object newValue)
		{
			if (BandHeader != null)
				BandHeader.DataContext = newValue;
		}

		#endregion

		#region Private EventHandlers

		private void OnVirtualBandItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateAllWidths();
		}

		private void OnVirtualBandItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UpdateAllWidths();
		}

		private void OnBandsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateAllWidths();
		}

		private void OnCellTemplateLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is ContentPresenter cp)
			{
				OnCellTemplateLoaded(cp);
				UpdateVirtualBandBindings(cp);
			}
		}

		private void OnCellTemplateUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is ContentPresenter cp)
			{
				cp.Loaded -= OnCellTemplateLoaded;
				cp.Unloaded -= OnCellTemplateUnloaded;
			}
		}

		private void OnCellEditingTemplateLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is ContentPresenter cp)
			{
				OnCellEditingTemplateLoaded(cp);
				UpdateVirtualBandBindings(cp);
				PrepareEdit(cp);
			}
		}

		private void OnCellEditingTemplateUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is ContentPresenter cp)
			{
				cp.Loaded -= OnCellEditingTemplateLoaded;
				cp.Unloaded -= OnCellEditingTemplateUnloaded;
			}
		}

		private void OnDataContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Item[]":
					OnDataContextIndexerChanged(sender);
					break;
			}
		}

		private void OnDataContextIndexerChanged(object? sender)
		{
			var row = OwnerFlexGrid?.ItemContainerGenerator.ContainerFromItem(sender) as DataGridRow;
			if (row == null)
				return;

			SyncDataGridColumn?.RefreshCellContent(row);
		}

		#endregion

		#region Internal Methods

		internal FrameworkElement GenerateElement(DataGridCell cell, bool isEditing)
		{
			var cp = new ContentPresenter();
			FlexGridAssist.SetParentCell(cp, cell);
			BindingOperations.SetBinding(cp, ContentPresenter.ContentProperty, new Binding());

			cp.ContentTemplate = GetTemplate(isEditing);

			AttachEventHandlers(cp, isEditing);

			return cp;
		}

		#endregion

		#region Public Methods

		public void PerformSort()
		{
			if (SyncDataGridColumn != null)
				OwnerFlexGrid?.PerformSort(SyncDataGridColumn);
		}

		#endregion

		#region Protected Virtual Methods

		protected virtual void OnParentBandChanged(Band? oldParent, Band? newParent) { }

		protected virtual void OnOwnerFlexGridChanged(FlexGrid? oldOwnerFlexGrid, FlexGrid? newOwnerFlexGrid)
		{
			SetBandsOwnerFlexGrid(newOwnerFlexGrid);
		}

		protected virtual void OnCellTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
		{
			if (OwnerFlexGrid != null)
			{
				foreach (var item in OwnerFlexGrid.Items)
				{
					var row = (DataGridRow)OwnerFlexGrid.ItemContainerGenerator.ContainerFromItem(item);
					if (row == null)
						continue;

					SyncDataGridColumn?.RefreshCellContent(row);
				}
			}
		}

		protected virtual void OnCellEditingTemplateChanged(DataTemplate oldValue, DataTemplate newValue) { }

		protected virtual void OnCellStyleChanged(Style oldValue, Style newValue)
		{
			if (SyncDataGridColumn != null)
				SyncDataGridColumn.CellStyle = newValue;
		}

		protected virtual void OnCellTemplateLoaded(ContentPresenter contentPresenter) { }

		protected virtual void OnCellEditingTemplateLoaded(ContentPresenter contentPresenter)
		{
			OnCellTemplateLoaded(contentPresenter);
		}

		protected virtual void PrepareEdit(ContentPresenter contentPresenter) { }

		#endregion

		#region Protected Override Methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			// Width Sync.
			if (e.Property == WidthProperty)
				SetWidth((double)e.OldValue, (double)e.NewValue);
			else if (e.Property == MinWidthProperty)
				SetSyncDataGridColumnMinWidth((double)e.NewValue);
			else if (e.Property == MaxWidthProperty)
				SetSyncDataGridColumnMaxWidth((double)e.NewValue);
			else if (e.Property == CanUserSortProperty)
				SetSyncDataGridColumnCanUserSort((bool)e.NewValue);
			else if (e.Property == SortDirectionProperty)
				SetSyncDataGridColumnSortDirection((ListSortDirection?)e.NewValue);
			else if (e.Property == SortMemberPathProperty)
				SetSyncDataGridColumnSortMemberPath((string)e.NewValue);
			else if (e.Property == DataContextProperty)
				SetBandHeaderDataContext(e.NewValue);
		}

		#endregion
	}
}
