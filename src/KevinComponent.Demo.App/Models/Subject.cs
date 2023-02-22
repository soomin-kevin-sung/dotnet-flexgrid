using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinComponent.Demo.App.Models
{
	public class Subject : ObservableObject
	{
		public Subject(string name)
		{
			_name = name;
		}

		#region Pirvate Static Variables

		static ObservableCollection<Subject>? _sampleData;

		#endregion

		#region Public Static Properties

		public static ObservableCollection<Subject> SampleData
		{
			get
			{
				if (_sampleData == null)
				{
					_sampleData = new ObservableCollection<Subject>();
					_sampleData.Add(new Subject("Math"));
					_sampleData.Add(new Subject("English"));
					_sampleData.Add(new Subject("Art"));
					_sampleData.Add(new Subject("Science"));
					_sampleData.Add(new Subject("History"));
					_sampleData.Add(new Subject("Music"));
				}

				return _sampleData;
			}
		}

		#endregion

		#region Private Variables

		string _name;

		#endregion

		#region Public Properties

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		#endregion
	}
}
