//////////////////////////////////////////////////////////////////////////////
// This source code and all associated files and resources are copyrighted by
// the author(s). This source code and all associated files and resources may
// be used as long as they are used according to the terms and conditions set
// forth in The Code Project Open License (CPOL), which may be viewed at
// http://www.blackbeltcoder.com/Legal/Licenses/CPOL.
//
// Copyright (c) 2010 Jonathan Wood
//

using System;

namespace JBLib.Evaluate
{
    // ProcessSymbol arguments
    public class SymbolEventArgs : EventArgs
	{
		public string Name { get; set; }

		public double Result { get; set; }

		public SymbolStatus Status { get; set; }
	}
}
