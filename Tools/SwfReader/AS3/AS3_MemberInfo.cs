using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AS3_MemberInfo
    {
        internal uint id;
		internal uint kind;
		internal string name;
        internal Dictionary<uint, AS3_Metadata> metadatas;
        internal bool isFinal;
        internal bool isOverride;
        internal bool isPublic;
		internal uint nIndex;
    }
}
