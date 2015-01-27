using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LeagueSharp.GameFiles.Tools
{
    /// <summary>
    /// A class that allows the read of RAF archives
    /// This class is a modification of the original RAFlib from ItzWarty
    /// http://code.google.com/p/raf-manager/source/browse/#svn%2FProjects%2FRAFLib
    /// </summary>
    public class ArchiveReader
    {
        // Path to the archive Structure
        public string RAFFilePath { get; private set; }

        // Path to the archive Data
        public string RAFFileDataPath { get; private set; }

        // Magic value used to identify the file type, must be 0x18BE0EF0
        private UInt32 m_magic;

        // Version of the archive format, must be 1
        private UInt32 m_version;

        // An index of the filetype. Don't modify this
        private UInt32 m_mgrIndex;

        // Dictionary with the full path of the RAF entry as the key
        public Dictionary<String, ArchiveFileListEntry> FileDictFull { get; private set; }

        // Dictionary with just the file name as the key
        //public Dictionary<String, List<RAFFileListEntry>> m_fileDictShort { get; private set; }

        public Version version { get; private set; }

        /// <summary>
        /// A class that allows the easy manipulation of RAF archives
        /// </summary>
        /// <param name="rafPath">Path to the .raf file</param>
        public ArchiveReader(string rafPath)
        {
            RAFFilePath = rafPath;
            string __versionstr = Path.GetFileName(Path.GetDirectoryName(rafPath));
            try
            {
                version = new Version(__versionstr);
            }
            catch { }
            RAFFileDataPath = rafPath + ".dat";
            FileDictFull = new Dictionary<String, ArchiveFileListEntry>();
            FileInfo fi = new FileInfo(RAFFilePath);
            if (fi.Exists && fi.Length > 31)
            {
                loadArchive();
            }
        }

        private void loadArchive()
        {
            using (MemoryStream m_mStream = new MemoryStream(File.ReadAllBytes(RAFFilePath)))
            {
                using (BinaryReader m_bReader = new BinaryReader(m_mStream))
                {
                    m_magic = m_bReader.ReadUInt32();
                    m_version = m_bReader.ReadUInt32();
                    m_mgrIndex = m_bReader.ReadUInt32();
                    // Offset to the table of contents from the start of the file
                    UInt32 offsetFileList = m_bReader.ReadUInt32();
                    // Offset to the string table from the start of the file
                    UInt32 offsetStringTable = m_bReader.ReadUInt32();
                    //UINT32 is casted to INT32.  This should be fine, since i doubt that the RAF will become
                    //a size of 2^31-1 in bytes.
                    createFileDicts(m_bReader, offsetFileList, offsetStringTable);
                }
            }
        }

        #region FileDict functions

        private void createFileDicts(BinaryReader m_bReader, UInt32 offsetFileList, UInt32 offsetStringTable)
        {
            try
            {
                //The file list starts with a uint stating how many files we have
                m_bReader.BaseStream.Seek((Int32)offsetFileList, 0);
                UInt32 fileListCount = m_bReader.ReadUInt32();

                //After the file list count, we have the actual data.
                offsetFileList += 4;

                for (UInt32 currentOffset = offsetFileList; currentOffset < offsetFileList + 16 * fileListCount; currentOffset += 16)
                {
                    ArchiveFileListEntry entry = new ArchiveFileListEntry(this, ref m_bReader, currentOffset, offsetStringTable);
                    this.FileDictFull.Add(entry.FileName.ToLower(), entry);
                    /*
                    FileInfo fi = new FileInfo(entry.FileName);
                    if (!this.m_fileDictShort.ContainsKey(fi.Name.ToLower()))
                        this.m_fileDictShort.Add(fi.Name.ToLower(), new List<RAFFileListEntry> { entry });
                    else
                        this.m_fileDictShort[fi.Name.ToLower()].Add(entry);
                     */
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion // FileDict functions
    }

    /// <summary>
    /// A class that represents a file within an RAF archive
    /// This class is a modification of the original RAFlib from ItzWarty
    /// http://code.google.com/p/raf-manager/source/browse/#svn%2FProjects%2FRAFLib
    /// </summary>
    public class ArchiveFileListEntry
    {
        //private RAFArchive raf = null;
        public Version version { get; private set; }
        public string RAFFileDataPath = null;
        public UInt32 fileOffset { get; private set; }  //It is assumed that LoL archive files will never reach 4 gigs of size.
        public UInt32 fileSize { get; private set; }
        public string FileName { get; private set; }

        /// <summary>
        /// A class that represents a file within an RAF archive
        /// </summary>
        /// <param name="raf">Pointer to the owning RAFArchive</param>
        /// <param name="directoryFileContent">Pointer to the content of the .raf.dat file</param>
        /// <param name="offsetDirectoryEntry">Offset to the entry data offsets</param>
        /// <param name="offsetStringTable">Offset to the entry's file name</param>
        public ArchiveFileListEntry(ArchiveReader raf, ref BinaryReader m_bReader, UInt32 offsetDirectoryEntry, UInt32 offsetStringTable)
        {
            this.RAFFileDataPath = raf.RAFFileDataPath;
            this.version = raf.version;
            m_bReader.BaseStream.Seek((long)offsetDirectoryEntry + 4, 0);
            this.fileOffset = m_bReader.ReadUInt32();
            this.fileSize = m_bReader.ReadUInt32();
            UInt32 strIndex = m_bReader.ReadUInt32();
            UInt32 entryOffset = offsetStringTable + 8 + strIndex * 8;
            m_bReader.BaseStream.Seek((long)entryOffset, 0);
            UInt32 entryValueOffset = m_bReader.ReadUInt32();
            UInt32 entryValueSize = m_bReader.ReadUInt32();
            m_bReader.BaseStream.Seek((long)(entryValueOffset + offsetStringTable), 0);
            this.FileName = Encoding.UTF8.GetString(m_bReader.ReadBytes((int)entryValueSize - 1));
        }
        public ArchiveFileListEntry(string RAFFileDataPath, Version version, uint fileOffset, uint fileSize, string FileName)
        {
            this.RAFFileDataPath = RAFFileDataPath;
            this.version = version;
            this.fileOffset = fileOffset;
            this.fileSize = fileSize;
            this.FileName = FileName;
        }
        /// <summary>
        /// Returns the content of the file
        /// </summary>
        public byte[] GetContent()
        {
            return _GetContent(true);
        }

        /// <summary>
        /// Returns the content of the file
        /// </summary>
        /// <param name="fStream">FileStream to the RAF .dat file</param>
        /// <returns></returns>
        public byte[] GetContent(FileStream fStream)
        {
            return _GetContent(fStream, true);            //Will contain compressed data
        }

        /// <summary>
        /// Returns the raw, still compressed, contents of the file. 
        /// </summary>
        /// <returns></returns>
        public byte[] GetRawContent()
        {
            return _GetContent(false);
        }

        /// <summary>
        /// Returns the raw, still compressed, contents of the file. 
        /// </summary>
        /// <param name="fStream">FileStream to the RAF .dat file</param>
        /// <returns></returns>
        public byte[] GetRawContent(FileStream fStream)
        {
            return _GetContent(fStream, false);
        }
        private byte[] _GetContent(bool uncompress)
        {
            // Open .dat file
            using (FileStream fStream = new FileStream(this.RAFFileDataPath, FileMode.Open))
            {
                return _GetContent(fStream, uncompress);
            }
        }
        private static bool IsZlibMagic(byte[] magic)
        {
            if (magic.Length < 2 || (magic[0] != 0x78 && magic[0] != 0x08 && magic[0] != 0x18 && magic[0] != 0x28 && magic[0] != 0x38 && magic[0] != 0x48 && magic[0] != 0x58 && magic[0] != 0x68))
            {
                return false;
            }
            uint __magic = (uint)((uint)magic[0] << 24) + (uint)((uint)magic[1] << 16);
            if (__magic % 31 != 0)
            {
                return false;
            }
            return true;
        }
        private byte[] _GetContent(FileStream fStream, bool uncompress)
        {
            fStream.Seek(this.fileOffset, SeekOrigin.Begin);
            if (!uncompress)
            {
                byte[] buffer = new byte[this.fileSize];            //Will contain compressed data
                fStream.Read(buffer, 0, (int)this.fileSize);
                return buffer;
            }
            else
            {
                byte[] magic = new byte[2];  //zlib magic number
                fStream.Read(magic, 0, 2);
                //check compressed
                if (IsZlibMagic(magic))
                {
                    //Create the decompressed file.
                    using (MemoryStream decompressed = new MemoryStream())
                    {
                        using (DeflateStream Decompress = new DeflateStream(fStream, CompressionMode.Decompress))
                        {
                            // Copy the decompression stream 
                            // into the output file.
                            Decompress.CopyTo(decompressed);
                            return decompressed.ToArray();
                        }
                    }
                }
                else
                {
                    return _GetContent(fStream, false);
                }
            }
        }
    }
    
}
