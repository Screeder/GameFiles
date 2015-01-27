/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GlyphEntry
	{
		internal uint GlyphIndex;
		internal int GlyphAdvance;

		internal GlyphEntry(uint glyphIndex, int glyphAdvance)
		{
			this.GlyphIndex = glyphIndex;
			this.GlyphAdvance = glyphAdvance;
		}
	}
}
