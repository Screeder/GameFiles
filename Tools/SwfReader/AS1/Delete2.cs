#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Delete2 : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.Delete2;}}
		internal uint Version {get{return 5;}}
		internal uint Length {get{return 1;}}

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -1; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("delete2");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.Delete2);
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
