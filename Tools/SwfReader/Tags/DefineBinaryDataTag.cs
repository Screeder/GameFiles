#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    class DefineBinaryDataTag : ISwfTag
    {
        internal uint id;
        internal byte[] data;
        private ByteReader _data;
        int reserved;

        internal DefineBinaryDataTag(SwfReader r, uint tagLen)
            : base(TagType.DefineBinaryData)
		{
            _data = new ByteReader(r.GetBytes(tagLen));
            id = _data.GetUI16();
            reserved = _data.GetInt32();
            data = _data.GetBytes(tagLen - 6);
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
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, _data.Size, "DEFINEBINARY defines id " + id.ToString("0000")));
        }
#endif
    }
}
