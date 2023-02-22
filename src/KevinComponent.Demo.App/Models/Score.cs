using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinComponent.Demo.App.Models
{
	public class Score : ObservableObject
	{
		public Score(double value)
		{
			Value = value;
		}

		#region Private Variables

		double _value;

		#endregion

		#region Public Properties

		public double Value
		{
			get => _value;
			set
			{
				if (SetProperty(ref _value, value))
					OnPropertyChanged(nameof(Grade));
			}
		}

		public string Grade
		{
			get
			{
				if (_value < 60)
					return "F";
				else if (_value < 65)
					return "D";
				else if (_value < 70)
					return "D+";
				else if (_value < 75)
					return "C";
				else if (_value < 80)
					return "C+";
				else if (_value < 85)
					return "B";
				else if (_value < 90)
					return "B+";
				else if (_value < 95)
					return "A";
				else
					return "A+";
			}
		}

		#endregion
	}
}
