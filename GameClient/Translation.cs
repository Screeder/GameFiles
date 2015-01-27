using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LeagueSharp.GameFiles.GameClient
{
    static public class Translation
    {
        const string _fontconfig = "data/menu/fontconfig_";
        const string _extension = ".txt";
        const string _universal = "en_us";
        static Dictionary<string, string> _translations;
        static Dictionary<string, string> _currentTranslations;
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
            if (_translations == null)
            {
                _populatedEvent = new ManualResetEvent(false);
                _Populate(ref _translations, _fontconfig + _universal + _extension);
                _Populate(ref _currentTranslations, _fontconfig + GameSystem.Config.locale + _extension);
                _populatedEvent.Set();
                _populatedEvent.Close();
                _populatedEvent.Dispose();
                _populatedEvent = null;
            }
        }

        /// <summary>
        /// Populate all the archive entries from the translation file
        /// </summary>
        static private void _Populate(ref Dictionary<string, string> target, string archiveFile)
        {
            if (Archives.Files.ContainsKey(archiveFile))
            {
                target = new Dictionary<string, string>();
                using (var __stream = new System.IO.StreamReader(new System.IO.MemoryStream(Archives.Files[archiveFile].GetLastContent())))
                {
                    string __line = __stream.ReadLine();
                    while (__line != null)
                    {
                        if (__line.StartsWith("tr "))
                        {
                            string[] __keyValue = __line.Split(new char[] { '=' }, 2);
                            target[_GetBracketString(__keyValue[0])] = _GetBracketString(__keyValue[1]);
                        }
                        __line = __stream.ReadLine();
                    }
                }
            }
        }

        static private string _GetBracketString(string value)
        {
            if (value.Contains('"'))
            {
                value = value.Substring(value.IndexOf('"') + 1);
            }
            if (value.Contains('"'))
            {
                value = value.Substring(0, value.IndexOf('"'));
            }
            return value;
        }
        
        /// <summary>
        /// Get the current game localized string
        /// </summary>
        /// <param name="value">entry</param>
        /// <returns>localized entry</returns>
        static public string GetLocale(string value)
        {
            _PopulateIfNeeded();
            if (_currentTranslations != null && _currentTranslations.ContainsKey(value))
            {
                return _currentTranslations[value];
            }
            return GetUniversal(value);
        }

        /// <summary>
        /// Get the en-us localized string
        /// </summary>
        /// <param name="value">entry</param>
        /// <returns>en-us localized entry</returns>
        static public string GetUniversal(string value)
        {
            _PopulateIfNeeded();
            if (_translations != null && _translations.ContainsKey(value))
            {
                return _translations[value];
            }
            return value;
        }
    }
}
