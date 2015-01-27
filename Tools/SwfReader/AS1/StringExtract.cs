#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class StringExtract : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.StringExtract;}}
		internal uint Version {get{return 4;}}
		internal uint Length {get{return 1;}}

		internal SwfType[] Types { get { return new SwfType[] { SwfType.Number, SwfType.Number, SwfType.String, SwfType.String }; } }
		internal uint StackPops { get { return 3; } }
		internal uint StackPushes { get { return 1; } }
		internal int StackChange { get { return -2; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("substring");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.StringExtract);
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
