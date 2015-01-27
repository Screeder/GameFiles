#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class SolidFill : IFillStyle
	{
		internal RGBA Color;

		internal FillType FillType{get{return FillType.Solid;}}

		internal SolidFill(RGBA c)
		{
			this.Color = c;
		}
		
		internal SolidFill(SwfReader r, bool useAlpha)
		{
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
			w.AppendByte((byte)this.FillType);

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
			w.Write("Solid Fill: ");
			Color.Dump(w);
        }
#endif
    }
}
