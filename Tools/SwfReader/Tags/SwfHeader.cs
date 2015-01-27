using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Signature	UI8		Signature byte:
							“F” indicates uncompressed
							“C” indicates compressed (SWF 6 and later only)
		Signature	UI8		Signature byte always “W”
		Signature	UI8		Signature byte always “S”
		Version		UI8		Single byte file version (for example, 0x06 for SWF 6)
		FileLength	UI32	Length of entire file in bytes
		FrameSize	RECT	Frame size in twips
		FrameRate	UI16	Frame delay in 8.8 fixed number of frames per second
		FrameCount	UI16	Total number of frames in file
	 */
    internal enum CompressionType
    {
        None,
        Error,
        ZLIB,
        LZMA
    }

    internal class SwfHeader : IVexConvertable
	{
        internal bool IsSwf { get { return (CompressionMode != CompressionType.Error); } }
		internal bool	IsCompressed { get { return (CompressionMode != CompressionType.None); } }
        internal CompressionType CompressionMode;
        internal double CompressionRatio;
		//internal byte		Signature0;
		//internal byte		Signature1;
		//internal byte		Signature2;
        internal string Signature;

		internal byte		Version;
		internal UInt32	FileLength;
		internal Rect		FrameSize;
		internal float	FrameRate;
		internal UInt16	FrameCount;

        internal SwfHeader(SwfReader r)
		{
            this.CompressionMode = CompressionType.Error;
            this.CompressionRatio = 0;
            //byte[] __signature = r.GetBytes(3);
            this.Signature = r.GetString(3);
            //Signature = r.GetString(3);
            if (Signature == "CWS")
            {
                this.CompressionMode = CompressionType.ZLIB;
                uint __oldSize = r.Size;
                r.DecompressCWSSwf();
                uint __newSize = r.Size;
                this.CompressionRatio = ((double)__oldSize / (double)__newSize) * 100;
            }
            else if (Signature == "ZWS")
            {
                this.CompressionMode = CompressionType.LZMA;
                //r.DecompressZWSSwf
                uint __oldSize = r.Size;
                r.DecompressCWSSwf();
                uint __newSize = r.Size;
                this.CompressionRatio = (double)__oldSize / (double)__newSize;
            }
            else if (Signature == "FWS")
            {
                this.CompressionMode = CompressionType.None;
			}
            if (this.CompressionMode != CompressionType.Error)
			{
				this.Version = r.GetByte();
				this.FileLength = r.GetUI32();
				this.FrameSize = new Rect(r);
				UInt16 __frate = r.GetUI16();
				this.FrameRate = (__frate >> 8) + ((__frate & 0xFF) / 0xFF);
				this.FrameCount = r.GetUI16();
			}
			else
			{
				this.Version = 0;
				this.FileLength = 0;
				this.FrameSize = new Rect(0,0,0,0);
				this.FrameRate = 0;
				this.FrameCount = 0;
			}
		}

        internal bool Validate()
		{
			return false;
		}
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
		{
			if (this.CompressionMode == CompressionType.ZLIB)
			{
                w.AppendByte((byte)'C');
			}
            else if (this.CompressionMode == CompressionType.LZMA)
            {
                w.AppendByte((byte)'Z');
            }
            else
            {
                w.AppendByte((byte)'F');
            }
			w.AppendByte((byte)'W');
			w.AppendByte((byte)'S');
			
			w.AppendByte(this.Version);

			w.AppendUI32(this.FileLength);
			this.FrameSize.ToSwf(w);

			ushort frateWhole = (ushort)this.FrameRate;
			uint frateDec = (((uint)this.FrameRate * 0x100) & 0xFF);
			w.AppendUI16((uint)((frateWhole << 8) + frateDec));

			w.AppendUI16(this.FrameCount);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("[HEADER]        File version: " + this.Version);
            w.Write("[HEADER]        File is ");
            if (this.CompressionMode == CompressionType.None)
            {
                w.WriteLine("not compressed");
            }
            else
            {
                w.WriteLine((this.CompressionMode == CompressionType.ZLIB ? "zlib" : "lzma") + " compressed. Ratio: " + (int)CompressionRatio + "%");
            }
            w.WriteLine("[HEADER]        File size: " + this.FileLength);
            w.WriteLine("[HEADER]        Frame rate: " + this.FrameRate);
            w.WriteLine("[HEADER]        Frame count: " + this.FrameCount);
            w.WriteLine("[HEADER]        Movie width: " + this.FrameSize.Width);
            w.WriteLine("[HEADER]        Movie height: " + this.FrameSize.Height);
        }
#endif
    }
}
