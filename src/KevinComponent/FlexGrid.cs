﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
			ExtendHorizontalScrollToFrozenColumns = false;
			FrozenBands = new BandCollection(this);
			Bands = new BandCollection(this);

			AttachEventHandlers();
			AttachEventHandlers(FrozenBands);
			AttachEventHandlers(Bands);
		}

		#region Depndency Properties

		public static readonly DependencyProperty UnSelectAllByEscapeKeyProperty =
			DependencyProperty.Register(
				"UnSelectAllByEscapeKey",
				typeof(bool),
				typeof(FlexGrid));

		/// <summary>
		/// 열고정 할 경우, 아래 스크롤에 대한 확장 여부
		/// </summary>
		public static readonly DependencyProperty ExtendHorizontalScrollToFrozenColumnsProperty =
			DependencyProperty.Register(
				"ExtendHorizontalScrollToFrozenColumns",
				typeof(bool),
				typeof(FlexGrid));

		#endregion

		#region Public Events

		public event EventHandler<FlexGridCommittedArgs> Committed;

		#endregion

		#region Public Properties

		public BandCollection FrozenBands { get; }

		public BandCollection Bands { get; }

		public bool UnSelectAllByEscapeKey
		{
			get => (bool)GetValue(UnSelectAllByEscapeKeyProperty);
			set => SetValue(UnSelectAllByEscapeKeyProperty, value);
		}

		public bool IsEditing
		{
			get
			{
				var row = (DataGridRow)ItemContainerGenerator.ContainerFromItem(CurrentCell.Item);
				if (row == null)
					return false;

				return row.IsEditing;
			}
		}

		/// <summary>
		/// 열고정 할 경우, 아래 스크롤에 대한 확장 여부
		/// </summary>
		public bool ExtendHorizontalScrollToFrozenColumns
		{
			get => (bool)GetValue(ExtendHorizontalScrollToFrozenColumnsProperty);
			set => SetValue(ExtendHorizontalScrollToFrozenColumnsProperty, value);
		}

		#endregion

		#region Internal Properties

		internal BandHeadersPresenter BandHeadersPresenter { get; private set; }

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
				if (!(column is FlexGridColumn fgc) ||
					!totalBottomBandsHash.Contains(fgc.OwnerBand) ||
					fgc.OwnerBand.SyncDataGridColumn != fgc)
					Columns.Remove(column);
			}

			// Insert Columns.
			for (int i = 0; i < totalBottomBands.Count; i++)
			{
				var band = totalBottomBands[i];
				var columnIdx = band.SyncDataGridColumn.DisplayIndex;

				if (columnIdx != i)
				{
					if (columnIdx == -1)
						Columns.Insert(i, band.SyncDataGridColumn);
					else
						Columns.Move(columnIdx, i);

					if (band.SortDirection != null)
						PerformSort(band.SyncDataGridColumn);
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

			Columns.Clear();
			SyncColumnsWithBands();
		}

		private void OnBandsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var bands = sender as BandCollection;
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
			if (!CanUserSortColumns)
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

		protected override void OnExecutedCommitEdit(ExecutedRoutedEventArgs e)
		{
			base.OnExecutedCommitEdit(e);

			var propertyInfo = typeof(DataGrid).GetProperty("CurrentCellContainer", BindingFlags.NonPublic | BindingFlags.Instance);
			if (propertyInfo.GetValue(this) is DataGridCell cell)
				Committed?.Invoke(this, new FlexGridCommittedArgs(new DataGridCellInfo(cell)));
			else if (CurrentCell != null)
				Committed?.Invoke(this, new FlexGridCommittedArgs(CurrentCell));
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);

			switch (e.Key)
			{
				case Key.Enter:
					{
						// 편집 중 Enter 클릭 시
						if (IsEditing)
						{
							// 적용하고 Enter 이벤트를 처리했다고 표기
							CommitEdit(DataGridEditingUnit.Row, true);
							e.Handled = true;
						}
					}
					break;
			}
		}

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
