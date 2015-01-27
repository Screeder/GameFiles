#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Call : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.Call;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 6; } }

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -1; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("callframe");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.Call);
            w.AppendUI16(Length - 3); // don't incude def byte and len

		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("Call: " + System.Enum.GetName(typeof(ActionKind), this.ActionId));
		}
#endif
    }
}
