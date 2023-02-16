using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinComponent.Demo.App.Models
{
	public class Person : ObservableObject
	{
		public Person(string name, DateTime birthDate, string address, string? webSite = null)
		{
			_name = name;
			_birthDate = birthDate;
			_address = address;
			_webSite = webSite;
		}

		#region Pirvate Static Variables

		static ObservableCollection<Person>? _sampleData;

		#endregion

		#region Public Static Properties

		public static ObservableCollection<Person> SampleData
		{
			get
			{
				if (_sampleData == null)
				{
					_sampleData = new ObservableCollection<Person>();
					_sampleData.Add(new Person("Soomin", new DateTime(1997, 7, 25), "Korea Seoul", "https://github.com/soomin-kevin-sung"));
					_sampleData.Add(new Person("Foo", new DateTime(2000, 1, 12), "Korea Busan"));
					_sampleData.Add(new Person("Bar", new DateTime(1948, 7, 15), "Korea Daejeon"));
					_sampleData.Add(new Person("BatMan", new DateTime(1950, 1, 8), "Cave", "https://www.naver.com"));
					_sampleData.Add(new Person("IronMan", new DateTime(1970, 2, 16), "Stark Tower", "https://google.com"));
					_sampleData.Add(new Person("Amy", new DateTime(1987, 3, 19), "Toriktu 38"));
					_sampleData.Add(new Person("Ben", new DateTime(1999, 4, 25), "26, boulevard", "https://www.youtube.com/"));
					_sampleData.Add(new Person("Cake", new DateTime(1997, 4, 10), "China Town"));
					_sampleData.Add(new Person("David", new DateTime(1968, 2, 21), "The Room", "https://www.linkedin.com/"));
					_sampleData.Add(new Person("Echo", new DateTime(1951, 1, 25), "Dust", "https://github.com/soomin-kevin-sung"));
					_sampleData.Add(new Person("Fox", new DateTime(2001, 7, 25), "Estate"));
					_sampleData.Add(new Person("Golf", new DateTime(1993, 3, 25), "Italy"));
					_sampleData.Add(new Person("Hotel", new DateTime(1983, 10, 29), "Nuke", "https://games.crossfit.com/"));
					_sampleData.Add(new Person("India", new DateTime(1975, 9, 25), "Train"));
					_sampleData.Add(new Person("Juliet", new DateTime(1991, 12, 29), "Mirage", "https://cafe.naver.com/cfable2ah#"));
					_sampleData.Add(new Person("Kilo", new DateTime(1990, 7, 29), "Inferno", "https://github.com/soomin-kevin-sung/dotnet-flexgrid"));
					_sampleData.Add(new Person("Lima", new DateTime(2006, 11, 29), "Overpass"));
				}

				return _sampleData;
			}
		}

		#endregion

		#region Private Variables

		string _name;
		DateTime _birthDate;
		string _address;
		string? _webSite;

		#endregion

		#region Public Properties

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public DateTime BirthDate
		{
			get => _birthDate;
			set => SetProperty(ref _birthDate, value);
		}

		public string Address
		{
			get => _address;
			set => SetProperty(ref _address, value);
		}

		public string? WebSite
		{
			get => _webSite;
			set => SetProperty(ref _webSite, value);
		}

		#endregion
	}
}
