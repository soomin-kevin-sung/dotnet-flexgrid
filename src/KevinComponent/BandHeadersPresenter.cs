using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KevinComponent
{
	[TemplatePart(Name = FrozenBandsWrapperPartName, Type = typeof(Grid))]
	[TemplatePart(Name = BandsScrollViewerPartName, Type = typeof(ScrollViewer))]
	[TemplatePart(Name = BandsWrapperPartName, Type = typeof(Grid))]
	public sealed class BandHeadersPresenter : Control
	{
		static BandHeadersPresenter()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BandHeadersPresenter), new FrameworkPropertyMetadata(typeof(BandHeadersPresenter)));
		}

		public BandHeadersPresenter()
		{

		}

		#region Contants

		public const string FrozenBandsWrapperPartName = "PART_FrozenBandsWrapper";
		public const string BandsScrollViewerPartName = "PART_BandsScrollViewer";
		public const string BandsWrapperPartName = "PART_BandsWrapper";

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty BandsScrollHorizontalOffsetProperty =
			DependencyProperty.Register(
				"BandsScrollHorizontalOffset",
				typeof(double),
				typeof(BandHeadersPresenter),
				new FrameworkPropertyMetadata(0d, OnBandsScrollHorizontalOffsetPropertyChanged));

		private static void OnBandsScrollHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var presenter = d as BandHeadersPresenter;
			presenter?._bandsScrollViewer?.ScrollToHorizontalOffset((double)e.NewValue);
		}

		#endregion

		#region Private Variables

		Grid? _frozenBandsWrapper;
		ScrollViewer? _bandsScrollViewer;
		Grid? _bandsWrapper;
		FlexGrid? _ownerFlexGrid;

		#endregion

		#region Public Properties

		public FlexGrid? OwnerFlexGrid
		{
			get
			{
				if (_ownerFlexGrid == null)
					_ownerFlexGrid = Utils.FindVisualParent<FlexGrid>(this);

				return _ownerFlexGrid;
			}
		}

		public double BandsScrollHorizontalOffset
		{
			get => (double)GetValue(BandsScrollHorizontalOffsetProperty);
			set => SetValue(BandsScrollHorizontalOffsetProperty, value);
		}

		#endregion

		#region Internal Methods

		internal void GenerateBandsElements()
		{
			if (OwnerFlexGrid == null || _frozenBandsWrapper == null || _bandsWrapper == null)
				return;

			var frozenBands = OwnerFlexGrid.FrozenBands;
			var bands = OwnerFlexGrid.Bands;

			_frozenBandsWrapper.Children.Clear();
			_bandsWrapper.Children.Clear();

			int maxRowCount = Math.Max(frozenBands.MaxDepth, bands.MaxDepth);
			ApplyColumnsAndRows(_frozenBandsWrapper, maxRowCount, frozenBands.BottomBandsCount);
			InsertBandHeaders(_frozenBandsWrapper, frozenBands, maxRowCount, 0, 0);

			ApplyColumnsAndRows(_bandsWrapper, maxRowCount, bands.BottomBandsCount);
			InsertBandHeaders(_bandsWrapper, bands, maxRowCount, 0, 0);
		}

		#endregion

		#region Private Methods

		private void ApplyColumnsAndRows(Grid wrapper, int rowCount, int colCount)
		{
			wrapper.ColumnDefinitions.Clear();
			wrapper.RowDefinitions.Clear();

			for (int i = 0; i < rowCount - 1; i++)
				wrapper.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

			wrapper.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < colCount; i++)
				wrapper.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

			wrapper.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
		}

		private void InsertBandHeaders(Grid wrapper, IEnumerable<Band> bands, int maxRowCount, int row, int col)
		{
			int usedColumnCount = 0;
			foreach (var band in bands)
			{
				if (band is VirtualBand vband)
				{
					InsertBandHeaders(wrapper, vband.VirtualizedBands, maxRowCount, row, col + usedColumnCount);
				}
				else
				{
					InsertBandHeaders(wrapper, band.Bands, maxRowCount - 1, row + 1, col + usedColumnCount);

					var bandHeader = band.BandHeader;
					Grid.SetColumn(bandHeader, col + usedColumnCount);
					Grid.SetRow(bandHeader, row);

					if (band.Bands.Count > 0)
					{
						Grid.SetRowSpan(bandHeader, 1);

						var colSpan = band.Bands.BottomBandsCount;
						Grid.SetColumnSpan(bandHeader, colSpan);

						usedColumnCount += colSpan;
					}
					else
					{
						Grid.SetRowSpan(bandHeader, maxRowCount);

						usedColumnCount++;
					}

					wrapper.Children.Add(bandHeader);
				}
			}
		}

		#endregion

		#region Public Override Methods

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_frozenBandsWrapper = GetTemplateChild(FrozenBandsWrapperPartName) as Grid;
			_bandsScrollViewer = GetTemplateChild(BandsScrollViewerPartName) as ScrollViewer;
			_bandsWrapper = GetTemplateChild(BandsWrapperPartName) as Grid;

		}

		#endregion

		#region Protected Override Methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == ActualHeightProperty)
			{
				//GenerateBandsElements();
			}
		}

		#endregion
	}
}
