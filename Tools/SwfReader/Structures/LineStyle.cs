#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class LineStyle : ILineStyle
	{
		internal ushort Width;
		internal RGBA Color;

		internal LineStyle(SwfReader r, bool useAlpha)
		{
			this.Width = r.GetUI16();
			if (useAlpha)
			{
				this.Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());
			}
			else
			{
				this.Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte());
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			ToSwf(w, true);
		}
		internal override void ToSwf(SwfWriter w, bool useAlpha)
		{
			w.AppendUI16(this.Width);

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
			w.Write("LS1 ");
			Color.Dump(w);
			w.Write(" w: " + this.Width);
		}
#endif
	}
}
