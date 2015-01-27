#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GetVariable : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.GetVariable;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 1; } }
		
		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return 0; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("getvariable");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.GetVariable);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("GetVariable");
        }
#endif
    }
}
