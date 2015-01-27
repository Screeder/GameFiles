#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class StartDrag : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.StartDrag;}}
		internal uint Version {get{return 4;}}
		internal uint Length {get{return 1;}}

		internal uint StackPops { get { return (uint)(3 + (Constrained ? 4 : 0)); } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return (int)(-3 - (Constrained ? 4 : 0)); } }

		internal bool Constrained { get { return false; } } // todo: find if constrained from stack
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("stopdrag");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.StartDrag);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine(System.Enum.GetName(typeof(ActionKind), this.ActionId));
        }
#endif
	}
}
