#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class FrameLabelTag : IControlTag
	{
        internal string TargetName;
        private uint size;
        internal FrameLabelTag(SwfReader r)
            : base(TagType.FrameLabel)
		{
            uint __pos = r.Position;
			this.TargetName = r.GetString();
            size = r.Position - __pos;
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			w.AppendTagIDAndLength(this.TagType, (uint)TargetName.Length + 1, true);
			w.AppendString(TargetName); // todo: check for unicode implications on labels
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, size, "FRAMELABEL \"" + TargetName + "\""));
		}
#endif
    }
}
