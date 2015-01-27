#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Enumerate : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.Enumerate;}}
		internal uint Version {get{return 5;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 1 + PropertyCount; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return (int)PropertyCount; } }

		internal uint PropertyCount { get { return 0; } } // todo: get number of props from, hmm...
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("enumerate");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.Enumerate);
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
