#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterGlow : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.Glow; } }

		RGBA GlowColor;
		float BlurX;
		float BlurY;
		float Strength;
		bool InnerGlow;
		bool Knockout;
		bool CompositeSource;
		uint Passes;

		internal FilterGlow(SwfReader r)
		{
			GlowColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());

			BlurX = r.GetFixed16_16();
			BlurY = r.GetFixed16_16();
			Strength = r.GetFixed8_8(); 
			
			InnerGlow = r.GetBit();
			Knockout = r.GetBit();
			CompositeSource = r.GetBit();	
			Passes = r.GetBits(5);

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
