using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KevinComponent
{
	public sealed class FlexGrid : DataGrid
	{
		static FlexGrid()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexGrid), new FrameworkPropertyMetadata(typeof(FlexGrid)));
		}

		public FlexGrid()
		{
			AutoGenerateColumns = false;
			FrozenBands = new BandCollection(this);
			Bands = new BandCollection(this);

			AttachEventHandlers();
			AttachEventHandlers(FrozenBands);
			AttachEventHandlers(Bands);
		}

		#region Depndency Properties

		public static readonly DependencyProperty ShowRowHeaderHighlightProperty =
			DependencyProperty.Register(
				"ShowRowHeaderHighlight",
				typeof(bool),
				typeof(FlexGrid));
		public static readonly DependencyProperty ShowColumnHeaderHighlightProperty =
			DependencyProperty.Register(
				"ShowColumnHeaderHighlight",
				typeof(bool),
				typeof(FlexGrid));
		public static readonly DependencyProperty UnSelectAllByEscapeKeyProperty =
			DependencyProperty.Register(
				"UnSelectAllByEscapeKey",
				typeof(bool),
				typeof(FlexGrid));

		#endregion

		#region Public Properties

		public BandCollection FrozenBands { get; }

		public BandCollection Bands { get; }

		public bool ShowRowHeaderHighlight
		{
			get => (bool)GetValue(ShowRowHeaderHighlightProperty);
			set => SetValue(ShowRowHeaderHighlightProperty, value);
		}

		public bool ShowColumnHeaderHighlight
		{
			get => (bool)GetValue(ShowColumnHeaderHighlightProperty);
			set => SetValue(ShowColumnHeaderHighlightProperty, value);
		}

		public bool UnSelectAllByEscapeKey
		{
			get => (bool)GetValue(UnSelectAllByEscapeKeyProperty);
			set => SetValue(UnSelectAllByEscapeKeyProperty, value);
		}

		#endregion

		#region Internal Properties

		internal BandHeadersPresenter? BandHeadersPresenter { get; private set; }

		#endregion

		#region Internal Methods

		internal void SyncColumnsWithBands()
		{
			if (!IsLoaded)
				return;

			BandHeadersPresenter?.GenerateBandsElements();

			var frozenBottomBands = FrozenBands.GetBottomBands();
			var bottomBands = Bands.GetBottomBands();

			FrozenColumnCount = frozenBottomBands.Length;

			var totalBottomBands = new List<Band>();
			totalBottomBands.AddRange(frozenBottomBands);
			totalBottomBands.AddRange(bottomBands);

			var totalBottomBandsHash = totalBottomBands.ToHashSet();

			// Remove Columns Not Needed.
			foreach (var column in Columns.ToArray())
			{
				if (column is not FlexGridColumn fgc || !totalBottomBandsHash.Contains(fgc.OwnerBand))
					Columns.Remove(column);
			}
			
			// Insert Columns.
			for (int i = 0; i < totalBottomBands.Count; i++)
			{
				var band = totalBottomBands[i]!;
				var columnIdx = band.SyncDataGridColumn.DisplayIndex;

				if (columnIdx != i)
				{
					if (columnIdx == -1)
						Columns.Insert(i, band.SyncDataGridColumn);
					else
						Columns.Move(columnIdx, i);
				}
			}
		}

		#endregion

		#region Private Methods

		private void AttachEventHandlers()
		{
			Loaded += OnLoaded;
		}

		private void AttachEventHandlers(BandCollection bands)
		{
			DetachEventHandlers(bands);
			bands.CollectionChanged += OnBandsCollectionChanged;

			foreach (var band in bands)
				AttachEventHandlers(band.Bands);
		}

		private void DetachEventHandlers(BandCollection bands)
		{
			bands.CollectionChanged -= OnBandsCollectionChanged;

			foreach (var band in bands)
				DetachEventHandlers(band.Bands);
		}

		private void PrepareForSort(DataGridColumn sortColumn)
		{
			if (Keyboard.IsKeyDown(Key.LeftShift)
				|| !Columns.Contains(sortColumn))
				return;

			if (Columns != null)
			{
				foreach (DataGridColumn column in Columns)
				{
					if (column != sortColumn)
						column.SortDirection = null;
				}
			}
		}

		#endregion

		#region Private EventHandlers

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			BandHeadersPresenter = Utils.FindVisualChild<BandHeadersPresenter>(this);
			SyncColumnsWithBands();
		}

		private void OnBandsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender is BandCollection bands)
				AttachEventHandlers(bands);

			SyncColumnsWithBands();
		}

		#endregion

		#region Public Methods

		public void RefreshCells(Band band)
		{
			if (band.SyncDataGridColumn == null || ItemsSource == null)
				return;

			foreach (var item in ItemsSource)
			{
				var row = (DataGridRow)ItemContainerGenerator.ContainerFromItem(item);
				if (row != null)
					band.SyncDataGridColumn.RefreshCellContent(row);
			}
		}

		public void PerformSort(DataGridColumn sortColumn)
		{
			if (!CanUserSortColumns || !sortColumn.CanUserSort)
				return;

			if (CommitEdit())
			{
				PrepareForSort(sortColumn);

				var args = new DataGridSortingEventArgs(sortColumn);
				OnSorting(args);

				if (Items.NeedsRefresh)
				{
					try
					{
						Items.Refresh();
					}
					catch
					{
						Items.SortDescriptions.Clear();
					}
				}
			}
		}

		#endregion

		#region Protected Override Methods

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.Key)
			{
				case Key.Escape:
					{
						if (UnSelectAllByEscapeKey)
							UnselectAllCells();
					}
					break;
			}
		}

		#endregion
	}
}
