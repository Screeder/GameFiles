using System.Collections.Generic;
#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif

namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class ExportAssetsTag : ISwfTag
	{
		internal Dictionary<uint, string> Exports = new Dictionary<uint, string>();

        internal ExportAssetsTag(SwfReader r)
            : base(TagType.ExportAssets)
		{
			uint count = r.GetUI16();
			for (int i = 0; i < count; i++)
			{
				uint index = r.GetUI16();
				string name = r.GetString();
				Exports.Add(index, name);
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true);

			w.AppendUI16((uint)Exports.Count);
			foreach (uint index in Exports.Keys)
			{
				w.AppendUI16(index);
				w.AppendString(Exports[index]);				
			}

			w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Export Assets: ");
			w.Indent++;
			foreach (uint key in Exports.Keys)
			{
				w.WriteLine(key + " : " + Exports[key]);
			}
			w.Indent--;
        }
#endif
    }
}
