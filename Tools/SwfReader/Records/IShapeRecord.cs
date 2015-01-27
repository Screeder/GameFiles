namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class IShapeRecord : IRecord
	{
#if SWFWRITER
        internal virtual void ToSwf(SwfWriter w, ref uint fillBits, ref uint lineBits, ShapeType shapeType) {}
#endif
	}
}
