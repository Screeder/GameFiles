using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AS3_Traits
    {
        internal const int TRAIT_SLOT = 0x00;
        internal const int TRAIT_METHOD = 0x01;
        internal const int TRAIT_GETTER = 0x02;
        internal const int TRAIT_SETTER = 0x03;
        internal const int TRAIT_CLASS = 0x04;
        internal const int TRAIT_Function = 0x05;
        internal const int TRAIT_CONST = 0x06;

        internal const int ATTR_FINAL = 0x01; // 1=final, 0=virtual
        internal const int ATTR_OVERRIDE = 0x02; // 1=override, 0=new
        internal const int ATTR_METADATA = 0x04; // 1=has metadata, 0=no metadata
        internal const int ATTR_PUBLIC = 0x08; // 1=add internal namespace

        internal byte kind;
        internal byte attributes;
	    internal AbstractMultiname name;

        internal AS3_MethodInfo constructor;
        internal AS3_Traits traits;
        internal AbstractMultiname baseName;
		
		

        internal Dictionary<uint, AbstractMultiname> interfaces = new Dictionary<uint, AbstractMultiname>();
        internal Dictionary<uint, AbstractMultiname> names = new Dictionary<uint, AbstractMultiname>();
        internal Dictionary<uint, AS3_SlotInfo> slots = new Dictionary<uint, AS3_SlotInfo>();
        internal Dictionary<uint, AS3_MethodInfo> methods = new Dictionary<uint, AS3_MethodInfo>();
        internal Dictionary<uint, AS3_MemberInfo> members = new Dictionary<uint, AS3_MemberInfo>();


        internal void ParseTraits(ByteReader r, DoABCTag doabc)
        {
            uint __num_traits = r.GetUInt30();
            for (uint __i = 0; __i < __num_traits; __i++)
            {
                try
                {
                    uint __nIndex = r.GetUInt30();
                    //var name:* = names[nIndex];
                    uint __tag = r.GetByte();
                    uint __attributes = (__tag >> 4);
                    uint __kind = (__tag & 0x0f);
                    AS3_MemberInfo __member = null;
                    switch (__kind)
                    {
                        case TRAIT_SLOT:
                        case TRAIT_CONST:
                        case TRAIT_CLASS:
                            AS3_SlotInfo __slot = new AS3_SlotInfo();
                            __slot.id = r.GetUInt30();
                            slots[__slot.id] = __slot;
                            if (__kind == TRAIT_SLOT || __kind == TRAIT_CONST)
                            {
                                __slot.type = doabc.Multinames[r.GetUInt30()];
                                uint __index = r.GetUInt30();
                                if (__index > 0)
                                {
                                    //__slot.value = defaults[data.readByte()][index];
                                    r.GetByte();
                                }
                            }
                            else // (kind == TRAIT_CLASS)
                            {
                                //slot.value = classes[readU32()];
                                r.GetUInt30();
                            }
                            __member = __slot;
                            break;
                        case TRAIT_METHOD:
                        case TRAIT_GETTER:
                        case TRAIT_SETTER:
                            uint disp_id = r.GetUInt30();
                            AS3_MethodInfo __method = doabc.MethodInfos[r.GetUInt30()];
                            __method.id = disp_id;
                            methods[disp_id] = __method;
                            __member = __method;
                            break;
                        default:
                            break;
                    }
                    if (__member == null)
                    {
                        //this.log.errorPrint("error trait kind "+kind + "\r");
                    }
                    else
                    {
                        __member.kind = __kind;
                        //member.name = name;
                        __member.nIndex = __nIndex;
                        members[__i] = __member;
                        //t.names[String(name)] = t.members[i] = member;
                        if (__attributes > 0)
                        {
                            if ((__attributes & ATTR_METADATA) != 0)
                            {
                                __member.metadatas = new Dictionary<uint, AS3_Metadata>();
                                uint __metadataCount = r.GetUInt30();
                                for (uint __j = 0; __j < __metadataCount; __j++)
                                {
                                    uint __metaindex = r.GetUInt30();
                                    try
                                    {
                                        __member.metadatas[__j] = doabc.MetaDatas[__metaindex];
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            if ((__attributes & ATTR_FINAL) != 0)
                            {
                                __member.isFinal = true;
                            }
                            if ((__attributes & ATTR_OVERRIDE) != 0)
                            {
                                __member.isOverride = true;
                            }
                            if ((__attributes & ATTR_PUBLIC) != 0)
                            {
                                __member.isPublic = true;
                            }
                        }
                    }

                }
                catch (Exception __e)
                {
                    return;
                }
            }
        }
        internal static void SkipTraits(ByteReader r)
        {
            uint __num_traits = r.GetUInt30();
            for (uint __i = 0; __i < __num_traits; __i++)
            {
                r.GetUInt30();
                uint __kind = r.GetByte();
                uint __attributes = (__kind & 0xf0);
                __kind &= 0x0f;
                r.GetUInt30();
                r.GetUInt30();
                if (__kind == AS3_Traits.TRAIT_SLOT || __kind == AS3_Traits.TRAIT_CONST)
                {
                    if (r.GetUInt30() != 0) r.GetByte();
                }
                else if (__kind > AS3_Traits.TRAIT_CONST)
                {
                    //fprintf(stderr, "Can't parse trait type %d\n", kind);
                }
                if ((__attributes & 0x40) != 0)
                {
                    uint __num_attributes = r.GetUInt30();
                    for (uint __j = 0; __j < __num_attributes; __j++)
                    {
                        r.GetUInt30();
                    }
                }
            }
        }
    }
}
