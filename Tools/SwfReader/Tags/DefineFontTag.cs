#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineFontTag : ISwfTag
	{
		/*
			Header				RECORDHEADER	Tag type = 10
			FontID				UI16			ID for this font character
			OffsetTable			UI16[nGlyphs]	Array of shape offsets
			GlyphShapeTable		SHAPE[nGlyphs]	Array of shapes
		 */

		internal uint FontId;
		internal uint[] OffsetTable;
		//internal List<GlyphShape> GlyphShapeTable;

        internal DefineFontTag(SwfReader r)
            : base(TagType.DefineFont)
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
			w.Write("DefineFont: ");
			w.WriteLine();
        }
#endif
    }
}


						
						
						
						












