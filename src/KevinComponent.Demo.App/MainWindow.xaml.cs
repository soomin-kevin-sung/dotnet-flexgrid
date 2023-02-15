using KevinComponent.Demo.App.Models;
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
				new Sample("New Sample 1", () => new MainWindow().Show())
			};

			flexGrid.ItemsSource = Samples;
		}

		#region Public Properties

		public ObservableCollection<Sample> Samples { get; }

		#endregion
	}
}
