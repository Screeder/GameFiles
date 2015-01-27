#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Enumerate2 : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.Enumerate2;}}
		internal uint Version {get{return 6;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 1 + PropertyCount; } }
		internal int StackChange { get { return (int)PropertyCount; } }

		internal uint PropertyCount { get { return 0; } } // todo: somehow deterime number of props on object
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("enumeratevalue");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.Enumerate2);
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
