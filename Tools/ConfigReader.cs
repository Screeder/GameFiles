using System.Collections.Generic;
using System.IO;

namespace LeagueSharp.GameFiles.Tools
{
    public class ConfigReader
    {
        public ConfigReader(string filename)
        {
            this._filename = filename;
            Update();
        }
        public ConfigReader(string[] Lines)
        {
            _Update(Lines);
        }
        string _filename;
        private Dictionary<string, Dictionary<string, string>> _configCategories;
        private void _Update(string[] Lines)
        {
            _configCategories = new Dictionary<string, Dictionary<string, string>>();
            _AddCategory("");
            string __category = "";
            foreach (string __line in Lines)
            {
                if (!string.IsNullOrWhiteSpace(__line))
                {
                    if (__line.StartsWith("["))
                    {
                        __category = __line.Trim().Replace("[", "").Replace("]", "");
                        _AddCategory(__category);
                    }
                    else
                    {
                        string[] __data = __line.Split(new char[] { '=' }, 2);
                        if (__data.Length == 2)
                        {
                            __data[0] = __data[0].Trim();
                            __data[1] = __data[1].Trim();
                            _configCategories[__category][__data[0]] = __data[1];
                        }
                    }
                }
            }
        }
        public void Update()
        {
            if (!string.IsNullOrEmpty(_filename))
            {
                if (File.Exists(_filename))
                {
                    _Update(File.ReadAllLines(_filename));
                }
                else
                {
                    _configCategories = new Dictionary<string, Dictionary<string, string>>();
                    _AddCategory("");
                }
            }
        }
        private void _AddCategory(string category)
        {
            if (!_configCategories.ContainsKey(category))
            {
                _configCategories.Add(category, new Dictionary<string, string>());
            }
        }
        public string this[string entry]
        {
            get
            {
                foreach (Dictionary<string, string> __dict in _configCategories.Values)
                {
                    if (__dict.ContainsKey(entry))
                    {
                        return __dict[entry];
                    }
                }
                return string.Empty;
            }
        }
        public string this[string category, string entry]
        {
            get
            {
                if (_configCategories.ContainsKey(category) && _configCategories[category].ContainsKey(entry))
                {
                    return _configCategories[category][entry];
                }
                return string.Empty;
            }
        }
    
        public IEnumerable<string> Categories
        {
            get { return _configCategories.Keys; }
        }

        public IEnumerable<string> CategoryKeys(string Category)
        {
            if (_configCategories.ContainsKey(Category))
            {
                return _configCategories[Category].Keys;
            }
            return null;
        }
    }
}
