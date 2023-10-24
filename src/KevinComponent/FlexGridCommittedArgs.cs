using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KevinComponent
{
	public class FlexGridCommittedArgs : EventArgs
	{
		public FlexGridCommittedArgs(DataGridCellInfo cellInfo)
		{
			CellInfo = cellInfo;
		}

		public DataGridCellInfo CellInfo { get; }
	}
}
