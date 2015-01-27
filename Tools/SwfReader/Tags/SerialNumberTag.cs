using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class SerialNumberTag : ISwfTag
	{
        /*
		    Header              RECORDHEADER                Tag type = 0x29
            Product             I32                         
            Edition             I32                         
            CharacterEndFlag    UI8                         
            Actions             ACTIONRECORD[zero or more]  Actions to perform
            ActionEndFlag       UI8                         Must be 0
         */
        int Product;
        int Edition;
        uint MajorVersion;
        uint MinorVersion;
        Int64 Build;
        Int64 CompilationDate;
        internal SerialNumberTag(SwfReader r)
            : base(TagType.SerialNumber)
        {
            Product = r.GetInt32();
            Edition = r.GetInt32();
            MajorVersion = r.GetByte();
            MinorVersion = r.GetByte();
            Build = r.GetInt64();
            CompilationDate = r.GetInt64();
		}
#if SWFWRITER
        internal override void ToSwf(SwfWriter w)
        {
            return; // NOT YET HANDLED
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);
            w.AppendByte((byte)MajorVersion);
            w.AppendByte((byte)MinorVersion);
            w.AppendInt32(Product);
            w.AppendInt32(Edition);
            //w.AppendBytes(build);
            //w.AppendBytes(compilationDate);
            w.ResetLongTagLength(this.TagType, start, true);
        }
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
        {
            string __dump = "SERIALNUMBER { Product: " + Product + ",Edition: " + Edition + ", Version: " + MajorVersion.ToString() + "." + MinorVersion.ToString() + ", Build : " + Build.ToString() + ", CompilationDate : " + CompilationDate.ToString() + " }";
            w.WriteLine(SwfCompilationUnit.DumpWrite(tagType, 26, __dump));
        }
#endif
    }
}
