using System;
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AS3_Code
    {
        internal AS3_Code()
        {
            this._byteReader = new ByteReader();
        }
        internal AS3_Code(byte[] code)
        {
            this._byteReader = new ByteReader(code);
        }
        internal AS3_Code(ByteReader r)
        {
            this._byteReader = r;
            _byteReader.Reset();
        }
        private ByteReader _byteReader;
        internal List<OpCode> Codes = new List<OpCode>();
        internal class OpCode
        {
            internal byte opcode;
            internal string name;
            internal string parameters;
            internal int stack_minus;
            internal int stack_plus;
            internal int scope_stack_plus;
            internal uint flags;
            internal object[] datas;
            internal OpCode(byte opcode, string name, string parameters, int stack_minus, int stack_plus, int scope_stack_plus, uint flags)
            {
                this.opcode = opcode;
                this.name = name;
                this.parameters = parameters;
                this.stack_minus = stack_minus;
                this.stack_plus = stack_plus;
                this.scope_stack_plus = scope_stack_plus;
                this.flags = flags;
            }
            internal OpCode Clone()
            {
                return new OpCode(opcode, name, parameters, stack_minus, stack_plus, scope_stack_plus, flags);
            }
        }

        static class OpCodes
        {
            const uint OP_REGISTER = 1;
            const uint OP_STACK_ARGS = 2;
            const uint OP_STACK_NS = 4;
            const uint OP_SET_DXNS = 8;
            const uint OP_RETURN = 16;
            const uint OP_THROW = 32;
            const uint OP_BRANCH = 64;
            const uint OP_JUMP = 128;
            const uint OP_LABEL = 256;
            const uint OP_LOOKUPSWITCH = 512;
            const uint OP_NEED_ACTIVATION = 1024;
            const uint OP_STACK_ARGS2 = 2048;
            const uint OP_INTERNAL = 32768;
            static Dictionary<byte, OpCode> _opcodes = new Dictionary<byte, OpCode>();
            static internal bool Exists(byte opcode)
            {
                _SetOpCodes();
                return (_opcodes.ContainsKey(opcode));
            }
            static internal OpCode GetOpCode(byte opcode)
            {
                if (Exists(opcode))
                {
                    return _opcodes[opcode].Clone();
                }
                return null;
            }
            static private void _SetOpCodes()
            {
                if (_opcodes.Count == 0)
                {
                    _Add(0xa0, "add", "", -2, 1, 0, 0);
                    _Add(0xc5, "add_i", "", -2, 1, 0, 0);
                    _Add(0x86, "astype", "2", -1, 1, 0, 0);
                    _Add(0x87, "astypelate", "", -2, 1, 0, 0);
                    _Add(0xA8, "bitand", "", -2, 1, 0, 0);
                    _Add(0x97, "bitnot", "", -1, 1, 0, 0);
                    _Add(0xa9, "bitor", "", -2, 1, 0, 0);
                    _Add(0xaa, "bitxor", "", -2, 1, 0, 0);
                    _Add(0x41, "call", "n", -2, 1, 0, OP_STACK_ARGS);
                    _Add(0x43, "callmethod", "mn", -1, 1, 0, OP_STACK_ARGS);
                    _Add(0x4c, "callproplex", "2n", -1, 1, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x46, "callproperty", "2n", -1, 1, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x4f, "callpropvoid", "2n", -1, 0, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x44, "callstatic", "mn", -1, 1, 0, OP_STACK_ARGS);
                    _Add(0x45, "callsuper", "2n", -1, 1, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x4e, "callsupervoid", "2n", -1, 0, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x78, "checkfilter", "", -1, 1, 0, 0);
                    _Add(0x80, "coerce", "2", -1, 1, 0, 0);
                    _Add(0x82, "coerce_a", "", -1, 1, 0, 0);
                    _Add(0x85, "coerce_s", "", -1, 1, 0, 0);
                    _Add(0x42, "construct", "n", -1, 1, 0, OP_STACK_ARGS);
                    _Add(0x4a, "constructprop", "2n", -1, 1, 0, OP_STACK_ARGS | OP_STACK_NS);
                    _Add(0x49, "constructsuper", "n", -1, 0, 0, OP_STACK_ARGS);
                    _Add(0x76, "convert_b", "", -1, 1, 0, 0);
                    _Add(0x73, "convert_i", "", -1, 1, 0, 0);
                    _Add(0x75, "convert_d", "", -1, 1, 0, 0);
                    _Add(0x77, "convert_o", "", -1, 1, 0, 0);
                    _Add(0x74, "convert_u", "", -1, 1, 0, 0);
                    _Add(0x70, "convert_s", "", -1, 1, 0, 0);
                    _Add(0xef, "debug", "D", 0, 0, 0, 0);
                    _Add(0xf1, "debugfile", "s", 0, 0, 0, 0);
                    _Add(0xf0, "debugline", "u", 0, 0, 0, 0);
                    _Add(0x94, "declocal", "r", 0, 0, 0, OP_REGISTER);
                    _Add(0xc3, "declocal_i", "r", 0, 0, 0, OP_REGISTER);
                    _Add(0x93, "decrement", "", -1, 1, 0, 0);
                    _Add(0xc1, "decrement_i", "", -1, 1, 0, 0);
                    _Add(0x6a, "deleteproperty", "2", -1, 1, 0, OP_STACK_NS);
                    _Add(0xa3, "divide", "", -2, 1, 0, 0);
                    _Add(0x2a, "dup", "", -1, 2, 0, 0);
                    _Add(0x06, "dxns", "s", 0, 0, 0, OP_SET_DXNS);
                    _Add(0x07, "dxnslate", "", -1, 0, 0, OP_SET_DXNS);
                    _Add(0xab, "equals", "", -2, 1, 0, 0);
                    _Add(0x72, "esc_xattr", "", -1, 1, 0, 0);
                    _Add(0x71, "esc_xelem", "", -1, 1, 0, 0);
                    _Add(0x5e, "findproperty", "2", 0, 1, 0, OP_STACK_NS);
                    _Add(0x5d, "findpropstrict", "2", 0, 1, 0, OP_STACK_NS);
                    _Add(0x59, "getdescendants", "2", -1, 1, 0, OP_STACK_NS);
                    _Add(0x64, "getglobalscope", "", 0, 1, 0, 0);
                    _Add(0x6e, "getglobalslot", "u", 0, 1, 0, 0);
                    _Add(0x60, "getlex", "2", 0, 1, 0, 0); //multiname may not be runtime
                    _Add(0x62, "getlocal", "r", 0, 1, 0, OP_REGISTER);
                    _Add(0xd0, "getlocal_0", "", 0, 1, 0, OP_REGISTER);
                    _Add(0xd1, "getlocal_1", "", 0, 1, 0, OP_REGISTER);
                    _Add(0xd2, "getlocal_2", "", 0, 1, 0, OP_REGISTER);
                    _Add(0xd3, "getlocal_3", "", 0, 1, 0, OP_REGISTER);
                    _Add(0x66, "getproperty", "2", -1, 1, 0, OP_STACK_NS);
                    _Add(0x65, "getscopeobject", "u", 0, 1, 0, 0); // u = index into scope stack
                    _Add(0x6c, "getslot", "u", -1, 1, 0, 0);
                    _Add(0x04, "getsuper", "2", -1, 1, 0, OP_STACK_NS);
                    _Add(0xaf, "greaterthan", "", -2, 1, 0, 0);
                    _Add(0xb0, "greaterequals", "", -2, 1, 0, 0);
                    _Add(0x1f, "hasnext", "", -2, 1, 0, 0);
                    _Add(0x32, "hasnext2", "rr", 0, 1, 0, OP_REGISTER);
                    _Add(0x13, "ifeq", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x12, "iffalse", "j", -1, 0, 0, OP_BRANCH);
                    _Add(0x18, "ifge", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x17, "ifgt", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x16, "ifle", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x15, "iflt", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x0f, "ifnge", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x0e, "ifngt", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x0d, "ifnle", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x0c, "ifnlt", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x14, "ifne", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x19, "ifstricteq", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x1a, "ifstrictne", "j", -2, 0, 0, OP_BRANCH);
                    _Add(0x11, "iftrue", "j", -1, 0, 0, OP_BRANCH);
                    _Add(0xb4, "in", "", -2, 1, 0, 0);
                    _Add(0x92, "inclocal", "r", 0, 0, 0, OP_REGISTER);
                    _Add(0xc2, "inclocal_i", "r", 0, 0, 0, OP_REGISTER);
                    _Add(0x91, "increment", "", -1, 1, 0, 0);
                    _Add(0xc0, "increment_i", "", -1, 1, 0, 0);
                    _Add(0x68, "initproperty", "2", -2, 0, 0, OP_STACK_NS);
                    _Add(0xb1, "instanceof", "", -2, 1, 0, 0);
                    _Add(0xb2, "istype", "2", -1, 1, 0, 0); // may not be a runtime multiname
                    _Add(0xb3, "istypelate", "", -2, 1, 0, 0);
                    _Add(0x10, "jump", "j", 0, 0, 0, OP_JUMP);
                    _Add(0x08, "kill", "r", 0, 0, 0, OP_REGISTER);
                    _Add(0x09, "label", "", 0, 0, 0, OP_LABEL);
                    _Add(0xae, "lessequals", "", -2, 1, 0, 0);
                    _Add(0xad, "lessthan", "", -2, 1, 0, 0);
                    _Add(0x1b, "lookupswitch", "S", -1, 0, 0, OP_LOOKUPSWITCH);
                    _Add(0xa5, "lshift", "", -2, 1, 0, 0);
                    _Add(0xa4, "modulo", "", -2, 1, 0, 0);
                    _Add(0xa2, "multiply", "", -2, 1, 0, 0);
                    _Add(0xc7, "multiply_i", "", -2, 1, 0, 0);
                    _Add(0x90, "negate", "", -1, 1, 0, 0);
                    _Add(0xc4, "negate_i", "", -1, 1, 0, 0);
                    _Add(0x57, "newactivation", "", 0, 1, 0, OP_NEED_ACTIVATION);
                    _Add(0x56, "newarray", "n", 0, 1, 0, OP_STACK_ARGS);
                    _Add(0x5a, "newcatch", "u", 0, 1, 0, 0); //u = index into exception_info
                    _Add(0x58, "newclass", "c", -1, 1, 0, 0); //c = index into class_info
                    _Add(0x40, "newfunction", "m", 0, 1, 0, 0); //i = index into method_info
                    _Add(0x55, "newobject", "n", 0, 1, 0, OP_STACK_ARGS2);
                    _Add(0x1e, "nextname", "", -2, 1, 0, 0);
                    _Add(0x23, "nextvalue", "", -2, 1, 0, 0);
                    _Add(0x02, "nop", "", 0, 0, 0, 0);
                    _Add(0x96, "not", "", -1, 1, 0, 0);
                    _Add(0x29, "pop", "", -1, 0, 0, 0);
                    _Add(0x1d, "popscope", "", 0, 0, -1, 0);
                    _Add(0x24, "pushbyte", "b", 0, 1, 0, 0);
                    _Add(0x2f, "pushdouble", "f", 0, 1, 0, 0); //index into floats
                    _Add(0x27, "pushfalse", "", 0, 1, 0, 0);
                    _Add(0x2d, "pushint", "I", 0, 1, 0, 0); //index into ints
                    _Add(0x31, "pushnamespace", "N", 0, 1, 0, 0); //index into namespace
                    _Add(0x28, "pushnan", "", 0, 1, 0, 0);
                    _Add(0x20, "pushnull", "", 0, 1, 0, 0);
                    _Add(0x30, "pushscope", "", -1, 0, 1, 0);
                    _Add(0x25, "pushshort", "u", 0, 1, 0, 0);
                    _Add(0x2c, "pushstring", "s", 0, 1, 0, 0);
                    _Add(0x26, "pushtrue", "", 0, 1, 0, 0);
                    _Add(0x2e, "pushuint", "U", 0, 1, 0, 0); //index into uints
                    _Add(0x21, "pushundefined", "", 0, 1, 0, 0);
                    _Add(0x1c, "pushwith", "", -1, 0, 1, 0);
                    _Add(0x48, "returnvalue", "", -1, 0, 0, OP_RETURN);
                    _Add(0x47, "returnvoid", "", 0, 0, 0, OP_RETURN);
                    _Add(0xa6, "rshift", "", -2, 1, 0, 0);
                    _Add(0x63, "setlocal", "r", -1, 0, 0, OP_REGISTER);
                    _Add(0xd4, "setlocal_0", "", -1, 0, 0, OP_REGISTER);
                    _Add(0xd5, "setlocal_1", "", -1, 0, 0, OP_REGISTER);
                    _Add(0xd6, "setlocal_2", "", -1, 0, 0, OP_REGISTER);
                    _Add(0xd7, "setlocal_3", "", -1, 0, 0, OP_REGISTER);
                    _Add(0x6f, "setglobalslot", "u", -1, 0, 0, 0);
                    _Add(0x61, "setproperty", "2", -2, 0, 0, OP_STACK_NS);
                    _Add(0x6d, "setslot", "u", -2, 0, 0, 0);
                    _Add(0x05, "setsuper", "2", -2, 0, 0, OP_STACK_NS);
                    _Add(0xac, "strictequals", "", -2, 1, 0, 0);
                    _Add(0xa1, "subtract", "", -2, 1, 0, 0);
                    _Add(0xc6, "subtract_i", "", -2, 1, 0, 0);
                    _Add(0x2b, "swap", "", -2, 2, 0, 0);
                    _Add(0x03, "throw", "", -1, 0, 0, OP_THROW);
                    _Add(0x95, "typeof", "", -1, 1, 0, 0);
                    _Add(0xa7, "urshift", "", -2, 1, 0, 0);

                    /* Alchemy opcodes */
                    _Add(0x3a, "si8", "", -2, 0, 0, 0);
                    _Add(0x3b, "si16", "", -2, 0, 0, 0);
                    _Add(0x3c, "si32", "", -2, 0, 0, 0);
                    _Add(0x3d, "sf32", "", -2, 0, 0, 0);
                    _Add(0x3e, "sf64", "", -2, 0, 0, 0);
                    _Add(0x35, "li8", "", -1, 1, 0, 0);
                    _Add(0x36, "li16", "", -1, 1, 0, 0);
                    _Add(0x37, "li32", "", -1, 1, 0, 0);
                    _Add(0x38, "lf32", "", -1, 1, 0, 0);
                    _Add(0x39, "lf64", "", -1, 1, 0, 0);
                    _Add(0x50, "sxi1", "", -1, 1, 0, 0);
                    _Add(0x51, "sxi8", "", -1, 1, 0, 0);
                    _Add(0x52, "sxi16", "", -1, 1, 0, 0);

                    /* opcodes not documented, but seen in the wild */
                    _Add(0x53, "applytype", "n", -1, 1, 0, OP_STACK_ARGS); //seen in builtin.abc

                    /* dummy instructions. Warning: these are not actually supported by flash */
                    _Add(0xfb, "__pushpackage__", "s", 0, 1, 0, OP_INTERNAL);
                    _Add(0xfc, "__rethrow__", "", 0, 0, 0, OP_THROW | OP_INTERNAL);
                    _Add(0xfd, "__fallthrough__", "s", 0, 0, 0, OP_INTERNAL);
                    _Add(0xfe, "__continue__", "s", 0, 0, 0, OP_RETURN | OP_INTERNAL);
                    _Add(0xff, "__break__", "s", 0, 0, 0, OP_RETURN | OP_INTERNAL);
                }
            }
            static private void _Add(byte opcode, string name, string parameters, int stack_minus, int stack_plus, int scope_stack_plus, uint flags)
            {
                _opcodes[opcode] = new OpCode(opcode, name, parameters, stack_minus, stack_plus, scope_stack_plus, flags);
            }
        }

        internal void ParseCode(DoABCTag doabc)
        {
            if (doabc.Strings.ContainsValue("CURRENT_VERSION"))
            {

            }
            try
            {
                while (_byteReader.LeftBytes > 0)
                {
                    byte __opcode = _byteReader.GetByte();
                    if (OpCodes.Exists(__opcode))
                    {
                        OpCode __OpCode = OpCodes.GetOpCode(__opcode);
                        char[] __parameters = __OpCode.parameters.ToCharArray();
                        __OpCode.datas = new object[__parameters.Length];
                        for (int __i = 0; __i < __parameters.Length; __i++)
                        {
                            object data = null;
                            char __parameter = __parameters[__i];
                            if (__parameter == 'n') // generic integer
                            {
                                data = _byteReader.GetInt30();
                            }
                            else if (__parameter == '2') //multiname
                            {
                                data = doabc.Multinames[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'N') //namespace
                            {
                                data = doabc.Namespaces[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'U') //uint
                            {
                                data = doabc.UIntegers[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'I') //int
                            {
                                data = doabc.Integers[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'f') //double
                            {
                                data = doabc.Doubles[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'm') //method
                            {
                                data = doabc.MethodInfos[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'c') //classinfo
                            {
                                data = doabc.Classes[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'i') //methodbody
                            {
                                _byteReader.GetUInt30();
                                //data = array_getvalue(file->method_bodies, swf_GetU30(tag));
                            }
                            else if (__parameter == 'u') // generic uinteger
                            {
                                data = _byteReader.GetInt30();
                            }
                            else if (__parameter == 'r') // local register
                            {
                                data = _byteReader.GetInt30();
                            }
                            else if (__parameter == 'b') // byte
                            {
                                data = _byteReader.GetByte();
                            }
                            else if (__parameter == 'j') // jump
                            {
                                data = _byteReader.GetU24();
                            }
                            else if (__parameter == 's') // string
                            {
                                data = doabc.Strings[_byteReader.GetUInt30()];
                            }
                            else if (__parameter == 'D') // debug
                            {
                                /*type, usually 1*/
                                byte __type = _byteReader.GetByte();
                                if (__type != 1)
                                {
                                    //fprintf(stderr, "Unknown debug type: %02x\n", type);
                                }
                                /*register name*/
                                //code->data[0] = strdup((char*)pool_lookup_string(pool, swf_GetU30(tag)));
                                _byteReader.GetUInt30();
                                /*register index*/
                                //code->data[1] = (void*)(ptroff_t)swf_GetU8(tag);
                                _byteReader.GetByte();
                                /*unused*/
                                _byteReader.GetUInt30();
                            }
                            else if (__parameter == 'S') // switch statement
                            {
                                uint __def = _byteReader.GetU24();
                                uint __targets_number = _byteReader.GetUInt30();
                                for (uint __j = 0; __j < __targets_number; __j++)
                                {
                                    uint __target = _byteReader.GetU24();
                                }
                                /*
                                lookupswitch_t* l = malloc(sizeof(lookupswitch_t));
                                l->def = (code_t*)(ptroff_t)swf_GetS24(tag);
                                l->targets = list_new();
                                int num = swf_GetU30(tag) + 1;
                                int t;
                                for (t = 0; t < num; t++)
                                    list_append(l->targets, (code_t*)(ptroff_t)swf_GetS24(tag));
                                data = l;
                                 * */
                            }
                            else
                            {
                                //printf("Can't parse opcode param type \"%c\" (for op %02x %s).\n", *p, code->opcode, op->name);
                                return;
                            }
                            __OpCode.datas[__i] = data;
                            //code->data[pos++] = data;
                        }
                        Codes.Add(__OpCode);
                    }
                    else
                    {

                    }

                }
                if (Codes.Count > 0 && doabc.Strings.ContainsValue("CURRENT_VERSION"))
                {
                    for (int __i = 0; __i < Codes.Count; __i++)
                    {
                        Console.WriteLine(__i.ToString("00000") + ")  " + Codes[__i].name);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
