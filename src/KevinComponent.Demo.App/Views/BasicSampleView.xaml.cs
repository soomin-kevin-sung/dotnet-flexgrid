using KevinComponent.Demo.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
	/// BasicSample.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class BasicSampleView : Window
	{
		public BasicSampleView()
		{
			InitializeComponent();

			var list = new ObservableCollection<Person>();
			list.Add(new Person("Soomin", new DateTime(1997, 7, 25), "Korea Seoul", "https://github.com/soomin-kevin-sung"));
			list.Add(new Person("Foo", new DateTime(2000, 1, 12), "Korea Busan"));
			list.Add(new Person("Bar", new DateTime(1948, 7, 15), "Korea Daejeon"));
			list.Add(new Person("BatMan", new DateTime(1950, 1, 8), "Cave", "https://www.naver.com"));
			list.Add(new Person("IronMan", new DateTime(1970, 2, 16), "Stark Tower", "https://google.com"));
			list.Add(new Person("Amy", new DateTime(1987, 3, 19), "Toriktu 38"));
			list.Add(new Person("Ben", new DateTime(1999, 4, 25), "26, boulevard", "https://www.youtube.com/"));
			list.Add(new Person("Cake", new DateTime(1997, 4, 10), "China Town"));
			list.Add(new Person("David", new DateTime(1968, 2, 21), "The Room", "https://www.linkedin.com/"));
			list.Add(new Person("Echo", new DateTime(1951, 1, 25), "Dust", "https://github.com/soomin-kevin-sung"));
			list.Add(new Person("Fox", new DateTime(2001, 7, 25), "Estate"));
			list.Add(new Person("Golf", new DateTime(1993, 3, 25), "Italy"));
			list.Add(new Person("Hotel", new DateTime(1983, 10, 29), "Nuke", "https://games.crossfit.com/"));
			list.Add(new Person("India", new DateTime(1975, 9, 25), "Train"));
			list.Add(new Person("Juliet", new DateTime(1991, 12, 29), "Mirage", "https://cafe.naver.com/cfable2ah#"));
			list.Add(new Person("Kilo", new DateTime(1990, 7, 29), "Inferno", "https://github.com/soomin-kevin-sung/dotnet-flexgrid"));
			list.Add(new Person("Lima", new DateTime(2006, 11, 29), "Overpass"));

			flexGrid.ItemsSource = list;
		}

		private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			var info = new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true };
			Process.Start(info);
		}
	}
}
