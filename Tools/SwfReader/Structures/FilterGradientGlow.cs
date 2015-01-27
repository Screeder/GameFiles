#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterGradientGlow : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.GradientGlow; } }

		uint NumColors;
		RGBA[] GradientColors;
		uint[] GradientRatio;
		float BlurX;
		float BlurY;
		float Angle;
		float Distance;
		float Strength;
		bool InnerShadow;
		bool Knockout;
		bool CompositeSource;
		bool OnTop;
		uint Passes;

		internal FilterGradientGlow(SwfReader r)
		{
			NumColors = (uint)r.GetByte();
			GradientColors = new RGBA[NumColors];
			for (int i = 0; i < NumColors; i++)
			{
				GradientColors[i] = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());
			}

			GradientRatio = new uint[NumColors];
			for (int i = 0; i < NumColors; i++)
			{
				GradientRatio[i] = (uint)r.GetByte();
			}

			BlurX = r.GetFixed16_16();
			BlurY = r.GetFixed16_16();
			Angle = r.GetFixed16_16();
			Distance = r.GetFixed16_16();
			Strength = r.GetFixed8_8(); 
			
			InnerShadow = r.GetBit();
			Knockout = r.GetBit();
			CompositeSource = r.GetBit();
			OnTop = r.GetBit();
			Passes = r.GetBits(4);

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
