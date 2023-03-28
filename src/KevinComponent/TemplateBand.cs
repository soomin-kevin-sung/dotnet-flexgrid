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

		#region Protected Override Methods

		protected override void PrepareEdit(ContentPresenter contentPresenter)
		{
			base.PrepareEdit(contentPresenter);

			var textBox = Utils.FindVisualChild<TextBox>(contentPresenter);
			if (textBox != null)
			{
				textBox.Focus();
				textBox.SelectAll();
			}
		}

		#endregion
	}
}
