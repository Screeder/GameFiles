namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ISwfTag : IVexConvertable
	{
        protected uint tagType;
        internal uint Type { get { return tagType; } }
        internal ISwfTag(uint tagType)
        {
            this.tagType = tagType;
        }
	}
}
