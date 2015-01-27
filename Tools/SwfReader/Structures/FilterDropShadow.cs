#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterDropShadow : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.DropShadow; } }

		RGBA DropShadowColor;
		float BlurX;
		float BlurY;
		float Angle;
		float Distance;
		float Strength;
		bool InnerShadow;
		bool Knockout;
		bool CompositeSource;
		uint Passes;

		internal FilterDropShadow(SwfReader r)
		{
			DropShadowColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());

			BlurX = r.GetFixed16_16();
			BlurY = r.GetFixed16_16();
			Angle = r.GetFixed16_16();
			Distance = r.GetFixed16_16();
			Strength = r.GetFixed8_8(); 
			
			InnerShadow = r.GetBit();
			Knockout = r.GetBit();
			CompositeSource = r.GetBit();	
			Passes = (uint)r.GetBits(5);

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
