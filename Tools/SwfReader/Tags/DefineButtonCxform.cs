#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineButtonCxform : ISwfTag
	{
        /*
            ButtonId                UI16    Button ID for this information
            ButtonColorTransform    CXFORM  Character color transform
        */
        internal uint ButtonId;
        internal ColorTransform ButtonColorTransform;

        internal DefineButtonCxform(SwfReader r)
            : base(TagType.DefineButtonCxform)
		{
            ButtonId = r.GetUI16();
            ButtonColorTransform = new ColorTransform(r, false);
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(ButtonId);
            ButtonColorTransform.ToSwf(w);

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineButtonCxform: ");
			w.WriteLine();
        }
#endif
    }
}
