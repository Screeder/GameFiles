#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveFloat : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Float; } }
        internal object Value { get { return FloatValue; } }
        internal int Length { get { return 4 + 1; } }
		internal float FloatValue;

		internal PrimitiveFloat(SwfReader r)
		{
			FloatValue = r.GetFixedNBits(32);
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write(FloatValue);
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendFixedNBits(FloatValue, 32);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(FloatValue + " ");
		}
#endif
    }
}
