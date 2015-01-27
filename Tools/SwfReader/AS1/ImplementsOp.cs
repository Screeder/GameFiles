#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ImplementsOp : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.ImplementsOp;}}
		internal uint Version {get{return 7;}}
		internal uint Length {get{return 1;}}

		internal uint StackPops { get { return 2 + ImplementedInterfaces; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return (int)(-1 - ImplementedInterfaces); } }

		internal uint ImplementedInterfaces { get { return 1; } } // todo: find interface count from stack
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("implements"); 
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.ImplementsOp);
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
