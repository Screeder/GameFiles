using System;
using System.IO;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    public class SwfReader : ByteReader
	{
        internal const int twip = 20;
        internal const float twipF = 20.0F;
		private const long _MAX_SIZE = 1000 * 1024;
        internal string FullPath = "";
        internal string DirectoryName = "";
        internal string Name = "Nameless";
        internal SwfCompilationUnit CompilationUnit;
        public SwfReader(string fileName)
        {
            if (File.Exists(fileName))
            {
                FullPath = Path.GetFullPath(fileName);
                DirectoryName = Path.GetDirectoryName(FullPath);
                Directory.SetCurrentDirectory(DirectoryName);
                Name = Path.GetFileNameWithoutExtension(FullPath);
                SetBytes(File.ReadAllBytes(FullPath));
                CompilationUnit = new SwfCompilationUnit(this);
            }
            else
            {
                throw new FileLoadException("This swf file could not be found.");
            }
        }
        internal SwfReader(byte[] rawSwf)
		{
            SetBytes(rawSwf);
            CompilationUnit = new SwfCompilationUnit(this);
		}

        internal void DecompressCWSSwf()
		{
			byte[] __buf = new byte[_bytes.Length - 8];
			int __len = _bytes[4] + _bytes[5] * 0x100 + _bytes[6] * 0x10000 + _bytes[7] * 0x1000000;
			byte[] __unzipped = new byte[__len];
			Array.Copy(_bytes, 0, __unzipped, 0, 8);
			Array.Copy(_bytes, 8, __buf, 0, _bytes.Length - 8);
            __buf = Uncompress(__buf);
            Array.Copy(__buf, 0, __unzipped, 8, __buf.Length);
			__unzipped[0] = (byte)'F';
			_bytes = __unzipped;
		}

        public void DeCompile()
        {
            CompilationUnit.Decompile();
        }
    }
}
