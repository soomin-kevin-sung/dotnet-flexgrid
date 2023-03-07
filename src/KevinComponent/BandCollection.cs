using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KevinComponent
{
	public sealed class BandCollection : ObservableCollection<Band>
	{
		internal BandCollection(FlexGrid flexGrid)
		{
			OwnerFlexGrid = flexGrid;
		}

		internal BandCollection(Band parentBand)
		{
			OwnerFlexGrid = parentBand.OwnerFlexGrid;
			ParentBand = parentBand;
		}

		#region Private Variables

		FlexGrid? _ownerFlexGrid;

		#endregion

		#region Public Events

		public event DependencyPropertyChangedEventHandler? VirtualBandItemsSourceChanged;
		public event NotifyCollectionChangedEventHandler? VirtualBandItemsSourceCollectionChanged;

		#endregion

		#region Internal Properties

		internal FlexGrid? OwnerFlexGrid
		{
			get => _ownerFlexGrid;
			set
			{
				if (_ownerFlexGrid != value)
				{
					_ownerFlexGrid = value;

					foreach (var band in this)
						band.OwnerFlexGrid = value;
				}
			}
		}

		#endregion

		#region Public Properties

		public Band? ParentBand { get; }

		public int MaxDepth
		{
			get
			{
				int max = 0;
				foreach (var band in this)
					max = Math.Max(max, band.Depth);

				return max;
			}
		}

		public int BottomBandsCount
		{
			get
			{
				int sum = 0;
				foreach (var band in this)
				{
					if (band is VirtualBand vband)
						sum += vband.VirtualizedBands.Length;
					else if (!band.HasChildBands)
						sum++;
					else
						sum += band.Bands.BottomBandsCount;
				}

				return sum;
			}
		}

		public double TotalWidth
		{
			get
			{
				double total = 0;
				foreach (var band in this)
				{
					if (band is VirtualBand vband)
						total += vband.VirtualizedBands.Sum(t => t.Width);
					else if (band.Bands.Count > 0)
						total += band.Bands.TotalWidth;
					else
						total += band.Width;
				}

				return total;
			}
		}

		#endregion

		#region Private Methods

		private Band[] GetBottomBands(Band band)
		{
			if (band is VirtualBand vband)
			{
				if (vband.VirtualizedBands.Length > 0)
					return vband.VirtualizedBands;
				else
					return new Band[0];
			}
			else if (!band.HasChildBands)
			{
				return new Band[] { band };
			}

			var result = new List<Band>();
			foreach (var subBand in band.Bands)
				result.AddRange(GetBottomBands(subBand));

			return result.ToArray();
		}

		private void AttachEventHandlers(VirtualBand vband)
		{
			DetachEventHandlers(vband);

			vband.ItemsSourceChanged += OnVirtualBandItemsSourceChanged;
			vband.ItemsSourceCollectionChanged += OnVirtualBandItemsSourceCollectionChanged;
		}

		private void DetachEventHandlers(VirtualBand vband)
		{
			vband.ItemsSourceChanged -= OnVirtualBandItemsSourceChanged;
			vband.ItemsSourceCollectionChanged -= OnVirtualBandItemsSourceCollectionChanged;
		}

		#endregion

		#region Private EventHandlers

		private void OnVirtualBandItemsSourceChanged(object? sender, DependencyPropertyChangedEventArgs e)
		{
			VirtualBandItemsSourceChanged?.Invoke(sender, e);
		}

		private void OnVirtualBandItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			VirtualBandItemsSourceCollectionChanged?.Invoke(sender, e);
		}

		#endregion

		#region Public Methods

		public Band[] GetBottomBands()
		{
			var result = new List<Band>();
			foreach (var band in this)
				result.AddRange(GetBottomBands(band));

			return result.ToArray();
		}

		#endregion

		#region Protected Override Methods

		protected override void InsertItem(int index, Band item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (ParentBand is VirtualBand)
				throw new InvalidOperationException("Cannot modify child bands when ParentBand is VirtualBand Type.");

			item.ParentBand = ParentBand;
			item.OwnerFlexGrid = OwnerFlexGrid;

			if (item is VirtualBand vband)
				AttachEventHandlers(vband);

			base.InsertItem(index, item);
		}

		protected override void ClearItems()
		{
			if (ParentBand is VirtualBand)
				throw new InvalidOperationException("Cannot modify child bands when ParentBand is VirtualBand Type.");

			foreach (var item in this)
			{
				if (item != null)
				{
					item.ParentBand = null;
					item.OwnerFlexGrid = null;

					if (item is VirtualBand vband)
						DetachEventHandlers(vband);
				}
			}

			base.ClearItems();
		}

		protected override void SetItem(int index, Band item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (ParentBand is VirtualBand)
				throw new InvalidOperationException("Cannot modify child bands when ParentBand is VirtualBand Type.");

			var oldItem = this[index];
			if (oldItem is VirtualBand oldVband)
				DetachEventHandlers(oldVband);

			oldItem.ParentBand = null;
			oldItem.OwnerFlexGrid = null;

			item.ParentBand = ParentBand;
			item.OwnerFlexGrid = OwnerFlexGrid;

			if (item is VirtualBand vband)
				AttachEventHandlers(vband);

			base.SetItem(index, item);
		}

		#endregion
	}
}
