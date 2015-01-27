#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class SetTarget : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.SetTarget;}}
		internal uint Version {get{return 3;}}		
		internal string TargetName;
		internal uint Length
		{
			get
			{
                return (uint)(3 + TargetName.Length + 1); 
			}
		}

		internal SetTarget(SwfReader r)
		{
			TargetName = r.GetString();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("settarget");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.SetTarget);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendString(TargetName);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("SetTarget: " + TargetName);
        }
#endif
    }
}
