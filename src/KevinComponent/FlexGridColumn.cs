using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class FlexGridColumn : DataGridColumn
	{
		public FlexGridColumn(Band band)
		{
			OwnerBand = band;
			MinWidth = band.MinWidth;
			MaxWidth = band.MaxWidth;
			CanUserSort = band.CanUserSort;
			SortDirection = band.SortDirection;
			SortMemberPath = band.SortMemberPath;
		}

		public Band OwnerBand { get; }

		#region Internal Methods

		internal void RefreshCellContent(DataGridRow row)
		{
			if (DisplayIndex < 0)
				return;

			var presenter = Utils.FindVisualChild<DataGridCellsPresenter>(row);
			if (presenter == null)
				return;

			var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(DisplayIndex);
			RefreshCellContent(cell, string.Empty);
		}

		#endregion

		#region Protected Override Methods

		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return OwnerBand.GenerateElement(cell, true);
		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return OwnerBand.GenerateElement(cell, false);
		}

		protected override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			if (element is DataGridCell cell)
			{
				var newContent = OwnerBand.GenerateElement(cell, cell.IsEditing);
				var oldContent = cell.Content as FrameworkElement;
				if (oldContent != null && oldContent != newContent)
				{
					var oldContentPresenter = oldContent as ContentPresenter;
					if (oldContentPresenter == null)
						oldContent.SetValue(FrameworkElement.DataContextProperty, null);
					else
						oldContentPresenter.Content = null;
				}

				cell.Content = newContent;
			}
		}

		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			return base.CommitCellEdit(editingElement);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == SortDirectionProperty)
				OwnerBand.SortDirection = (ListSortDirection?)e.NewValue;
		}

		#endregion
	}
}
