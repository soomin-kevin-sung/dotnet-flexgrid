using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace KevinComponent
{
	public sealed class BandHeaderGripper : Thumb
	{
		static BandHeaderGripper()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BandHeaderGripper), new FrameworkPropertyMetadata(typeof(BandHeaderGripper)));
		}
	}
}
