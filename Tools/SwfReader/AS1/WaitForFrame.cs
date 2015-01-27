#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class WaitForFrame : IAction
	{
		// this is 'ifFrameLoaded' in swf, deprecated...
        internal ActionKind ActionId { get { return ActionKind.WaitForFrame; } }
        internal uint Version { get { return 3; } }
        internal uint Length { get { return 6; } }

        internal int Frame;
        internal uint SkipCount;

        internal WaitForFrame(SwfReader r)
		{
			Frame = r.GetUI16();
			SkipCount = (uint)r.GetByte();
		}
#if SWFASM
        internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("ifframeloaded");
		}
#endif
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.WaitForFrame);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendUI16((uint)Frame);
            w.AppendByte((byte)SkipCount);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("WaitForFrame: " + this.Frame + " or skip: " + this.SkipCount);
		}
#endif
	}
}
