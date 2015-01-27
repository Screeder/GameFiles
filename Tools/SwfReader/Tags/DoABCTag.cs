using System;
using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    class DoABCTag : ISwfTag
    {
        //internal uint id;
        internal Dictionary<uint, int> Integers = new Dictionary<uint, int>();
        internal Dictionary<uint, uint> UIntegers = new Dictionary<uint, uint>();
        internal Dictionary<uint, double> Doubles = new Dictionary<uint, double>();
        internal Dictionary<uint, string> Strings = new Dictionary<uint, string>();
        internal Dictionary<uint, AS3_Namespace> Namespaces = new Dictionary<uint, AS3_Namespace>();
        internal Dictionary<uint, Dictionary<uint, AS3_Namespace>> NamespaceSets = new Dictionary<uint, Dictionary<uint, AS3_Namespace>>();
        internal Dictionary<uint, AbstractMultiname> Multinames = new Dictionary<uint, AbstractMultiname>();
        internal Dictionary<uint, AS3_MethodInfo> MethodInfos = new Dictionary<uint, AS3_MethodInfo>();
        internal Dictionary<uint, AS3_Metadata> MetaDatas = new Dictionary<uint, AS3_Metadata>();
        //internal Dictionary<uint, DoABC_Traits> Instances = new Dictionary<uint, DoABC_Traits>();
        internal Dictionary<uint, AS3_Class> Classes = new Dictionary<uint, AS3_Class>();
        internal Dictionary<uint, DoABC_Script> Scripts = new Dictionary<uint, DoABC_Script>();

        //private string name;
        private ByteReader _data;
        uint magic;
        internal uint flag;
        private string name;

        internal const int ABCFLAG_LAZY = 0x01;
        internal const int CONSTANT_Utf8 = 0x01;
        internal const int CONSTANT_Int = 0x03;
        internal const int CONSTANT_UInt = 0x04;
        internal const int CONSTANT_PrivateNs = 0x05; // non-shared namespace
        internal const int CONSTANT_Double = 0x06;
        internal const int CONSTANT_Qname = 0x07; // o.ns::name, ct ns, ct name
        internal const int CONSTANT_Namespace = 0x08;
        internal const int CONSTANT_Multiname = 0x09; // o.name, ct nsset, ct name
        internal const int CONSTANT_False = 0x0A;
        internal const int CONSTANT_True = 0x0B;
        internal const int CONSTANT_Null = 0x0C;
        internal const int CONSTANT_QnameA = 0x0D; // o.@ns::name, ct ns, ct attr-name
        internal const int CONSTANT_MultinameA = 0x0E; // o.@name, ct attr-name
        internal const int CONSTANT_RTQname = 0x0F; // o.ns::name, rt ns, ct name
        internal const int CONSTANT_RTQnameA = 0x10; // o.@ns::name, rt ns, ct attr-name
        internal const int CONSTANT_RTQnameL = 0x11; // o.ns::[name], rt ns, rt name
        internal const int CONSTANT_RTQnameLA = 0x12; // o.@ns::[name], rt ns, rt attr-name
        internal const int CONSTANT_NameL = 0x13;	// o.[], ns=internal implied, rt name
        internal const int CONSTANT_NameLA = 0x14; // o.@[], ns=internal implied, rt attr-name
        internal const int CONSTANT_NamespaceSet = 0x15;
        internal const int CONSTANT_PackageNs = 0x16;
        internal const int CONSTANT_PackageInternalNs = 0x17;
        internal const int CONSTANT_ProtectedNs = 0x18;
        internal const int CONSTANT_StaticProtectedNs = 0x19;
        internal const int CONSTANT_StaticProtectedNs2 = 0x1a;
        internal const int CONSTANT_MultinameL = 0x1B;
        internal const int CONSTANT_MultinameLA = 0x1C;
        internal const int CONSTANT_TypeName = 0x1D;
        internal const int NEED_ARGUMENTS = 0x01;
        internal const int NEED_ACTIVATION = 0x02;
        internal const int NEED_REST = 0x04;
        internal const int HAS_OPTIONAL = 0x08;
        internal const int IGNORE_REST = 0x10;
        internal const int NATIVE = 0x20;
        internal const int HAS_ParamNames = 0x80;

        internal DoABCTag(ByteReader r, uint tagLen, bool isDoABC2)
            : base(TagType.DoABC)
        {
            _data = new ByteReader(r.GetBytes(tagLen));
            if (isDoABC2)
            {
                this.tagType = TagType.DoABC2;
                uint __startPos = r.Position;
                flag = _data.GetUI32(); //consume flag mode
                name = _data.GetString(); //consume name
            }
            //Extract(_data);
        }
        internal DoABCTag(ByteReader r, uint tagLen)
            : base(TagType.DoABC)
        {
            _data = new ByteReader(r.GetBytes(tagLen));
            //Extract(_data);
        }

        internal void Extract()
        {
            Extract(_data);
        }

        internal void Extract(ByteReader r)
        {
            magic = r.GetUI32();
            switch (magic) //3014672 - 0x002e0010
            {
                case (46 << 16 | 14):
                case (46 << 16 | 15):
                case (46 << 16 | 16):
                case (47 << 16 | 12):
                case (47 << 16 | 13):
                case (47 << 16 | 14):
                case (47 << 16 | 15):
                case (47 << 16 | 16):
                case (47 << 16 | 17):
                case (47 << 16 | 18):
                case (47 << 16 | 19):
                case (47 << 16 | 20):
                    break;
                default:
                    Console.WriteLine("not an abc file.  magic=" + magic.ToString("X2"));
                    return;
            }
            //switch a byte ???
            //if (r.PeekByte() == 2)
            //{
                //byte __startByte = r.GetByte();
            //}
            //Console.WriteLine("!!! " + r.LeftBytes + " BYTES !!! ");
            // ints
            uint __n = r.GetUInt30();
            Integers.Clear();
            if (__n > 0) { Integers[0] = 0; }
            //Console.Write("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " int : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    Integers[__i] = r.GetInt30();
                    //Console.Write(" " + Integers[__i] + " ");
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            // uints
            __n = r.GetUInt30();
            UIntegers.Clear();
            if (__n > 0) { UIntegers[0] = 0; }
            //Console.Write("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " uint : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    UIntegers[__i] = r.GetUInt30();
                    //Console.Write(" " + UIntegers[__i] + " ");
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            // doubles
            __n = r.GetUInt30();
            Doubles.Clear();
            if (__n > 0) { Doubles[0] = 0; }
            //Console.Write("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " double : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    Doubles[__i] = r.GetDouble();
                   // Console.Write(" " + Doubles[__i] + " ");
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            // strings

            __n = r.GetUInt30();
            Strings.Clear();
            if (__n > 0) { Strings[0] = ""; }
            string[] strings = new string[__n];
            //Console.Write("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " string : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    Strings[__i] = r.GetUTF8Bytes(r.GetUInt30());
                    //Console.Write(" \"" + Strings[__i] + "\" ");
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            // namespaces
            __n = r.GetUInt30();
            Namespaces.Clear();
            if (__n > 0) { Namespaces[0] = new AS3_Namespace(); }
            //Console.Write("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " namespace : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    byte __namespace_type = r.GetByte();
                    switch (__namespace_type)
                    {
                        case CONSTANT_Namespace:
                        case CONSTANT_PackageNs:
                        case CONSTANT_PackageInternalNs:
                        case CONSTANT_ProtectedNs:
                        case CONSTANT_StaticProtectedNs:
                        case CONSTANT_StaticProtectedNs2:
                        case CONSTANT_PrivateNs:
                            string __namespace = Strings[r.GetUInt30()];
                            Namespaces[__i] = new AS3_Namespace(__namespace, __namespace_type);
                            //Console.Write(" " + Namespaces[__i] + " ");
                            break;
                        case 210:
                            uint __rere = r.GetUInt30();
                            string __namespace2 = Strings[__rere];
                            //Namespaces[__i] = "private";
                            Namespaces[__i] = new AS3_Namespace("private", __namespace_type);
                            //Console.Write(" private ");
                            break;
                        default:
                            //Console.WriteLine("not Namespaces type =" + __namespace_type.ToString("X2"));
                            break;
                    }
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");


            // namespace sets
            __n = r.GetUInt30();
            NamespaceSets.Clear();
            if (__n > 0) { NamespaceSets[0] = new Dictionary<uint, AS3_Namespace>(); }
            //Console.WriteLine("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " sets namespace : [");
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    Dictionary<uint, AS3_Namespace> __namespaceSet = new Dictionary<uint, AS3_Namespace>();
                    uint __nnset_count = r.GetUInt30();
                    //Console.Write(" Pool of #" + __i + " set namespace : [");
                    for (uint __j = 0; __j < __nnset_count; __j++)
                    {
                        uint __index = r.GetUInt30();
                        if (Namespaces.Count > __index)
                        {
                            __namespaceSet[__j] = Namespaces[__index];
                            //Console.Write(" \"" + Namespaces[__index] + "\" ");
                        }
                        else
                        {
                        }
                    }
                    NamespaceSets[__i] = __namespaceSet;
                    //Console.WriteLine(" ]");
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            // multinames
            Multinames.Clear();
            __n = r.GetUInt30();
            //Console.WriteLine("Pool of " + (__n > 0 ? (__n - 1).ToString() : "0") + " Multiname : [");
            if (__n > 0) { Multinames[0] = new Multiname(0, "*", 0); }
            for (uint __i = 1; __i < __n; __i++)
            {
                try
                {
                    uint __index1;
                    uint __index2;
                    byte __kind = r.GetByte();
                    switch (__kind)
                    {
                        case CONSTANT_Qname:
                        case CONSTANT_QnameA:
                            __index1 = r.GetUInt30();
                            __index2 = r.GetUInt30();
                            Multinames[__i] = new QName(Namespaces[__index1], Strings[__index2], __kind);
                            break;

                        case CONSTANT_RTQname:
                        case CONSTANT_RTQnameA:
                            __index1 = r.GetUInt30();
                            Multinames[__i] = new RTQName(Strings[__index1], __kind);
                            break;

                        case CONSTANT_RTQnameL:
                        case CONSTANT_RTQnameLA:
                            Multinames[__i] = new AbstractMultiname(__kind);
                            break;

                        case CONSTANT_NameL:
                        case CONSTANT_NameLA:
                            Multinames[__i] = new QName(new AS3_Namespace(), null, __kind);
                            break;

                        case CONSTANT_Multiname:
                        case CONSTANT_MultinameA:
                            __index1 = r.GetUInt30();
                            __index2 = r.GetUInt30();
                            string __nameMultiname = Strings[__index1];
                            Multinames[__i] = new Multiname(__index2, __nameMultiname, __kind);
                            break;

                        case CONSTANT_MultinameL:
                        case CONSTANT_MultinameLA:
                            __index1 = r.GetUInt30();
                            Multinames[__i] = new Multiname(__index1, null, __kind);
                            break;

                        case CONSTANT_TypeName:
                            __index1 = r.GetUInt30();
                            uint __TypeNameCount = r.GetUInt30();
                            AbstractMultiname __nameTypeName = Multinames[__index1];
                            Dictionary<int, AbstractMultiname> __types = new Dictionary<int, AbstractMultiname>();
                            for (int __j = 0; __j < __TypeNameCount; __j++)
                            {
                                __index2 = r.GetUInt30();
                                __types[__j] = Multinames[__index2];
                            }
                            Multinames[__i] = new TypeName(__nameTypeName, __types, __kind);
                            break;
                        default:
                            Console.Write("invalid kind " + __kind);
                            break;
                    }
                }
                catch
                {
                }
            }
            //Console.WriteLine("]");

            //--------------Method-----------------------
            try
            {
                __n = r.GetUInt30();
                MethodInfos.Clear();
                //Console.WriteLine("Pool of " + (__n) + " Method : [");
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        AS3_MethodInfo m = new AS3_MethodInfo();
                        m.method_id = __i;
                        uint __param_count = r.GetUInt30();
                        uint __return_type_index = r.GetUInt30();
                        m.returnType = Multinames[__return_type_index];
                        m.paramTypes = new Dictionary<uint, AbstractMultiname>();
                        for (uint __j = 0; __j < __param_count; __j++)
                        {
                            uint __type_index = r.GetUInt30();
                            m.paramTypes[__j] = Multinames[__type_index];
                        }
                        m.debugName = Strings[r.GetUInt30()];
                        m.flags = r.GetByte();
                        if ((m.flags & HAS_OPTIONAL) != 0)
                        {
                            uint __optional_count = r.GetUInt30();
                            for (uint __k = 0; __k < __optional_count; __k++)
                            {
                                uint __param_index = r.GetUInt30();    // optional value index
                                uint __param_kind = r.GetByte();    // kind byte for each default value
                                if (__param_index == 0)
                                {
                                    // kind is ignored, default value is based on type
                                    //m.optionalValues[k] = undefined;
                                }
                                else
                                {
                                    /*
                                        if (!defaults[kind])
                                        this.log.errorPrint("ERROR kind="+kind+" method_id " + i + "\r");
                                    else
                                        m.optionalValues[k] = defaults[kind][index];
                                     */
                                }
                            }

                        }
                        if ((m.flags & HAS_ParamNames) != 0)
                        {
                            // has_paramnames
                            for (int __k = 0; __k < __param_count; __k++)
                            {
                                r.GetUInt30();
                            }
                        }
                        MethodInfos[__i] = m;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            //parseMetadataInfos
            MetaDatas.Clear();
            try
            {
                __n = r.GetUInt30();
                //Console.WriteLine("Pool of " + (__n) + " metadata ");
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        AS3_Metadata __Metadata = new AS3_Metadata();
                        __Metadata.name = Strings[r.GetUInt30()];
                        uint __values_count = r.GetUInt30();
                        __Metadata.values = new Dictionary<string, string>();
                        /*
                        Dictionary<uint, string> __metadatanames = new Dictionary<uint, string>();
                        for (uint __j = 0; __j < __values_count; __j++)
                        {
                            __metadatanames.Add(__j, Strings[r.GetUInt30()]);
                        }
                        */
                        for (uint __j = 0; __j < __values_count; __j++)
                        {
                            __Metadata.values.Add(Strings[r.GetUInt30()], Strings[r.GetUInt30()]);
                            //__Metadata.values.Add(__metadatanames[__j], Strings[r.GetUInt30()]);
                        }
                        MetaDatas[__i] = __Metadata;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            //parseClassInfos  FAKE
            try
            {
                Classes.Clear();
                __n = r.GetUInt30();
                //Console.WriteLine("Pool of " + (__n) + " Class info ");
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        AS3_Class _class = new AS3_Class();
                        _class.classname = Multinames[r.GetUInt30()];//classname
                        _class.superclass = Multinames[r.GetUInt30()];//supername superclass
                        _class.flags = r.GetByte();
                        if ((_class.flags & 8) != 0)
                        {
                            _class.protectedNs = Namespaces[r.GetUInt30()];//protectedNS
                        }
                        uint __interface_count = r.GetUInt30();  //interface count
                        for (uint __k = 0; __k < __interface_count; __k++)
                        {
                            uint __interface_index = r.GetUInt30();
                        }
                        uint __interface_init = r.GetUInt30(); //iinit
                        _class.constructor = MethodInfos[__interface_init];
                        _class.traits = new AS3_Traits();
                        _class.traits.ParseTraits(r, this);
                        Classes[__i] = _class;
                    }
                    catch
                    {
                    }
                }
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        uint __class_init = r.GetUInt30(); //cinit
                        Classes[__i].static_traits = new AS3_Traits();
                        Classes[__i].static_traits.ParseTraits(r, this);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            //parseScriptInfos
            try
            {
                Scripts.Clear();
                __n = r.GetUInt30();
                //Console.WriteLine("Pool of " + (__n) + " Script info ");
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        DoABC_Script __script = new DoABC_Script();
                        __script.constructor = MethodInfos[r.GetUInt30()];
                        __script.traits = new AS3_Traits();
                        __script.traits.ParseTraits(r, this);
                        /*
                        DoABC_Traits t = new DoABC_Traits();
                        t.name = "script" + __i;
                        t.baseName = Multinames[0]; // Object
                        t.init = MethodInfos[r.GetUInt30()];
                        t.init.name = t.name + "$init";
                        t.init.kind = TRAIT_Method;
                         * */
                        //_ParseTraits(r, t);
                        
                        //DoABC_Traits.SkipTraits(r);
                        Scripts[__i] = __script;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            //return;
            //parseMethodBodies
            try
            {
                __n = r.GetUInt30();
                //Console.WriteLine("Pool of " + (__n) + " MethodBodies ");
                for (uint __i = 0; __i < __n; __i++)
                {
                    try
                    {
                        AS3_MethodInfo m = MethodInfos[r.GetUInt30()];
                        m.max_stack = r.GetUInt30();
                        m.local_count = r.GetUInt30();
                        uint initScopeDepth = r.GetUInt30();
                        uint maxScopeDepth = r.GetUInt30();
				        m.max_scope = maxScopeDepth - initScopeDepth;
                        uint code_length = r.GetUInt30();
                        if (code_length > 0)
                        {
                            m.code = new AS3_Code(r.GetBytes(code_length));
                            
                            m.code.ParseCode(this);
                        }
                        m.exceptions = new Dictionary<uint, AS3_Exception>();
                        uint exception_count = r.GetUInt30();
                        for (uint __j = 0; __j < exception_count; __j++)
                        {
                            AS3_Exception __exception = new AS3_Exception();
                            __exception.from = r.GetUInt30();
                            __exception.to = r.GetUInt30();
                            __exception.target = r.GetUInt30();
                            __exception.var_type = Multinames[r.GetUInt30()];   //var exc_type = multiname_clone(pool_lookup_multiname(pool, swf_GetU30(tag)));
                            __exception.var_name = Multinames[r.GetUInt30()];   //var name:* = names[readU32()];
                            m.exceptions.Add(__j, __exception);
				        }
                        m.activation = new AS3_Traits();
                        m.activation.ParseTraits(r, this);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            int __leftBytes = r.LeftBytes;
            /*
            if (__leftBytes > 0)
            {
                Console.WriteLine("!!! " + __leftBytes + " BYTES LEFT !!! ");
            }
            else if (__leftBytes == 0)
            {
                Console.WriteLine("!!! DONE !!! ");
            }
            else
            {
                Console.WriteLine("!!! ERROR !!! " + __leftBytes + " BYTES MIN !!! ");
            }
             */ 
        }


#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
        {
            return; // NOT YET HANDLED
        }
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
        {
            string __dump;
            if (this.tagType == TagType.DoABC2)
            {
                __dump = "DOABC2 \"" + name + "\"" + (flag == ABCFLAG_LAZY ? ", lazy load" : "");
            }
            else
            {
                __dump = "DOABC";
            }
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, _data.Size, __dump));
        }
        public override string ToString()
        {
            string __returnString = "";
            if (this.tagType == TagType.DoABC2)
            {
                __returnString += "DoABC2 Tag: [" + Environment.NewLine;
                __returnString += "\t" + "name : " + name + Environment.NewLine;
                __returnString += "\t" + "flag : " + flag + Environment.NewLine;
            }
            else
            {
                __returnString += "DoABC Tag: [" + Environment.NewLine;
            }
            if (Integers.Count > 0)
            {
                __returnString += "\t" + "Pool of " + (Integers.Count - 1) + " int : [";
                for (uint __i = 1; __i < Integers.Count; __i++)
                {
                    __returnString += " " + Integers[__i] + " ";
                }
                __returnString += "]" + Environment.NewLine;
            }
            else
            {
                __returnString += "\t" + "Pool of 0 int : []" + Environment.NewLine;
            }


            __returnString += "]" + Environment.NewLine;
            return __returnString;
        }
#endif
    }
}
