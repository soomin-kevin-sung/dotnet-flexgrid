using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class VirtualBandBinding : Binding, IVirtualBandBindable
	{
		public VirtualBandBinding() : base("")
		{
		}

		public VirtualBandBinding(string path) : base(path)
		{
		}

		public VirtualBandBinding(string path, BindingMode mode, UpdateSourceTrigger updateSourceTrigger) : base(path)
		{
			Mode = mode;
			UpdateSourceTrigger = updateSourceTrigger;
		}
	}
}
