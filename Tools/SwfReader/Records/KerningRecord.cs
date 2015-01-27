/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class KerningRecord
	{
		internal uint FontKerningCode1;
		internal uint FontKerningCode2;
		internal int FontKerningAdjustment;		

		internal KerningRecord(uint fontKerningCode1, uint fontKerningCode2, int fontKerningAdjustment)
		{
			this.FontKerningCode1 = fontKerningCode1;
			this.FontKerningCode2 = fontKerningCode2;
			this.FontKerningAdjustment = fontKerningAdjustment;
		}
	}
}
