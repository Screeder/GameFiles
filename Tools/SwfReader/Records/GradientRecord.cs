#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GradientRecord : IRecord
	{
		internal byte Ratio;
		internal RGBA Color;
		internal GradientRecord(SwfReader r, bool useAlpha)
		{
			Ratio = r.GetByte();
			if (useAlpha)
			{
				Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());
			}
			else
			{
				Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte());
			}
		}
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
		{
			throw new System.NotSupportedException("use overload that specifies alpha");
		}
		internal void ToSwf(SwfWriter w, bool useAlpha)
		{
			w.AppendByte(Ratio);
			w.AppendByte(Color.R);
			w.AppendByte(Color.G);
			w.AppendByte(Color.B);
			if (useAlpha)
			{
				w.AppendByte(Color.A);
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("[");
			w.Write("r:" + Ratio.ToString("X2") + " c:");
			Color.Dump(w);
			w.Write("]");
        }
#endif
    }
}
