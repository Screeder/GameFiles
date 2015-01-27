using System;
using System.Drawing;
using System.IO;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineBitsTag : ISwfTag
	{
		internal UInt16 CharacterId;
		internal byte[] JpegData;
		internal bool HasOwnTable = false;
		internal bool HasAlphaData = false;
		internal byte[] CompressedAlphaData;

		// this needs to combine with the jpegTables in the swfCompilationUnit to make an image
        internal DefineBitsTag(SwfReader r, uint curTagLen, bool hasOwnTable, bool hasAlphaData)
            : base(TagType.DefineBits)
		{
			this.HasOwnTable = hasOwnTable;
			this.HasAlphaData = hasAlphaData;

            if (hasOwnTable && !hasAlphaData)
            {
                tagType = TagType.DefineBitsJPEG2;
            }
            else if(hasOwnTable && hasAlphaData)
            {
                tagType = TagType.DefineBitsJPEG3;
            }

			CharacterId = r.GetUI16();
			if (!hasAlphaData)
			{
				if(!hasOwnTable)
				{
				    r.GetBytes(2); // SOI
				    JpegData = r.GetBytes(curTagLen - 6);
				    r.GetBytes(2); // EOI
				}
				else
				{
					JpegData = r.GetBytes(curTagLen - 2);
					CleanData();
				}
			}
			else
			{
				uint jpgDataSize = r.GetUI32();
				JpegData = r.GetBytes(jpgDataSize);

				// ignore alpha for now
				CompressedAlphaData = r.GetBytes(curTagLen - 6 - jpgDataSize);
				CleanData();
			}
		}
		private void CleanData()
		{
			// first header
			if (JpegData[0] == 0xFF && JpegData[1] == 0xD9 && JpegData[2] == 0xFF && JpegData[3] == 0xD8)
			{
				byte[] copy = new byte[JpegData.Length - 4];
				Array.Copy(JpegData, 4, copy, 0, JpegData.Length - 4);
				JpegData = copy;
			}
			// cleans out bug markers in swf jpgs
			int index = 2;
			int len = this.JpegData.Length;
			while (index + 3 < len)
			{
				byte b0 = JpegData[index];
				if (b0 != 0xFF)
				{
					break;
				}
				if (JpegData[index + 1] == 0xD9 && JpegData[index + 2] == 0xFF && JpegData[index + 3] == 0xD8)
				{
					// bingo
					byte[] copy = new byte[JpegData.Length - 4];
					Array.Copy(JpegData, 0, copy, 0, index);
					Array.Copy(JpegData, index + 4, copy, index, JpegData.Length - index - 4);
					JpegData = copy;
					break;
				}
				else
				{
					int tagLen = (JpegData[index + 2] << 8) + JpegData[index + 3] + 2;
					index += tagLen;
				}
			}
		}

        internal Bitmap GetBitmap()
        {
            MemoryStream __jpegStream = new MemoryStream(JpegData);
            Bitmap __jpegBitmap = new Bitmap(__jpegStream);
            if (HasAlphaData)
            {
                Bitmap __alphaBitmap = new Bitmap(__jpegBitmap.Width, __jpegBitmap.Height);
                byte[] __alphaData = SwfReader.Uncompress(CompressedAlphaData);
                int __alphaPos = 0;
                for (int __i = 0; __i < __alphaBitmap.Height; __i++)
                {
                    for (int __j = 0; __j < __alphaBitmap.Width; __j++)
                    {
                        Color __oldColor = __jpegBitmap.GetPixel(__j, __i);
                        Color __newColor = Color.FromArgb(__alphaData[__alphaPos], __oldColor);
                        __alphaPos++;
                        __alphaBitmap.SetPixel(__j, __i, __newColor);
                    }
                }
                __jpegBitmap = __alphaBitmap;
            }
            return __jpegBitmap;
        }

#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(CharacterId);

            if (!HasAlphaData)
            {
                if (!HasOwnTable)
                {
                    w.AppendByte(0xFF);
                    w.AppendByte(0xD8);

                    w.AppendBytes(JpegData);

                    w.AppendByte(0xFF);
                    w.AppendByte(0xD9);
                }
                else
                {
                    w.AppendBytes(JpegData);
                }
            }
            else
            {
                w.AppendUI32((uint)JpegData.Length);
                w.AppendBytes(JpegData);

                w.AppendBytes(CompressedAlphaData);
            }

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("DefineBits id_" + CharacterId);
        }
#endif
    }
}
