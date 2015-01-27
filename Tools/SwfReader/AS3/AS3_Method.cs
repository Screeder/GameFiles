using System;
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    class AS3_MethodInfo : AS3_MemberInfo
    {
        internal uint method_id;
		internal Boolean dumped;
		internal uint flags;
		internal String debugName;
        internal Dictionary<uint, AbstractMultiname> paramTypes;
		internal Array optionalValues;
		//Some SWFs break when TypeName assigned to QName
		//internal var returnType:QName;
        internal AbstractMultiname returnType;
		internal uint local_count;
		internal uint max_scope;
		internal uint max_stack;
		private uint code_length;
        internal AS3_Code code;
        internal AS3_Traits activation;
		internal Dictionary<uint, AS3_Exception> exceptions;
		//Added by SWF Investigator
		internal uint mDefPosition;
		internal uint codePosition;
    }
}
