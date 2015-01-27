using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueSharp.GameFiles.Tools
{

    public class IniBinHash
    {
        public uint this[string fullvarname]
        {
            get
            {
                return IniBinHash.GetHash(fullvarname);
            }
        }
        public uint this[string section, string varname]
        {
            get
            {
                return IniBinHash.GetHash(section + "*" + varname);
            }
        }
        public uint this[string section, string subsection, string varname]
        {
            get
            {
                return IniBinHash.GetHash(section + "*" + subsection + "*" + varname);
            }
        }

        public static UInt32 GetHash(string Key)
        {
            UInt32 hash = 0;
            foreach (var c in Key.ToLowerInvariant())
            {
                hash = c + 65599 * hash;
            }
            return hash;
        }
    }
    
    public static class IniBinReader
    {
        /// <summary>
        /// Parse bytes from inibin file
        /// </summary>
        /// <param name="buffer">bytes from inibin file</param>
        /// <returns>Dictionary of hashes founded and its value </returns>
        public static Dictionary<uint, object> GetValues(byte[] buffer)
        {
            try
            {
                using (IniBinStream __is = new IniBinStream(buffer))
                {
                    return __is.GetValues();
                }
            }
            catch
            {
                return new Dictionary<uint, object>();
            }
        }

        /// <summary>
        /// A class that parse the inibin bytes
        /// </summary>
        private class IniBinStream : MemoryStream
        {
            public IniBinStream(byte[] buffer) : base(buffer) { }

            public Dictionary<uint, object> GetValues()
            {
                Dictionary<uint, object> __result = new Dictionary<uint, object>();
                this.Seek(0, SeekOrigin.Begin);
                int __version = this.ReadByte();
                int __rawOffset = ReadInt16();
                uint __contentType = this.ReadUInt16();
                if ((__contentType & 0x0001) != 0) //Uint32
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadUInt32());
                    }
                }
                if ((__contentType & 0x0002) != 0) //float
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadSingle());
                    }
                }
                if ((__contentType & 0x0004) != 0) //half float (1/10 * byte)
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadByte() * 0.1F);
                    }
                }
                if ((__contentType & 0x0008) != 0) //Int16
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadInt16());
                    }
                }
                if ((__contentType & 0x0010) != 0) //byte
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadByte());
                    }
                }
                if ((__contentType & 0x0020) != 0) //bool
                {
                    int __index = 8;
                    int __value = 0;
                    foreach (uint __key in this.ReadKeys())
                    {
                        if (__index > 7)
                        {
                            __value = this.ReadByte();
                            __index = 0;
                        }
                        __result.Add(__key, ((0x1 & __value) == 1));
                        __value = __value >> 1;
                        __index++;
                    }
                }
                if ((__contentType & 0x0040) != 0) //??? //loadingscreen // 
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, new Vector3(this.ReadByte(), this.ReadByte(), this.ReadByte()));
                    }
                }
                // Newer section.
                // I don't know what exactly these values represent.
                // I think it's related to champions with the new rage mechanic.
                // I'm just using it to increment the stream.
                // So, when I get to the part to read in strings, the pointer is at the
                // correct location.
                if ((__contentType & 0x0080) != 0)
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        float b1 = this.ReadSingle();
                        float b2 = this.ReadSingle();
                        float b3 = this.ReadSingle();
                        __result.Add(__key, new Color(b1, b2, b3));
                    }
                }
                if ((__contentType & 0x0100) != 0) //???  //data/spells/rumbleflamethrower.inibin
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadInt16());
                    }
                }
                if ((__contentType & 0x0200) != 0) //Vector2
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadVector2());
                    }
                }
                if ((__contentType & 0x0400) != 0) //4-byte color values or something?
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadUInt32());
                    }
                }
                if ((__contentType & 0x0800) != 0) //???  //data/menu_sc4/hud.inibin //SharpDX.Vector3
                {
                    foreach (uint __key in this.ReadKeys())
                    {
                        __result.Add(__key, this.ReadVector3());
                    }
                }
                if ((__contentType & 0x1000) != 0) //string
                {
                    Dictionary<uint, UInt16> __strings = new Dictionary<uint, ushort>();
                    foreach (uint __key in this.ReadKeys())
                    {
                        if (!__strings.ContainsKey(__key))
                        {
                            __strings.Add(__key, this.ReadUInt16());
                        }
                        else
                        {
                            __strings[__key] = this.ReadUInt16();
                        }
                    }
                    foreach (KeyValuePair<uint, UInt16> __string in __strings)
                    {
                        __result.Add(__string.Key, this.ReadString(__string.Value));
                    }
                }
                if ((__contentType & 0x2000) != 0) //??  not seen yet
                {
                    uint[] __keys = this.ReadKeys();
                    long __pos = this.Position;
                    byte[] __tt = new byte[this.Length - this.Position];
                    this.Read(__tt, 0, __tt.Length);
                    this.Seek(__pos, SeekOrigin.Begin);
                    foreach (uint __key in __keys)
                    {
                    }
                }
                if ((__contentType & 0x4000) != 0) //??  not seen yet
                {
                    uint[] __keys = this.ReadKeys();
                    long __pos = this.Position;
                    byte[] __tt = new byte[this.Length - this.Position];
                    this.Read(__tt, 0, __tt.Length);
                    this.Seek(__pos, SeekOrigin.Begin);
                    foreach (uint __key in __keys)
                    {
                    }
                }
                if ((__contentType & 0x8000) != 0) //??  not seen yet
                {
                    uint[] __keys = this.ReadKeys();
                    long __pos = this.Position;
                    byte[] __tt = new byte[this.Length - this.Position];
                    this.Read(__tt, 0, __tt.Length);
                    this.Seek(__pos, SeekOrigin.Begin);
                    foreach (uint __key in __keys)
                    {
                    }
                }
                return __result;
            }

            private byte[] ReadBytes(int count)
            {
                byte[] __buffer = new byte[count];
                this.Read(__buffer, 0, count);
                return __buffer;
            }
            private Int16 ReadInt16() { return BitConverter.ToInt16(ReadBytes(2), 0); }
            private UInt16 ReadUInt16() { return BitConverter.ToUInt16(ReadBytes(2), 0); }
            private Int32 ReadInt32() { return BitConverter.ToInt32(ReadBytes(4), 0); }
            private UInt32 ReadUInt32() { return BitConverter.ToUInt32(ReadBytes(4), 0); }
            private Single ReadSingle() { return BitConverter.ToSingle(ReadBytes(4), 0); }
            private uint[] ReadKeys()
            {
                int __keycount = this.ReadInt16();
                uint[] __keys = new uint[__keycount];
                for (int i = 0; i < __keycount; i++)
                {
                    __keys[i] = this.ReadUInt32();
                }
                return __keys;
            }
            private string ReadString(int offset)
            {
                long oldPos = this.Position;
                this.Seek(offset, SeekOrigin.Current);
                StringBuilder sb = new StringBuilder();
                int c;
                while ((c = this.ReadByte()) > 0)
                {
                    sb.Append((char)c);
                }
                this.Seek(oldPos, SeekOrigin.Begin);
                return sb.ToString();
            }
            private Vector3 ReadVector3()
            {
                return new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
            }
            private Vector2 ReadVector2()
            {
                return new Vector2(this.ReadSingle(), this.ReadSingle());
            }
        }
    }

}
