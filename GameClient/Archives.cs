using LeagueSharp.GameFiles.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace LeagueSharp.GameFiles.GameClient
{
    public static class Archives
    {
        static Dictionary<string, ArchiveFileVersions> _archiveFilesVersions;
        static ArchiveFiles _archiveFiles = new ArchiveFiles();
        static ArchiveFileEntries _archiveFileEntries = new ArchiveFileEntries();
        /// <summary>
        /// archive entries seen in the game archive folder (with version)
        /// </summary>
        static public Dictionary<string, ArchiveFileVersions> Files
        {
            get
            {
                _PopulateIfNeeded();
                return _archiveFilesVersions;
            }
        }
        /// <summary>
        /// Reset Event used to avoid multiple populate
        /// </summary>
        static private ManualResetEvent _populatedEvent;
        /// <summary>
        /// Allow multi thread access without re populate
        /// </summary>
        static private void _PopulateIfNeeded()
        {
            if (_populatedEvent != null) _populatedEvent.WaitOne();
            if (_archiveFilesVersions == null)
            {
                _populatedEvent = new ManualResetEvent(false);
                _Populate();
                _populatedEvent.Set();
                _populatedEvent.Close();
                _populatedEvent.Dispose();
                _populatedEvent = null;
            }
        }
        /// <summary>
        /// Archives parsed count
        /// </summary>
        static public int ArchivesCount { get; private set; }
        /// <summary>
        /// Entries seen in all the archive files
        /// </summary>
        static public long ArchivesEntries
        {
            get { return _archiveFileEntries.Count; }
        }
        /// <summary>
        /// Populate all the archive entries from the game archive folder
        /// </summary>
        static private void _Populate()
        {
            _archiveFilesVersions = new Dictionary<string, ArchiveFileVersions>();
            if (Directory.Exists(Directories.ProjectGameArchivesFolder))
            {
                _archiveFiles = new ArchiveFiles();

                bool __mutextCreated = false;
                Mutex __mutex = new Mutex(true, "LeagueSharpArchives", out __mutextCreated);
                // Wait until it is safe to enter.
                __mutex.WaitOne();
				Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LeagueSharp", "GameArchiveDb"), true);
				Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LeagueSharp", "GameArchiveDb"));
                _archiveFiles.DeSerialize(_dbPath + ".list");
                _archiveFileEntries.DeSerialize(_dbPath + ".dat");
                if (__mutextCreated)
                {
                    List<string> rafFilePaths = _GetArchiveFiles(Directories.ProjectGameArchivesFolder);
                    ArchivesCount = rafFilePaths.Count;
                    bool __modified = false;
                    foreach (string path in rafFilePaths)
                    {
                        if (!_archiveFiles.Contains(path.ToLowerInvariant()))
                        {
                            __modified = true;
                            ArchiveReader raf = new ArchiveReader(path);
                            _archiveFileEntries.AddRange(raf.FileDictFull.Values);
                            _archiveFiles.Add(path.ToLowerInvariant());
                        }
                    }
                    foreach (string path in _archiveFiles)
                    {
                        if (!rafFilePaths.Contains(path.ToLowerInvariant()))
                        {
                            __modified = true;
                            foreach(ArchiveFileListEntry entry in _archiveFileEntries.ToArray())
                            {
                                if (!File.Exists(entry.RAFFileDataPath))
                                {
                                    _archiveFileEntries.Remove(entry);
                                }
                            }
                            _archiveFiles.Remove(path.ToLowerInvariant());
                        }
                    }
                    if (__modified)
                    {
                        _archiveFiles.Serialize(_dbPath + ".list");
                        _archiveFileEntries.Serialize(_dbPath + ".dat");
                    }
                }
                __mutex.ReleaseMutex();
                for (int __i = 0; __i < _archiveFileEntries.Count; __i++)
                {
                    string __filename = _archiveFileEntries[__i].FileName.ToLowerInvariant();
                    if (!_archiveFilesVersions.ContainsKey(__filename))
                    {
                        _archiveFilesVersions.Add(__filename, new ArchiveFileVersions());
                    }
                    _archiveFilesVersions[__filename].Add(__i);
                }
            }
        }

        private static string _dbPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LeagueSharp", "GameArchiveDb", _GetHash(Directories.ProjectGameArchivesFolder));
            }
        }
        private static string _GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            byte[] __hashed = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte __b in __hashed)
                sb.Append(__b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// Searches each folder inside the base directory for .raf files, ignoring any sub-directories
        /// </summary>
        /// <param name="baseDir">The path to RADS\projects\lol_game_client\filearchives</param>
        /// <returns></returns>
        static private List<string> _GetArchiveFiles(string baseDir)
        {
            string[] folders = Directory.GetDirectories(baseDir);
            List<string> returnFiles = new List<string>();
            foreach (string folder in folders)
            {
                string[] __files = Directory.GetFiles(folder, "*.raf", SearchOption.TopDirectoryOnly);
                foreach (string __file in __files)
                {
                    returnFiles.Add(__file.ToLowerInvariant());
                }
            }
            return returnFiles;
        }

        /// <summary>
        /// A List that countain all the versions of an ArchiveFileListEntry
        /// </summary>
        public class ArchiveFileVersions : List<int>
        {
            /// <summary>
            /// Get the last file entry content (may be compressed)
            /// </summary>
            /// <returns>byte[] from the last version</returns>
            public byte[] GetLastRawContent()
            {
                int __index = GetLastVersionIndex();
                if (__index > -1)
                {
                    return _archiveFileEntries[__index].GetRawContent();
                }
                return null;
            }
            /// <summary>
            /// Get the last file entry content (uncompressed if needed)
            /// </summary>
            /// <returns>byte[] from the last version</returns>
            public byte[] GetLastContent()
            {
                int __index = GetLastVersionIndex();
                if (__index > -1)
                {
                    return _archiveFileEntries[__index].GetContent();
                }
                return null;
            }
            private int GetLastVersionIndex()
            {
                if (this.Count > 1)
                {
                    int __lastindex = 0;
                    Version __lastver = new Version();
                    foreach (int __index in this)
                    {
                        if (_archiveFileEntries[__index].version > __lastver)
                        {
                            __lastindex = __index;
                            __lastver = _archiveFileEntries[__index].version;
                        }
                    }
                    return __lastindex;
                }
                else
                {
                    return this[0];
                }
            }
            public ArchiveFileListEntry LastVersion
            {
                get
                {
                    return _archiveFileEntries[GetLastVersionIndex()];
                }
            }
        }

        public class ArchiveFileEntries : List<ArchiveFileListEntry>
        {
            public void Serialize(string filename)
            {
                using (FileStream __fs = new FileStream(filename, FileMode.Create))
                {
                    using(BinaryWriter __bw = new BinaryWriter(__fs))
                    {
                        foreach(ArchiveFileListEntry __entry in this)
                        {
                            __bw.Write(__entry.RAFFileDataPath);
                            __bw.Write(__entry.version.ToString());
                            __bw.Write(__entry.fileOffset);
                            __bw.Write(__entry.fileSize);
                            __bw.Write(__entry.FileName);
                        }
                    }
                }
            }
            public void DeSerialize(string filename)
            {
                if (File.Exists(filename))
                {
                    using (FileStream __fs = new FileStream(filename, FileMode.Open))
                    {
                        using (BinaryReader __br = new BinaryReader(__fs))
                        {
                            while (__br.BaseStream.Length > __br.BaseStream.Position)
                            {
                                this.Add(new ArchiveFileListEntry(__br.ReadString(), new Version(__br.ReadString()), __br.ReadUInt32(), __br.ReadUInt32(), __br.ReadString()));
                            }
                        }
                    }
                }
            }
        }
    
        public class ArchiveFiles : List<string>
        {
            public void Serialize(string filename)
            {
                using (FileStream __fs = new FileStream(filename, FileMode.Create))
                {
                    using (BinaryWriter __bw = new BinaryWriter(__fs))
                    {
                        foreach (string __entry in this)
                        {
                            __bw.Write(__entry);
                        }
                    }
                }
            }
            public void DeSerialize(string filename)
            {
                if (File.Exists(filename))
                {
                    using (FileStream __fs = new FileStream(filename, FileMode.Open))
                    {
                        using (BinaryReader __br = new BinaryReader(__fs))
                        {
                            while (__br.BaseStream.Length > __br.BaseStream.Position)
                            {
                                this.Add(__br.ReadString());
                            }
                        }
                    }
                }
            }
        }
    }
}
