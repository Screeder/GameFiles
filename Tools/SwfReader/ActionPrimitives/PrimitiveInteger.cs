#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveInteger : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Integer; } }
        internal object Value { get { return IntegerValue; } }
        internal int Length { get { return 4 + 1; } }
		internal int IntegerValue;

		internal PrimitiveInteger(SwfReader r)
		{
			IntegerValue = r.GetInt32();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write(IntegerValue);
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendInt32(IntegerValue);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(IntegerValue + " ");
		}
#endif
    }
}
