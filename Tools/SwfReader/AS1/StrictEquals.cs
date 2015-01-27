#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class StrictEquals : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.StrictEquals;}}
		internal uint Version {get{return 6;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 2; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return -1; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("strictequals");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.StrictEquals);
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
