using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.Swf
{
    public class ByteReader
	{
        protected uint _pos;
        protected int _len;
        protected byte _bitMask;
        protected byte[] _bytes;
        protected static UTF8Encoding _utf8Encoder = new UTF8Encoding(false);
        internal ByteReader()
        {
            this.SetBytes(new byte[0]);
        }
        internal ByteReader(byte[] bytes)
        {
            this.SetBytes(bytes);
        }
        internal void SetBytes(byte[] bytes)
        {
            this._bytes = bytes;
            this._len = _bytes.Length;
            this.Reset();
        }
        internal uint Size
        {
            get { return (uint)_bytes.Length; }
        }
        internal int LeftBytes
        {
            get { return _bytes.Length - (int)_pos; }
        }
        internal uint Position
		{
			get
			{
				return this._pos;
			}
			set
			{
				this._pos = value;
                this.Align();
			}
		}
        internal void Reset()
		{
			_pos = 0;
			_bitMask = 0x80;
		}
        internal bool GetBit()
		{
			bool __result = (_bytes[_pos] & _bitMask) != 0;
			_bitMask /= 2;
			if (_bitMask == 0)
			{
				_pos++;
				_bitMask = 0x80;
			}
			return __result;
		}
        internal uint GetBits(uint count)
		{
			uint __result = 0;
			for(int i = 0; i < count; i++)
			{
				__result = GetBit() ? (__result << 1) | 1 : __result << 1;
			}
			return __result;
		}
        internal int GetSignedNBits(uint count)
		{
			uint __result = GetBits(count);
			if((__result & (1 << (int)(count - 1))) != 0)
			{
				__result |= (uint)(-1 << (int)count);
			}
			return (int)__result;
		}
        internal byte GetByte()
		{
			Align();
			return _bytes[_pos++];
		}
        internal byte[] GetBytes(uint count)
		{
			Align();
			byte[] __byteArray = new byte[count];
			for (int i = 0; i < count; i++)
			{
				__byteArray[i] = _bytes[_pos++];
			}
			return __byteArray;
		}
        
        internal uint GetUInt30()
        {
            int __shift = 0;
            uint __return = 0;
            while (true)
            {
                uint __byte = GetByte();
                __return |= (__byte & (uint)127) << __shift;
                __shift += 7;
                if ((__byte & (uint)128) == 0 || __shift >= 32)
                    break;
            }
            return __return;
        }
        internal int GetInt30()
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(GetUInt30()), 0);
        }

        internal uint GetU24()
        {
            uint b1 = GetByte();
            uint b2 = GetByte();
            uint b3 = GetByte();
            return b3<<16|b2<<8|b1;
        }
        internal int GetS24()
        {
            int b1 = GetByte();
            int b2 = GetByte();
            int b3 = GetByte();
            if ((b3&0x80) != 0) {
                return -1-((b3<<16|b2<<8|b1)^0xffffff);
            } else {
                return b3<<16|b2<<8|b1;
            }
        }

        internal byte PeekByte()
		{
			Align();
			return _bytes[_pos];
		}
        internal byte[] PeekBytes(uint len)
        {
            return PeekBytes(_pos, len);
        }
        internal byte[] PeekBytes(uint start, uint len)
		{
			Align();
			byte[] __result = new byte[len];
			Array.Copy(_bytes, start, __result, 0, len);
			return __result;
		}
        internal UInt16 GetUI16()
		{
			Align();
			return (UInt16)((_bytes[_pos++]) + (_bytes[_pos++] << 8));
		}
        internal Int16 GetInt16()
		{
			Align();
            return (Int16)((_bytes[_pos++]) + (_bytes[_pos++] << 8));
		}
        internal Int32 GetInt32()
		{
			Align();
			return (Int32)
				(
					(uint)(_bytes[_pos++]) +
					(uint)(_bytes[_pos++] << 8) +
					(uint)(_bytes[_pos++] << 16) +
					(uint)(_bytes[_pos++] << 24)
				);
		}
        internal Int64 GetInt64()
        {
            Align();
            return (Int64)
                (
                    (uint)(_bytes[_pos++]) +
                    (uint)(_bytes[_pos++] << 8) +
                    (uint)(_bytes[_pos++] << 16) +
                    (uint)(_bytes[_pos++] << 24) +
                    (uint)(_bytes[_pos++] << 32) +
                    (uint)(_bytes[_pos++] << 40) +
                    (uint)(_bytes[_pos++] << 48) +
                    (uint)(_bytes[_pos++] << 56)
                );
        }
        internal uint PeekUI16()
		{
			Align();
			return (uint)((_bytes[_pos]) +	(_bytes[_pos + 1] << 8));
		}
        internal uint GetUI32()
		{
			return	(uint)(_bytes[_pos++]) +
					(uint)(_bytes[_pos++] << 8) +
					(uint)(_bytes[_pos++] << 16) +
					(uint)(_bytes[_pos++] << 24);
		}
        internal void SkipBytes(uint count)
		{
			Align();
			_pos += count;
		}
        internal void SkipBits(uint count)
		{
			_pos += (uint)(count / 8);
			count = count % 8;
			for (int __i = 0; __i < count; __i++)
			{
				_bitMask /= 2;
				if (_bitMask == 0)
				{
					_pos++;
					_bitMask = 0x80;
				}				
			}
		}
        internal float GetFixedNBits(uint nBits)
		{
			float __result = this.GetSignedNBits(nBits);
			if(nBits > 1)
			{
				__result = __result / 0x10000;
			}
			else if(nBits == 1 && __result == -1F)
			{
				__result = -1F / (float)(0x100000);
			}
			else if (nBits == 1 && __result == 0F)
			{
				__result = 1F / (float)(0x100000);
			}
			else
			{
				__result = __result / 0x10000;
			}
			return __result;
		}
        internal float GetFixed16_16()
		{
			return this.GetFixedNBits(32);
		}
        internal float GetFixed8_8()
		{
			float __result = this.GetSignedNBits(16);
			return __result / 0X100;
		}
        internal float GetFloat32()
		{
            byte[] __bytes = this.GetBytes(4);
            return BitConverter.ToSingle(__bytes, 0);
		}
        internal double GetDouble()
		{
			byte[] __bytes = this.GetBytes(8);
			return BitConverter.ToDouble(__bytes, 0);
		}
        internal void Align()
		{
			if (_bitMask != 0x80)
			{
				_bitMask = 0x80;
				_pos++;
			}
		}
        internal string GetString()
        {
            return GetString(0);
		}
        internal string GetUTF8Bytes(uint len)
        {
            Align();
            byte[] __bytes = GetBytes(len);
            return _utf8Encoder.GetString(__bytes);
        }
        internal string GetString(uint len)
        {
            Align();
            uint __start = _pos;
            char __c = (char)_bytes[_pos++];
            while ((byte)__c != 0)
            {
                __c = (char)_bytes[_pos++];
            }
            uint __end = _pos;
            uint __len = (uint)(len == 0 ? (__end - __start - 1) : (len - (__end == (uint)(len + 1) ? 1 : 0)));
            _pos = __start;
            string __result = _utf8Encoder.GetString(_bytes, (int)__start, (int)__len);
            _pos = __start + __len;
            if ((char)_bytes[_pos] == 0)
            {
                _pos++;
            }
            //_pos = __end;
            return __result;
		}

        internal static byte[] Uncompress(byte[] compressed)
        {
            MemoryStream __compressedStream = new MemoryStream(compressed);
            ZlibStream __deflateStream = new ZlibStream(__compressedStream, CompressionMode.Decompress, false);
            MemoryStream __resultStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int readCount = __deflateStream.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                    __resultStream.Write(buffer, 0, readCount);
                else
                    break;
            }
            __deflateStream.Close();
            __compressedStream.Close();
            __compressedStream.Dispose();
            byte[] __result = __resultStream.ToArray();
            __resultStream.Close();
            __resultStream.Dispose();
            return __result;
        }

        internal static byte[]Compress(byte[] uncompressed)
        {
            MemoryStream __uncompressedStream = new MemoryStream(uncompressed);
            ZlibStream __inflateStream = new ZlibStream(__uncompressedStream, CompressionMode.Compress, false);
            MemoryStream __resultStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int readCount = __inflateStream.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                    __resultStream.Write(buffer, 0, readCount);
                else
                    break;
            }
            __inflateStream.Close();
            __uncompressedStream.Close();
            __uncompressedStream.Dispose();
            byte[] __result = __resultStream.ToArray();
            __resultStream.Close();
            __resultStream.Dispose();
            return __result;
        }

        internal static byte[] DecompressZLIB(byte[] compressed, uint unzippedSize)
        {
            return Uncompress(compressed);
        }
        internal byte[] DecompressZLIB(uint compressedSize, uint unzippedSize)
        {
            byte[] __compressed = this.GetBytes(compressedSize);
            return Uncompress(__compressed);
        }

    }
}
