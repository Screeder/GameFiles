#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Extends : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.Extends;}}
		internal uint Version {get{return 7;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 2; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -2 ; } }

#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("extends");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.Extends);
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
