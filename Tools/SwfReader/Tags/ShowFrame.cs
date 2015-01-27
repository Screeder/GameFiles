#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class ShowFrame : IControlTag
	{
        internal ShowFrame(SwfReader r)
            : base(TagType.ShowFrame)
		{
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint len = 0;
			w.AppendTagIDAndLength(this.TagType, len, false);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, 0, "SHOWFRAME"));
            //SHOWFRAME 1 (00:00:00,000) (label "ItemDataPack")
		}
#endif
    }
}
