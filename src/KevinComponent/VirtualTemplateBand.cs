using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KevinComponent
{
	public sealed class VirtualTemplateBand : VirtualBand
	{
		#region Private Methods

		private void SetCellTemplate(DataTemplate newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TemplateBand templateBand)
					templateBand.CellTemplate = newValue;
			}
		}

		private void SetCellEditingTemplate(DataTemplate newValue)
		{
			foreach (var band in VirtualizedBands)
			{
				if (band is TemplateBand templateBand)
					templateBand.CellEditingTemplate = newValue;
			}
		}

		#endregion

		#region Protected Override Methods

		protected override Band CreateVirtualizedBand()
		{
			return new TemplateBand(this);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == CellTemplateProperty)
				SetCellTemplate((DataTemplate)e.NewValue);
			else if (e.Property == CellEditingTemplateProperty)
				SetCellEditingTemplate((DataTemplate)e.NewValue);
		}

		#endregion
	}
}
