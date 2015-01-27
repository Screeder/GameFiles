#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveNull : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Null; } }
        internal object Value { get { return null; } }
        internal int Length { get { return 0 + 1; } }
		internal PrimitiveNull(SwfReader r)
		{
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write("NULL");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("null ");
		}
#endif
    }
}
