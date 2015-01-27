using System;
using System.IO;

namespace LeagueSharp.GameFiles
{
    class LastVersions
    {
        const string _last_versions = "last_versions";
        static Version _launcher;
        /// <summary>
        /// Return the Launcher Version from last_versions file
        /// </summary>
        static public Version Launcher
        {
            get
            {
                if (_launcher == null) { Set(); }
                return _launcher;
            }
        }
        static Version _airClient;
        /// <summary>
        /// Return the Air Client Version from last_versions file
        /// </summary>
        static public Version AirClient
        {
            get
            {
                if (_airClient == null) { Set(); }
                return _airClient;
            }
        }
        static Version _airClientConfig;
        /// <summary>
        /// Return the Air Client Config Version from last_versions file
        /// </summary>
        static public Version AirClientConfig
        {
            get
            {
                if (_airClientConfig == null) { Set(); }
                return _airClientConfig;
            }
        }
        static Version _gameClientSolution;
        /// <summary>
        /// Return the Game Client Solution Version from last_versions file
        /// </summary>
        static public Version GameClientSolution
        {
            get
            {
                if (_gameClientSolution == null) { Set(); }
                return _gameClientSolution;
            }
        }
        static private void Set()
        {
            string __verFile = Path.Combine(Directories.RadsFolder, _last_versions);
            if (File.Exists(__verFile))
            {
                byte[] verBytes = File.ReadAllBytes(__verFile);
                if (verBytes.Length > 15)
                {
                    _launcher = new Version(verBytes[3], verBytes[2], verBytes[1], verBytes[0]);
                    _airClient = new Version(verBytes[7], verBytes[6], verBytes[5], verBytes[4]);
                    _airClientConfig = new Version(verBytes[11], verBytes[10], verBytes[9], verBytes[8]);
                    _gameClientSolution = new Version(verBytes[15], verBytes[14], verBytes[13], verBytes[12]);
                }
            }
        }
    }
}
