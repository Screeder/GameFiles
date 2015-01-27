namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ILineStyle : IVexConvertable
    {
#if SWFWRITER
        internal virtual void ToSwf(SwfWriter w, bool useAlpha) { }
#endif
    }
}
