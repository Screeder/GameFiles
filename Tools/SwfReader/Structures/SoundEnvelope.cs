/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.Swf
{

	/*
	 SOUNDENVELOPE
		Pos44			UI32	Position of envelope point as a number of 44 kHz samples. Multiply accordingly if using a sampling rate less than 44kHz.
		LeftLevel		UI16	Volume level for left channel. Minimum is 0, maximum is 32768.
		RightLevel		UI16	Volume level for right channel. Minimum is 0, maximum is 32768.	
	*/
	internal class SoundEnvelope
	{
		internal uint Pos44;
		internal uint LeftLevel;
		internal uint RightLevel;

		internal SoundEnvelope(uint pos, uint left, uint right)
		{
			Pos44 = pos;
			LeftLevel = left;
			RightLevel = right;
		}

	}
}
