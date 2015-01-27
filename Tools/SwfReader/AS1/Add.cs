#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Add : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.Add;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 2; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return -1; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("oldadd");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.Add);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Add");
        }
#endif
    }
}
