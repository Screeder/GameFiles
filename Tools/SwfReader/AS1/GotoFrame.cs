#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GotoFrame : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.GotoFrame;}}
		internal uint Version {get{return 3;}}
		internal uint Length	{get{return 5;}}	
		internal int Frame;

		internal GotoFrame(SwfReader r)
		{
			Frame = r.GetInt16();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("gotoandplay"); // todo: need to look at prev/next instr to find out if this is play or stop
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.GotoFrame);
            w.AppendUI16(Length - 3);// don't incude this part
            w.AppendInt16(Frame);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("GotoFrame: " + this.Frame);
        }
#endif
	}
}
