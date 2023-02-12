using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KevinComponent
{
	public sealed class TemplateBand : Band
	{
		public TemplateBand() { }

		internal TemplateBand(VirtualTemplateBand virtualBand) : base(virtualBand)
		{
			CellTemplate = virtualBand.CellTemplate;
			CellEditingTemplate = virtualBand.CellEditingTemplate;
		}
	}
}
