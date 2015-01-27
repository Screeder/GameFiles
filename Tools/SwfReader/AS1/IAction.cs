#if SWFASM
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class IAction: IVexConvertable
	{
#if SWFASM
        internal virtual void ToFlashAsm(IndentedTextWriter w) { }
#endif
	}
}
