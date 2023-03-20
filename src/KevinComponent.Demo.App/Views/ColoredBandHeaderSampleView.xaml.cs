using KevinComponent.Demo.App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KevinComponent.Demo.App.Views
{
	/// <summary>
	/// ColoredBandHeaderSampleView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ColoredBandHeaderSampleView : Window
	{
		public ColoredBandHeaderSampleView()
		{
			InitializeComponent();
			flexGrid.ItemsSource = Person.SampleData;
		}

		private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			nameBand.HorizontalHeaderAlignment = HorizontalAlignment.Right;
			var info = new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true };
			Process.Start(info);
		}
	}
}
