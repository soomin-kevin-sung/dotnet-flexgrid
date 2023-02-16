using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
