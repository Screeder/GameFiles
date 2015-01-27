using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AbstractMultiname
    {
        protected byte Kind;
        internal AbstractMultiname(byte Kind)
        {
            this.Kind = Kind;
        }

    }

    internal class QName : RTQName
    {
        AS3_Namespace Namespace;
        internal QName(AS3_Namespace Namespace, string Name, byte Kind)
            : base(Name, Kind)
        {
            this.Namespace = Namespace;
        }
    }
    internal class RTQName : AbstractMultiname
    {
        internal string Name;
        internal RTQName(string Name, byte Kind)
            : base(Kind)
        {
            this.Name = Name;
        }
    }
    internal class TypeName : AbstractMultiname
    {
        internal AbstractMultiname Name;
        internal Dictionary<int, AbstractMultiname> Types;
        internal TypeName(AbstractMultiname Name, Dictionary<int, AbstractMultiname> Types, byte Kind)
            : base(Kind)
        {
            this.Name = Name;
            this.Types = Types;
        }
    }
    internal class Multiname : AbstractMultiname
    {
        internal string Name;
        internal uint Index;
        internal Multiname(uint Index, string Name, byte Kind)
            : base(Kind)
        {
            this.Name = Name;
            this.Index = Index;
        }
    }
}
