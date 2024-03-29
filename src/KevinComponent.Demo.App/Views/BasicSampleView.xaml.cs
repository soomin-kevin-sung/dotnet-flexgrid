﻿using KevinComponent.Demo.App.Models;
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
			flexGrid.ItemsSource = Person.SampleData;
		}

		private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			var info = new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true };
			Process.Start(info);
		}
	}
}
