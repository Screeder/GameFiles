#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GoToLabel : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.GoToLabel;}}
		internal uint Version {get{return 3;}}
		
		internal string Label;
        internal uint Length { get { return (uint)(3 + Label.Length + 1); } }

		internal GoToLabel(SwfReader r)
		{
			Label = r.GetString();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("gotolabel");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.GoToLabel);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendString(Label);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Goto: " + this.Label);
        }
#endif
    }
}
