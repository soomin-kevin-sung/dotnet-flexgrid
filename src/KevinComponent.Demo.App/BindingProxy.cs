using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KevinComponent.Demo.App
{
	public class BindingProxy : Freezable
	{
		#region Dependency Properties

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));

		#endregion

		#region Public Properties

		public object Data
		{
			get => GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		#endregion

		#region Protected Override Methods

		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		#endregion
	}
}
