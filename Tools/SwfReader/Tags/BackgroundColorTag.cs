#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class BackgroundColorTag : ISwfTag
	{
		internal RGBA Color;

        internal BackgroundColorTag(SwfReader r)
            : base(TagType.BackgroundColor)
		{
			this.Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), 0xFF);
		}

#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint len = 3;
			w.AppendTagIDAndLength(this.TagType, len, false);

			w.AppendByte(Color.R);
			w.AppendByte(Color.G);
			w.AppendByte(Color.B);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, 3, "SETBACKGROUNDCOLOR ( " + this.Color.R.ToString("X") + "/" + this.Color.G.ToString("X") + "/"+ this.Color.B.ToString("X") + " )"));
        }
#endif
    }
}
