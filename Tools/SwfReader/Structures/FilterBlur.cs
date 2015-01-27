#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterBlur : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.Blur; } }

		float BlurX;
		float BlurY;
		uint Passes;

		internal FilterBlur(SwfReader r)
		{
			BlurX = r.GetFixed16_16();
			BlurY = r.GetFixed16_16();
			Passes = r.GetBits(5);
			r.GetBits(3); // reserved
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
