#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveConstant16 : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Constant16; } }
        internal object Value { get { return Constant16Value; } }
        internal int Length { get { return 2 + 1; } }
		internal uint Constant16Value;

		internal PrimitiveConstant16(SwfReader r)
		{
			Constant16Value = r.GetUI16();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			if (ActionRecords.CurrentConstantPool != null && ActionRecords.CurrentConstantPool.Constants.Length > Constant16Value)
			{
				string s = ActionRecords.CurrentConstantPool.Constants[Constant16Value];
				w.Write("'" + PrimitiveString.EscapeString(s) + "'");
			}
			else
			{
				w.Write("cp: " + Constant16Value);
			}
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendUI16(Constant16Value);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			if (ActionRecords.CurrentConstantPool != null && ActionRecords.CurrentConstantPool.Constants.Length > Constant16Value)
			{
				string s = ActionRecords.CurrentConstantPool.Constants[Constant16Value];
				w.WriteLine("cp_" + Constant16Value + " \"" +
					PrimitiveString.EscapeString(s) + "\"");
			}
			else
			{
				w.WriteLine("cp: " + Constant16Value + " ");
			}
		}
#endif
    }
}
