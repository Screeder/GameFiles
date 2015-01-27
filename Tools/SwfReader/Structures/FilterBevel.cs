#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterBevel : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.Bevel; } }

		RGBA ShadowColor;
		RGBA HighlightColor;
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

		internal FilterBevel(SwfReader r)
		{
			ShadowColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());
			HighlightColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());

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
