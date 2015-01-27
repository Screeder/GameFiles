using System;
using System.Diagnostics;
using System.IO;

namespace LeagueSharp.GameFiles
{
    public class Directories
    {
        static string _gameFolder;
        /// <summary>
        /// Return the base Game folder (eg : C:\Riot Games\League of Legends) from processes
        /// </summary>
        static public string GameFolder
        {
            get
            {
                if (_gameFolder == null)
                {
                    foreach (Process __process in Process.GetProcesses())
                    {
                        string __processname = __process.ProcessName.ToLowerInvariant();
                        if (__processname == "league of legends" //game
                            || __processname == "lolpatcher"  //patcher
                            || __processname == "lolclient") //client
                        {
                            string __smallrads = _rads.ToLowerInvariant();
                            string __mainModule = __process.MainModule.FileName.ToLowerInvariant();
                            if (__mainModule.Contains(__smallrads))
                            {
                                _gameFolder = __process.MainModule.FileName.Substring(0, __mainModule.LastIndexOf(__smallrads) - 1);
                                return _gameFolder;
                            }
                        }
                    }
                    throw new System.Exception("League of Legends Folder not founded");
                }
                return _gameFolder;
            }
        }
        const string _rads = "RADS";

        static string _radsFolder;
        /// <summary>
        /// Return the rads Game folder (eg : C:\Riot Games\League of Legends\RADS)
        /// </summary>
        static public string RadsFolder
        {
            get
            {
                if (_radsFolder == null)
                {
                    _radsFolder = Path.Combine(GameFolder, _rads);
                }
                return _radsFolder;
            }
        }

        const string _system = "system";
        static string _systemFolder;
        /// <summary>
        /// Return the system Game folder (eg : C:\Riot Games\League of Legends\RADS\system)
        /// </summary>
        static public string SystemFolder
        {
            get
            {
                if (_systemFolder == null)
                {
                    _systemFolder = Path.Combine(RadsFolder, _system);
                }
                return _systemFolder;
            }
        }

        const string _projects = "projects";

        static string _projectsFolder;
        /// <summary>
        /// Return the projects Game folder (eg : C:\Riot Games\League of Legends\RADS\projects)
        /// </summary>
        static public string ProjectsFolder
        {
            get
            {
                if (_projectsFolder == null)
                {
                    _projectsFolder = Path.Combine(RadsFolder, _projects);
                }
                return _projectsFolder;
            }
        }

        const string _releases = "releases";
        const string _deploy = "deploy";
        const string _assets = "assets";
        const string _swfs = "swfs";
        static string _baseAirClientFolder;
        /// <summary>
        /// Return the base Air Client Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_air_client)
        /// </summary>
        static public string BaseAirClientFolder
        {
            get
            {
                if (_baseAirClientFolder == null)
                {
                    _baseAirClientFolder = Path.Combine(ProjectsFolder, GameSystem.Config.airProject);
                }
                return _baseAirClientFolder;
            }
        }
        static string _airClientFolder;
        /// <summary>
        /// Return the Current Air Client Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_air_client\releases\*.*.*.*\deploy)
        /// </summary>
        static public string AirClientFolder
        {
            get
            {
                if (_airClientFolder == null)
                {
                    _airClientFolder = Path.Combine(BaseAirClientFolder, _releases, LastVersions.AirClient.ToString(), _deploy);
                }
                return _airClientFolder;
            }
        }
        static string _airClientConfigFolder;
        /// <summary>
        /// Return the Current Air Client Config Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_air_client_config_euw\releases\*.*.*.*\deploy)
        /// </summary>
        static public string AirClientConfigFolder
        {
            get
            {
                if (_airClientConfigFolder == null)
                {
                    _airClientConfigFolder = Path.Combine(ProjectsFolder, GameSystem.Config.airConfigProject, _releases, LastVersions.AirClientConfig.ToString(), _deploy);
                }
                return _airClientConfigFolder;
            }
        }

        const string _game_client = "lol_game_client";
        const string _filearchives = "filearchives";

        static string _baseGameClientProject;
        /// <summary>
        /// Return the Base Game Client Project Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_game_client)
        /// </summary>
        static public string BaseGameClientProject
        {
            get
            {
                if (_baseGameClientProject == null)
                {

                    _baseGameClientProject = Path.Combine(ProjectsFolder, _game_client);
                }
                return _baseGameClientProject;
            }
        }

        static string _gameClientReleasesProject;
        /// <summary>
        /// Return the Releases Game Client Project Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_game_client\releases)
        /// </summary>
        static public string GameClientReleasesProject
        {
            get
            {
                if (_gameClientReleasesProject == null)
                {

                    _gameClientReleasesProject = Path.Combine(BaseGameClientProject, _releases);
                }
                return _gameClientReleasesProject;
            }
        }
        static string _latestProjectGameFolder;
        /// <summary>
        /// Return the Current Game Client Project Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_game_client\releases\*.*.*.*\deploy)
        /// </summary>
        static public string LatestProjectGameFolder
        {
            get
            {
                if (_latestProjectGameFolder == null)
                {
                    _latestProjectGameFolder = Path.Combine(GameClientReleasesProject, GetLastVersion(GameClientReleasesProject), _deploy);
                }
                return _latestProjectGameFolder;
            }
        }

        static string _projectGameArchivesFolder;
        /// <summary>
        /// Return the Archives Game Client Project Game folder (eg : C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives)
        /// </summary>
        static public string ProjectGameArchivesFolder
        {
            get
            {
                if (_projectGameArchivesFolder == null)
                {
                    _projectGameArchivesFolder = Path.Combine(BaseGameClientProject, _filearchives);
                }
                return _projectGameArchivesFolder;
            }
        }

        const string _solutions = "solutions";
        static string _solutionsFolder;
        /// <summary>
        /// Return the solutions Game folder (eg : C:\Riot Games\League of Legends\RADS\solutions)
        /// </summary>
        static public string SolutionsFolder
        {
            get
            {
                if (_solutionsFolder == null)
                {
                    _solutionsFolder = Path.Combine(RadsFolder, _solutions);
                }
                return _solutionsFolder;
            }
        }

        static public string _gameClientSolution;
        /// <summary>
        /// Return the Game Client Solution Game folder (eg : C:\Riot Games\League of Legends\RADS\solutions\lol_game_client_sln\releases\*.*.*.*\deploy)
        /// </summary>
        static public string GameClientSolution
        {
            get
            {
                if (_gameClientSolution == null)
                {
                    _gameClientSolution = Path.Combine(SolutionsFolder, GameSystem.Config.gameProject, _releases, LastVersions.GameClientSolution.ToString(), _deploy);
                }
                return _gameClientSolution;
            }
        }

        static private string GetLastVersion(string directory)
        {
            string[] __directories = Directory.GetDirectories(directory);
            Version __version = new Version();
            foreach(string __directory in __directories)
            {
                try
                {
                    Version __tmpVersion = new Version(Path.GetFileName(__directory));
                    if (__tmpVersion > __version)
                    {
                        __version = __tmpVersion;
                    }
                }
                catch
                {
                }
            }
            return __version.ToString();
        }
    }
}
