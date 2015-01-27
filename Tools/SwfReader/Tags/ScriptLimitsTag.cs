using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    class ScriptLimitsTag : ISwfTag
    {
        Int16 ScriptRecursionLimit;
        Int16 ScriptTimeLimit;
        internal ScriptLimitsTag(SwfReader r)
            : base(TagType.ScriptLimits)
		{
            ScriptRecursionLimit = r.GetInt16();
            ScriptTimeLimit = r.GetInt16();
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
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, 4, "SCRIPTLIMITS { ScriptRecursionLimit: " + ScriptRecursionLimit.ToString() + ", ScriptTimeLimit: " + ScriptTimeLimit.ToString() + " }"));
        }
#endif
    }
}
