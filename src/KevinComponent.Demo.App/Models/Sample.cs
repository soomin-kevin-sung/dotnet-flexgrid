using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KevinComponent.Demo.App.Models
{
	public class Sample : ObservableObject
	{
		public Sample(string description, Action action)
		{
			_description = description;
			_action = action;
		}

		#region Private Variables

		Action _action;
		string _description;
		ICommand? _runCommand;

		#endregion

		#region Public Properties

		public string Description
		{
			get => _description;
			set => SetProperty(ref _description, value);
		}

		public ICommand RunCommand => _runCommand ??= new RelayCommand(Run);

		#endregion

		#region Public Methods

		public void Run()
		{
			_action?.Invoke();
		}

		#endregion
	}
}
