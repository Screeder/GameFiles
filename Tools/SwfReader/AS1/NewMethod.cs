#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class NewMethod : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.NewMethod;}}
		internal uint Version {get{return 5;}}
		internal uint Length { get { return 1; } }

		internal uint StackPops { get { return 3 + NumArgs; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return (int)(-2 - NumArgs); } }

		internal uint NumArgs { get { return 0; } } // get num args from stack
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("newmethod");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.NewMethod);
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
