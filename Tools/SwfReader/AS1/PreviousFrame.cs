#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PreviousFrame : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.PreviousFrame;}}
		internal uint Version {get{return 3;}}
		internal uint Length {get{return 1;}}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("prevframe");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.PreviousFrame);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("PrevFrame");
		}
#endif
    }
}
