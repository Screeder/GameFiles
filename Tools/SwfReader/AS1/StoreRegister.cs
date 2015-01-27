#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class StoreRegister : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.StoreRegister;}}
		internal uint Version {get{return 5;}}	
		internal uint Length{get{return 4;}}
		
		internal uint StackPops { get { return 0; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return 0; } }

		internal uint Register;

		internal StoreRegister(SwfReader r)
		{
			Register = (uint)r.GetByte();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("setregister r:" + Register);
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.StoreRegister);
            w.AppendUI16(Length - 3); // don't incude this part

            w.AppendByte((byte)Register);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("StoreRegister r:" + Register);
        }
#endif
    }
}
