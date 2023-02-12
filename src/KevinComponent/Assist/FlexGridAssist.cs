using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KevinComponent.Assist
{
	public static class FlexGridAssist
	{
		#region Dependecny Attached Properties

		public static readonly DependencyProperty ParentCellProperty =
			DependencyProperty.RegisterAttached(
				"ParentCell",
				typeof(DataGridCell),
				typeof(FlexGridAssist),
				new FrameworkPropertyMetadata(null));

		public static DataGridCell GetParentCell(DependencyObject element) => (DataGridCell)element.GetValue(ParentCellProperty);
		public static void SetParentCell(DependencyObject element, DataGridCell value) => element.SetValue(ParentCellProperty, value);

		#endregion
	}
}
