using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class DefineFontAlignZonesTag : ISwfTag
	{
		/*
			Header			RECORDHEADER	Tag type = 73.
			FontID			UI16			ID of font to use, specified by DefineFont3.
			CSMTableHint	UB[2]			Font thickness hint. Refers to the thickness of the typical stroke used in the font.
											0 = thin
											1 = medium
											2 = thick 
											Flash Player maintains a selection of CSM tables for many fonts. 
											However, if the font is not found in Flash Player's internal table, 
											this hint is used to choose an appropriate table. 
			Reserved		UB[6]			Must be 0.
			ZoneTable		ZONERECORD		Alignment zone information for each glyph.
							[GlyphCount]	
		 */
        internal uint FontId;
        internal uint CSMTableHint;
        internal ZoneRecord[] ZoneTable;
        private Dictionary<uint, DefineFont2_3> Fonts;

        internal DefineFontAlignZonesTag(SwfReader r, Dictionary<uint, DefineFont2_3> fonts)
            : base(TagType.DefineFontAlignZones)
		{
            Fonts = fonts;

			FontId = r.GetUI16();
			CSMTableHint = r.GetBits(2);
			r.SkipBits(6);
			r.Align();

			DefineFont2_3 font = Fonts[FontId];
			uint glyphCount = font.NumGlyphs;

			ZoneTable = new ZoneRecord[glyphCount];
			for (int i = 0; i < glyphCount; i++)
			{
				ZoneTable[i] = new ZoneRecord(r);
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(FontId);
            w.AppendBits(CSMTableHint, 2);
            w.AppendBits(0, 6);
            w.Align();

			DefineFont2_3 font = Fonts[FontId];
			uint glyphCount = font.NumGlyphs;

			for (int i = 0; i < glyphCount; i++)
			{
				ZoneTable[i].ToSwf(w);
			}

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineFontAlignZones: ");
			w.WriteLine();
        }
#endif
    }
}

