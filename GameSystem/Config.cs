using System.IO;

namespace LeagueSharp.GameFiles.GameSystem
{
    class Config
    {
        const string _launchercfg = "launcher.cfg";
        const string _localecfg = "locale.cfg";
        const string _systemcfg = "system.cfg";

        static string _airConfigProject;
        /// <summary>
        /// Return the AirConfigProject folder (eg : lol_air_client_config_euw) from launcher.cfg
        /// </summary>
        static public string airConfigProject
        {
            get
            {
                if (_airConfigProject == null)
                {
                    SetLauncher();
                }
                return _airConfigProject;
            }
        }
        static string _airProject;
        /// <summary>
        /// Return the AirProject folder (eg : lol_air_client) from launcher.cfg
        /// </summary>
        static public string airProject
        {
            get
            {
                if (_airProject == null)
                {
                    SetLauncher();
                }
                return _airProject;
            }
        }
        static string _gameProject;
        /// <summary>
        /// Return the GameProject folder (eg : lol_game_client_sln) from launcher.cfg
        /// </summary>
        static public string gameProject
        {
            get
            {
                if (_gameProject == null)
                {
                    SetLauncher();
                }
                return _gameProject;
            }
        }
        static string _installation_id;
        /// <summary>
        /// Return the installation_id (eg : xze/Jg==) from launcher.cfg
        /// </summary>
        static public string installation_id
        {
            get
            {
                if (_installation_id == null)
                {
                    SetLauncher();
                }
                return _installation_id;
            }
        }
        
        static string _locale;
        /// <summary>
        /// Return the locale (eg : en_gb) from locale.cfg
        /// </summary>
        static public string locale
        {
            get
            {
                if (_locale == null)
                {
                    SetLocale();
                }
                return _locale;
            }
        }

        static string _DownloadPath;
        /// <summary>
        /// Return the DownloadPath (eg : /releases/live) from system.cfg
        /// </summary>
        static public string DownloadPath
        {
            get
            {
                if (_DownloadPath == null)
                {
                    SetSystem();
                }
                return _DownloadPath;
            }
        }

        static string _DownloadURL;
        /// <summary>
        /// Return the DownloadURL (eg : l3cdn.riotgames.com) from system.cfg
        /// </summary>
        static public string DownloadURL
        {
            get
            {
                if (_DownloadURL == null)
                {
                    SetSystem();
                }
                return _DownloadURL;
            }
        }

        static string _Region;
        /// <summary>
        /// Return the Region (eg : EUW) from system.cfg
        /// </summary>
        static public string Region
        {
            get
            {
                if (_Region == null)
                {
                    SetSystem();
                }
                return _Region;
            }
        }

        static private void SetLauncher()
        {
            string __launcherconf = Path.Combine(Directories.SystemFolder, _launchercfg);
            if (File.Exists(__launcherconf))
            {
                string[] __conf = File.ReadAllLines(__launcherconf);
                foreach(string __line in __conf)
                {
                    string[] __data = __line.Split(new char[] { '=' }, 2);
                    if (__data.Length == 2)
                    {
                        __data[0] = __data[0].Trim();
                        __data[1] = __data[1].Trim();
                        switch (__data[0])
                        {
                            case "airConfigProject":
                                _airConfigProject = __data[1];
                                break;
                            case "airProject":
                                _airProject = __data[1];
                                break;
                            case "gameProject":
                                _gameProject = __data[1];
                                break;
                            case "installation_id":
                                _installation_id = __data[1];
                                break;
                        }
                    }
                }
            }
        }
        static private void SetLocale()
        {
            string __localeconf = Path.Combine(Directories.SystemFolder, _localecfg);
            if (File.Exists(__localeconf))
            {
                string[] __conf = File.ReadAllLines(__localeconf);
                foreach (string __line in __conf)
                {
                    string[] __data = __line.Split(new char[] { '=' }, 2);
                    if (__data.Length == 2)
                    {
                        __data[0] = __data[0].Trim();
                        __data[1] = __data[1].Trim();
                        switch (__data[0])
                        {
                            case "locale":
                                _locale = __data[1];
                                break;
                        }
                    }
                }
            }
        }
        static private void SetSystem()
        {
            string __systemconf = Path.Combine(Directories.SystemFolder, _systemcfg);
            if (File.Exists(__systemconf))
            {
                string[] __conf = File.ReadAllLines(__systemconf);
                foreach (string __line in __conf)
                {
                    string[] __data = __line.Split(new char[] { '=' }, 2);
                    if (__data.Length == 2)
                    {
                        __data[0] = __data[0].Trim();
                        __data[1] = __data[1].Trim();
                        switch (__data[0])
                        {
                            case "DownloadPath":
                                _DownloadPath = __data[1];
                                break;
                            case "DownloadURL":
                                _DownloadURL = __data[1];
                                break;
                            case "Region":
                                _Region = __data[1];
                                break;
                        }
                    }
                }
            }
        }

    }
}
