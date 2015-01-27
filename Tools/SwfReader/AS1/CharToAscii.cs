#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class CharToAscii : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.CharToAscii;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return 0; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("ord");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.CharToAscii);
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
