using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineFontInfoTag : ISwfTag
	{
		/*
			Header					RECORDHEADER			Tag type = 13.
			FontID					UI16					Font ID this information is for.
			FontNameLen				UI8						Length of font name.
			FontName				UI8[FontNameLen]		Name of the font.
			FontFlagsReserved		UB[2]					Reserved bit fields.
			FontFlagsSmallText		UB[1]					SWF 7 file format or later: Font is small. 
															Character glyphs are aligned on pixel boundaries for dynamic and input text.
			FontFlagsShiftJIS		UB[1]					ShiftJIS character codes.
			FontFlagsANSI			UB[1]					ANSI character codes.
			FontFlagsItalic			UB[1]					Font is italic.
			FontFlagsBold			UB[1]					Font is bold.
			FontFlagsWideCodes		UB[1]					If 1, CodeTable is UI16 array; otherwise, CodeTable is UI8 array.
			CodeTable				If FontFlagsWideCodes	Glyph to code table, sorted in ascending order.
									UI16[nGlyphs]
									Otherwise UI8[nGlyphs]						
		 */

		internal uint FontId;
		internal string FontName;
		internal bool IsSmallText;
		internal bool IsShiftJis;
		internal bool IsAnsi;
		internal bool IsItalic;
		internal bool IsBold;
		internal bool IsWideCodes;
		internal Dictionary<uint, uint> GlyphMap;

        internal DefineFontInfoTag(SwfReader r)
            : base(TagType.DefineFontInfo)
		{
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineFontInfo: ");
			w.WriteLine();
		}
#endif
	}
}
