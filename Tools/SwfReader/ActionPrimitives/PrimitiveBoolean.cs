#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveBoolean : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Boolean; } }
		internal object Value { get { return BooleanValue; } }
        internal int Length { get { return 1 + 1; } }
		internal bool BooleanValue;

		internal PrimitiveBoolean(SwfReader r)
		{
			BooleanValue = r.GetByte() > 0 ? true : false; // stored as byte
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write(BooleanValue.ToString().ToUpper());
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)PrimitiveType);
            byte b = (BooleanValue == true) ? (byte)0x01 : (byte)0x00;
            w.AppendByte(b);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(BooleanValue + " ");
		}
#endif
    }
}
