#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class JPEGTables : ISwfTag
	{
        internal byte[] JpegTable;

        internal JPEGTables(SwfReader r, uint curTagLen)
            : base(TagType.JPEGTables)
		{
			if(curTagLen > 8)
			{
				r.GetBytes(2); //jpg SOI Marker
				JpegTable = r.GetBytes(curTagLen - 4);
				r.GetBytes(2); //jpg EOI Marker
			}
            else
            {
                JpegTable = new byte[0];
            }
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            if(JpegTable.Length > 0)
            {
                    w.AppendByte(0xFF);
                    w.AppendByte(0xD8);

                    w.AppendBytes(JpegTable);

                    w.AppendByte(0xFF);
                    w.AppendByte(0xD9);
            }

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("JPEGTables: ");
			w.WriteLine();
        }
#endif
    }
}
