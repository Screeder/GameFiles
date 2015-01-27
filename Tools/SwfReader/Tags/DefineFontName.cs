#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineFontName : ISwfTag
	{
		/*
            Header                  RECORDHEADER            Tag type = 88
            FontID                  UI16                    ID for this font to which this refers
            FontName                STRING                  Name of the font. For fonts starting as Type 1, this is the
                                                            PostScript FullName. For fonts starting in sfnt formats such as
                                                            TrueType and OpenType, this is name ID 4, platform ID 1,
                                                            language ID 0 (Full name, Mac OS, English).
            FontCopyright           STRING                  Arbitrary string of copyright information
		 */

        internal uint FontID;
        internal string FontName;
        internal string FontCopyright;

        internal DefineFontName(SwfReader r)
            : base(TagType.DefineFontName)
		{
            FontID = r.GetUI16();
            FontName = r.GetString();
            FontCopyright = r.GetString();
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(FontID);
            w.AppendString(FontName);
            w.AppendString(FontCopyright);
            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineFontName: ");
			w.WriteLine();
		}
#endif
	}
}
