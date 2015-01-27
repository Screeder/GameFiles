using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AS3_Class
    {
        internal AbstractMultiname classname;
        internal AbstractMultiname superclass;
        internal AS3_Namespace protectedNs;

        internal Dictionary<uint, AbstractMultiname> interfaces = new Dictionary<uint, AbstractMultiname>();
        internal AS3_MethodInfo constructor;
        internal AS3_MethodInfo static_constructor;
        internal AS3_Traits traits;
        internal AS3_Traits static_traits;
        internal uint flags;
        //abc_asset_t* asset;
    }
}
