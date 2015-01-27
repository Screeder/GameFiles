using System;
using System.CodeDom.Compiler;

namespace LeagueSharp.GameFiles.Tools.Swf
{
    class MetadataTag : ISwfTag
    {
        internal string xml;
        private uint size;
        internal MetadataTag(SwfReader r):base(TagType.Metadata)
		{
            uint __pos = r.Position;
            xml = r.GetString();
            size = r.Position - __pos;
		}
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
        {
            return; // NOT YET HANDLED
        }
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
        {
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, size, "METADATA {" + xml + "}"));
        }
#endif
    }
}
