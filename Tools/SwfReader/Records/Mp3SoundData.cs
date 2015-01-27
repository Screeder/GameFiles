/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Mp3SoundData
	{
		internal uint SeekSamples;
		internal Mp3Frame[] Frames;

		internal Mp3SoundData(SwfReader r)
		{
			this.SeekSamples = r.GetUI16();
		}
	}
}
