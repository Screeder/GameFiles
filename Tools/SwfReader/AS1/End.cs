#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class End : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.End;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 0; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return 0; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("end");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.End);
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
