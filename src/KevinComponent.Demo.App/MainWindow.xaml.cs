using KevinComponent.Demo.App.Models;
using KevinComponent.Demo.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KevinComponent.Demo.App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Samples = new ObservableCollection<Sample>
			{
				new Sample("Basic Sample", () => new BasicSampleView().Show()),
				new Sample("Frozen Header Sample", () => new FrozenHeaderView().Show()),
				new Sample("Merged Header Sample", () => new MergedHeaderSampleView().Show()),
				new Sample("VirtualBand Sample", () => { })
			};

			flexGrid.ItemsSource = Samples;
		}

		#region Public Properties

		public ObservableCollection<Sample> Samples { get; }

		#endregion
	}
}
