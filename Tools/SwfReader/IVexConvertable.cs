#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class IVexConvertable
    {
#if SWFDUMPER
		internal virtual void Dump(IndentedTextWriter w) {}
#endif
#if SWFWRITER		
        internal virtual void ToSwf(SwfWriter w) {}
#endif
    }
}
