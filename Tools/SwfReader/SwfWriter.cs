#if SWFWRITER
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
namespace Swf
{
	internal class SwfWriter
    {
        internal const int twip = 20;
        internal const float twipF = 20.0F;
        private const long MAX_SIZE = 1000 * 1024;

        private byte bitMask;
        private byte curBits = 0;
        private MemoryStream stream;

		private static System.Text.UTF8Encoding utf8Encoder = new System.Text.UTF8Encoding(false);

		internal SwfWriter(MemoryStream stream)
        {
            this.stream = stream;
            Reset();
        }
        internal long Position
        {
            get
            {
				return stream.Position;
            }
            set
            {
                Align();
				stream.Position = value;
            }
        }
        internal void Reset()
		{
			stream.Position = 0;
            curBits = 0;
            bitMask = 0x80;
        }
        internal void AppendBit(bool value)
        {
            curBits = value ? (byte)(curBits | bitMask) : curBits;
            bitMask /= 2;
            if (bitMask == 0)
            {
                stream.WriteByte(curBits);
				bitMask = 0x80;
				curBits = 0x00;
            }
        }
        internal void AppendBits(uint value, uint count)
		{
			if (count == 0)
			{
				return;
			}
			count--;
			count &= 0x1F;

            uint mask = (uint)Math.Pow(2, count);
            for (int i = (int)count; i >= 0; i--)
            {
                AppendBit((value & mask) > 0);
                mask /= 2;
            }
        }
        internal void AppendSignedNBits(int value, uint count)
        {
			count = Math.Min(count, 32);

			uint mask = (uint)Math.Pow(2, count - 1);
            for (int i = 0; i < count; i++)
            {
                AppendBit((value & mask) > 0);
                mask >>= 1;
            }
        }
        internal void AppendByte(byte value)
        {
            stream.WriteByte(value);
        }
		internal void AppendBytes(byte[] values)
        {
            Align();
			stream.Write(values, 0, values.Length);
        }
        internal void AppendUI16(uint value)
        {
            Align();
			stream.WriteByte((byte)(value & 0xFF));
			stream.WriteByte((byte)(value >> 8));
        }
        internal void AppendInt16(int value)
        {
			AppendUI16((uint)value);
        }
        internal void AppendUI32(uint value)
        {
            Align();
			stream.WriteByte((byte)(value & 0x00FF));
			stream.WriteByte((byte)((value >> 8) & 0x00FF));
			stream.WriteByte((byte)((value >> 16) & 0x00FF));
			stream.WriteByte((byte)(value >> 24));
        }
        internal void AppendInt32(int value)
        {
            AppendUI32((uint)value);
        }
        internal void AppendFixedNBits(double value, uint bits)
		{
			int bitValue = (int)(value * 0x10000);
			AppendSignedNBits(bitValue, bits);
        }
		internal void AppendFixed16_16(double value)
        {
			Align();
			uint bot = (uint)(value * 0x10000);
			stream.WriteByte((byte)(bot & 0xFF));
			stream.WriteByte((byte)((bot >> 8) & 0xFF));

			uint top = (uint)value;
			stream.WriteByte((byte)(top & 0xFF));
			stream.WriteByte((byte)((top >> 8) & 0xFF));
        }
		internal void AppendFixed8_8(float value)
        {
			Align();
			uint bot = (uint)(value * 0x100);
			stream.WriteByte((byte)(bot & 0xFF));

			uint top = (uint)value;
			stream.WriteByte((byte)(top & 0xFF));
        }
		internal void AppendFloat32(float value)
        {
			Align();

			byte[] bytes = BitConverter.GetBytes(value);

			AppendByte(bytes[0]);
			AppendByte(bytes[1]);
			AppendByte(bytes[2]);
			AppendByte(bytes[3]);
        }
		internal void AppendDouble(double value)
		{
			Align();

			byte[] bytes = BitConverter.GetBytes(value);

			AppendByte(bytes[4]);
			AppendByte(bytes[5]);
			AppendByte(bytes[6]);
			AppendByte(bytes[7]);

			AppendByte(bytes[0]);
			AppendByte(bytes[1]);
			AppendByte(bytes[2]);
			AppendByte(bytes[3]);
        }

        internal void Align()
        {
            if (bitMask != 0x80)
            {
				AppendByte(curBits);
                bitMask = 0x80;
				curBits = 0x00;
            }
        }
        internal void AppendString(string s)
		{
			AppendBytes(utf8Encoder.GetBytes(s));
			AppendByte(0);
        }
        internal void AppendString(string s, uint len)
		{
			AppendBytes(utf8Encoder.GetBytes(s.ToCharArray(), 0, (int)len));
			AppendByte(0);
        }
		internal bool AppendTagIDAndLength(TagType tagType, uint bodyLength)
		{
			bool isLong = (bodyLength >= 0x3f);
			if (isLong)
			{
				AppendTagIDAndLength(tagType, bodyLength, true);
			}
			else
			{
				AppendTagIDAndLength(tagType, bodyLength, false);
			}
			return isLong;
		}
		internal bool AppendTagIDAndLength(TagType tagType, uint bodyLength, bool isLong)
		{
			uint highBits = ((uint)tagType) << 6;
			if (isLong)
			{
				highBits |= 0x3F;
				AppendUI16(highBits);
				AppendUI32(bodyLength);
			}
			else
			{
				highBits |= bodyLength;
				AppendUI16(highBits);
			}
			return isLong;
		}
        internal void ResetLongTagLength(TagType tagType, uint start)
        {
            ResetLongTagLength(tagType, start, false);
        }
		internal void ResetLongTagLength(TagType tagType, uint start, bool alwaysLong)
		{
			uint bodyLength = (uint)((Position - start) - 6);
			Position = start;

            bool isLong = true;
            if (alwaysLong)
            {
                AppendTagIDAndLength(tagType, (uint)bodyLength, true);
            }
            else
            {
                isLong = AppendTagIDAndLength(tagType, (uint)bodyLength);
                if (!isLong)
                {
                    byte[] bytes = new byte[bodyLength];
                    Position = start + 6;
                    this.stream.Read(bytes, 0, (int)bodyLength);
                    Position = start + 2;
                    this.stream.Write(bytes, 0, (int)bodyLength);
                }
            }
	        uint headLength = isLong ? 6u : 2u;
	        Position = start + bodyLength + headLength;
		}

		// utils

		internal static uint MinimumBits(uint value)
		{
			if (value == 0)
			{
				return 0;
			}
			uint mask = 1;
			uint bits = 1;
			for (; bits < 32; bits++)
			{
				mask <<= 1;
				if (mask > value)
				{
					break;
				}
			}
			return bits;
		}

		internal static uint MinimumBits(int value)
		{
			if (value == 0)
			{
				return 1;
			}
			return MinimumBits((uint)Math.Abs(value)) + 1;
		}

		internal static uint MinimumBits(double value)
		{
			uint result = 16 + 1; //decimal bits and sign
			if (value != 0)
			{
				result += MinimumBits((uint)Math.Abs(value));
			}
			return result;
		}

		internal static uint MinimumBits(params int[] values)
		{
			uint max = 1;
			for (int i = 0; i < values.Length; i++)
			{
				uint bits = MinimumBits(values[i]);
				max = (bits > max) ? bits : max;
			}
			return max;
		}
		internal static uint MinimumBits(params uint[] values)
		{
			return MinimumBits(values, true);
		}

		internal static uint MinimumBits(params float[] values)
		{
			uint[] converted = new uint[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				converted[i] = (uint)values[i];
			}
			return MinimumBits(converted, true) + 1 + 16; // one of sign, 16 because the decimal part is always 16
		}

		internal static uint MinimumBits(uint[] values, bool dummy)
		{
			uint maximumBits = 1;
			uint curbits;
			for (int i = 0; i < values.Length; i++)
			{
				curbits = MinimumBits(values[i]);
				if (curbits > maximumBits)
				{
					maximumBits = curbits;
				}
			}
			return maximumBits;
		}
		internal byte[] ToArray()
		{
			return stream.ToArray();
		}
		internal void Zip()
		{
			uint headerLen = 8;
			byte[] compressPart = new byte[Position - headerLen];
			byte[] source = stream.ToArray();
			Array.Copy(source, headerLen, compressPart, 0, Position - headerLen);
            byte[] compressed = ByteReader.Compress(compressPart);

			Position = 0;
			AppendByte((byte)'C');

			Position = headerLen;
			AppendBytes(compressed);

			stream.SetLength(headerLen + compressed.Length);
		}
    }
}
#endif
