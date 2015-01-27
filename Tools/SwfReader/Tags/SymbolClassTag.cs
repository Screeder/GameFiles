using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    class SymbolClassTag : ISwfTag
    {
        internal uint TagLen = 0;

        internal Dictionary<string, uint> Symbols = new Dictionary<string, uint>();
		internal string topLevelClass;
		
		//Added by SWF Investigator
        internal object class2idref = new object();


        internal SymbolClassTag(SwfReader r, uint tagLen)
            : base(TagType.SymbolClass)
		{
            TagLen = tagLen;
            Symbols.Clear();
            UInt16 __count = r.GetUI16();
            for (UInt16 __i = 0; __i < __count; __i++)
			{
                uint __idref = r.GetUI16();
				String __name = r.GetString();
                if (__idref == 0)
                {
                    this.topLevelClass = __name;
                }
                if (!Symbols.ContainsKey(__name))
                {
                    Symbols.Add(__name, __idref);
                }
            }
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
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, TagLen, "SYMBOLCLASS"));
            foreach(KeyValuePair<string, uint> __symbol in Symbols)
            {
                w.WriteLine("                exports " + __symbol.Value.ToString("0000") + " as \"" + __symbol.Key + "\"");
            }
        }
#endif
    }
}
