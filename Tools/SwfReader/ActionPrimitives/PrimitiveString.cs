#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveString : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.String; } }
        internal object Value { get { return StringValue; } }
        internal int Length { get { return StringValue.Length + 2; } }
		internal string StringValue;

		internal PrimitiveString(SwfReader r)
		{
			StringValue = r.GetString();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write("'" + EscapeString(StringValue) + "'");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendString(StringValue);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(StringValue + " ");
		}

		internal static string EscapeString(string s)
		{
			if(s.IndexOf('\n') > -1)
			{
				s = s + "";
			}
			s = s.Replace("\n", "\\n");
			s = s.Replace("\r", "\\r");
			s = s.Replace("\t", "\\t");
			return s;
		}
#endif
    }
}
