#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterConvolution : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.Convolution; } }

		uint MatrixX;
		uint MatrixY;
		float Divisor;
		float Bias;
		float[] Matrix;
		RGBA DefaultColor;
		bool Clamp;
		bool PreserveAlpha;

		internal FilterConvolution(SwfReader r)
		{
			MatrixX = (uint)r.GetByte();
			MatrixY = (uint)r.GetByte();
			Divisor = r.GetFloat32();
			Bias = r.GetFloat32();

			uint mxCount = MatrixX * MatrixY;
			Matrix = new float[mxCount];
			for (int i = 0; i < mxCount; i++)
			{
				Matrix[i] = r.GetFloat32();
			}

			DefaultColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());

			r.GetBits(6);
			Clamp = r.GetBit();
			PreserveAlpha = r.GetBit();

			r.Align();
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(this);
        }
#endif
    }
}
