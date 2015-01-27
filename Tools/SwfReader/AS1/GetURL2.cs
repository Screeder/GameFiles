#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GetURL2 : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.GetURL2;}}
		internal uint Version {get{return 4;}}
		internal uint Length { get { return 4; } }

		internal uint StackPops { get { return 2; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -2; } }


		internal SendVarsMethod SendVarsMethod;		
		internal bool TargetIsSprite; // or sprite		
		internal bool LoadVariables;

		internal GetURL2(SwfReader r)
		{
			SendVarsMethod = (SendVarsMethod)r.GetBits(2);
			r.GetBits(4); // reserved
			TargetIsSprite = r.GetBit();
			LoadVariables = r.GetBit();
            r.Align();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("geturl2");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.GetURL2);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendBits((uint)SendVarsMethod, 2);
            w.AppendBits(0, 4);
            w.AppendBit(TargetIsSprite);
            w.AppendBit(LoadVariables);
            w.Align();
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(System.Enum.GetName(typeof(ActionKind), this.ActionId));
        }
#endif
    }

	internal enum SendVarsMethod
	{
		None = 0,
		Get = 1,
		Post = 2,
	}
}
