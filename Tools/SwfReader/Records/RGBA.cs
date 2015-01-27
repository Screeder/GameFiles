#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class RGBA : IRecord
	{
		internal byte R;
		internal byte G;
		internal byte B;
		internal byte A;

		internal RGBA(byte r, byte g, byte b, byte a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		internal RGBA(byte r, byte g, byte b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = 0xFF;
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            throw new System.Exception("RGB must be written manually as rgb vs rgba is ambigous");
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("#");
			w.Write(this.R.ToString("X2"));
			w.Write(this.G.ToString("X2"));
			w.Write(this.B.ToString("X2"));
			w.Write(this.A.ToString("X2"));
			w.Write(" ");
		}
#endif
	}
}
