#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class EndShapeRecord : IShapeRecord
	{
		/*
			ENDSHAPERECORD
			Field		Type	Comment
			TypeFlag	UB[1]	Non-edge record flag. Always 0.
			EndOfShape	UB[5]	End of shape flag. Always 0.
		 */
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			w.AppendBit(false);
			w.AppendBits(0, 5);
		}
		internal override void ToSwf(SwfWriter w, ref uint fillBits, ref uint lineBits, ShapeType shapeType)
		{
			w.AppendBit(false);
			w.AppendBits(0, 5);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("End Shape");
		}
#endif
    }
}
