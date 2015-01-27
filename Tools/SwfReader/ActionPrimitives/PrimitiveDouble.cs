using System;
#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveDouble : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Double; } }
        internal object Value { get { return DoubleValue; } }
        internal int Length { get { return 8 + 1; } }
		internal double DoubleValue;

		internal PrimitiveDouble(SwfReader r)
		{
            byte[] bytes = new byte[8];
            bytes[4] = r.GetByte();
            bytes[5] = r.GetByte();
            bytes[6] = r.GetByte();
            bytes[7] = r.GetByte();

            bytes[0] = r.GetByte();
            bytes[1] = r.GetByte();
            bytes[2] = r.GetByte();
            bytes[3] = r.GetByte();

            DoubleValue = BitConverter.ToDouble(bytes, 0);
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write(DoubleValue);
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            byte[] bytes = BitConverter.GetBytes(DoubleValue);



            w.AppendByte((byte)PrimitiveType);

            w.AppendByte(bytes[4]);
            w.AppendByte(bytes[5]);
            w.AppendByte(bytes[6]);
            w.AppendByte(bytes[7]);

            w.AppendByte(bytes[0]);
            w.AppendByte(bytes[1]);
            w.AppendByte(bytes[2]);
            w.AppendByte(bytes[3]);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(DoubleValue + " ");
		}
#endif
    }
}
