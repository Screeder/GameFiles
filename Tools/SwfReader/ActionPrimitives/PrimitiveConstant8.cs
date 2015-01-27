#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveConstant8 : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Constant8; } }
        internal object Value { get { return Constant8Value; } }
        internal int Length { get { return 1 + 1; } }
		internal uint Constant8Value;

		internal PrimitiveConstant8(SwfReader r)
		{
			Constant8Value = (uint)r.GetByte();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			if (ActionRecords.CurrentConstantPool != null && ActionRecords.CurrentConstantPool.Constants.Length > Constant8Value)
			{
				string s = ActionRecords.CurrentConstantPool.Constants[Constant8Value];
				w.Write("'" + PrimitiveString.EscapeString(s) + "'");
			}
			else
			{
				w.Write("cp: " + Constant8Value + " ");
			}
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendByte((byte)Constant8Value);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			if (ActionRecords.CurrentConstantPool != null && ActionRecords.CurrentConstantPool.Constants.Length > Constant8Value)
			{
				string s = ActionRecords.CurrentConstantPool.Constants[Constant8Value];
				w.WriteLine("cp_" + Constant8Value  + " \"" +
					PrimitiveString.EscapeString(s) + "\"");
			}
			else
			{
				w.WriteLine("cp: " + Constant8Value + " ");
			}
		}
#endif
    }
}
