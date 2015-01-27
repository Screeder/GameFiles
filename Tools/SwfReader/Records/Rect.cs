#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Rect : IRecord
	{
		/*
			RECT
			Field	Type		Comment
			Nbits	UB[5]		Bits in each rect value field
			Xmin	SB[Nbits]	x minimum position for rect
			Xmax	SB[Nbits]	x maximum position for rect
			Ymin	SB[Nbits]	y minimum position for rect
			Ymax	SB[Nbits]	y maximum position for rect
		*/
		internal static readonly Rect Empty = new Rect(0, 0, 0, 0);

		internal int XMin;
		internal int XMax;
		internal int YMin;
		internal int YMax;

		internal Rect(SwfReader r)
		{
            r.Align();
			byte minBits = (byte)r.GetBits(5);
			XMin = r.GetSignedNBits(minBits);
			XMax = r.GetSignedNBits(minBits);
			YMin = r.GetSignedNBits(minBits);
			YMax = r.GetSignedNBits(minBits);
			r.Align();
		}

        internal int Width
        {
            get { return XMax - XMin; }
        }
        internal int Height
        {
            get { return YMax - YMin; }
        }
        
		internal Rect(int xMin, int xMax, int yMin, int yMax)
		{
			this.XMin = xMin;
			this.XMax = xMax;
			this.YMin = yMin;
			this.YMax = yMax;
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			w.Align();

			uint bits = SwfWriter.MinimumBits(this.XMax, this.XMin, this.YMax, this.YMin);

			w.AppendBits((uint)bits, 5);
			w.AppendBits((uint)(this.XMin), bits);
			w.AppendBits((uint)(this.XMax), bits);
			w.AppendBits((uint)(this.YMin), bits);
			w.AppendBits((uint)(this.YMax), bits);

			w.Align();
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("Rect [");
			w.Write("xMin: " + this.XMin + ", ");
			w.Write("xMax: " + this.XMax + ", ");
			w.Write("yMin: " + this.YMin + ", ");
			w.Write("yMax: " + this.YMax);
			w.Write("]");
		}
#endif
	}
}

