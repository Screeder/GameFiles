using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	class FileAttributesTag : ISwfTag
	{
        /*
            FileAttributes
         * 
            var tag:FileAttributes = new FileAttributes();
            r.syncBits();
            r.readUBits(1); //reserved
            tag.useDirectBlit = r.readBit();
            tag.useGPU = r.readBit();
            tag.hasMetadata = r.readBit();
            tag.actionScript3 = r.readBit();
            tag.suppressCrossDomainCaching = r.readBit();
            tag.swfRelativeUrls = r.readBit();
            tag.useNetwork = r.readBit();
            r.readUBits(24); //reserved
            return tag;
         * 
            Field			Type			Comment
            Header			RECORDHEADER	Tag type = 69
            Reserved		UB[1]			Must be 0
            useDirectBlit   B[1]
            useGPU          B[1]
            HasMetadata		UB[1]			If 1, the SWF file contains the Metadata tag
                                            If 0, the SWF file does not contain the Metadata tag
            actionScript3   UB[1]
            suppressCrossDomainCaching   UB[1] 
            swfRelativeUrls UB[1]
            UseNetwork		UB[1]			If 1, this SWF file is given network file access when loaded locally
                                            If 0, this SWF file is given local file access when loaded locally
            Reserved		UB[24]			Must be 0
         */
        const uint len = 4;
		private UInt32 _flags;

        internal FileAttributesTag(SwfReader r)
            : base(TagType.FileAttributes)
		{
			this._flags = r.GetUI32();
		}
        internal bool UseDirectBlit
		{
			get
			{
				return ((this._flags & 0x02) != 0);
			}
			set
			{
                this._flags = (value) ? this._flags | 0x02 : this._flags & 0xFFFFFFFD;
			}
		}
        internal bool UseGPU
        {
            get
            {
                return ((this._flags & 0x04) != 0);
            }
            set
            {
                this._flags = (value) ? this._flags | 0x04 : this._flags & 0xFFFFFFFB;
            }
        }
        
		internal bool HasMetadata
		{
			get
			{
				return ((this._flags & 0x08) != 0);
			}
			set
			{
                this._flags = (value) ? this._flags | 0x08 : this._flags & 0xFFFFFFF7;
			}
		}
        internal bool ActionScript3
        {
            get
            {
                return ((this._flags & 0x10) != 0);
            }
            set
            {
                this._flags = (value) ? this._flags | 0x10 : this._flags & 0xFFFFFFEF;
            }
        }
        internal bool SuppressCrossDomainCaching
        {
            get
            {
                return ((this._flags & 0x20) != 0);
            }
            set
            {
                this._flags = (value) ? this._flags | 0x20 : this._flags & 0xFFFFFFDF;
            }
        }
        internal bool SwfRelativeUrls
        {
            get
            {
                return ((this._flags & 0x40) != 0);
            }
            set
            {
                this._flags = (value) ? this._flags | 0x40 : this._flags & 0xFFFFFFBF;
            }
        }
		internal bool UseNetwork
		{
			get
			{
				return ((this._flags & 0x80) != 0);
			}
			set
			{
                this._flags = (value) ? this._flags | 0x80 : this._flags & 0xFFFFFF7F;
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			w.AppendTagIDAndLength(this.TagType, len, false);
			w.AppendUI32(this._flags);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            //w.Write("[" + SwfCompilationUnit.TagTypeString(tagType) + "]    " + len + "       FILEATTRIBUTES");
            string __dump = "FILEATTRIBUTES";
            if (this.UseDirectBlit)
			{
                __dump += " UseDirectBlit";
			}
            if (this.UseGPU)
            {
                __dump += " UseGPU";
            }
            if (this.HasMetadata)
            {
                __dump += " HasMetadata";
            }
            if (this.ActionScript3)
            {
                __dump += " as3";
            }
            if (this.SuppressCrossDomainCaching)
            {
                __dump += " SuppressCrossDomainCaching";
            }
            if (this.SwfRelativeUrls)
            {
                __dump += " SwfRelativeUrls";
            }
		    __dump += (this.UseNetwork) ? " useNetwork" : " useLocal";
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, 4, __dump));
        }
#endif
    }
}
