#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class UnsupportedDefinitionTag : ISwfTag
	{
        internal string Message;
        internal UnsupportedDefinitionTag(SwfReader r, uint tagType, string msg)
            : base(tagType)
		{
			this.Message = msg;
        }
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
		{
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("UNSUPPORTED DEFINITION TAG! " + this.Message);
		}
#endif

    }
}
