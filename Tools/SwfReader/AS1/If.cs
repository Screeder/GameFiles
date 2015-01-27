#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class If : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.If;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 5; } }

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -1; } }

		
		internal int BranchOffset;

		internal If(SwfReader r)
		{
			BranchOffset = r.GetInt16();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("branchiftrue " + ActionRecords.GetLabel(BranchOffset)); // todo: add label logic to if's
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.If);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendInt16(BranchOffset);
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
