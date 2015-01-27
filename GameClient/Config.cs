using LeagueSharp.GameFiles.Tools;
using System.IO;

namespace LeagueSharp.GameFiles.GameClient
{
    public class Config
    {
        const string _hudcfg = @"DATA\menu\hud";
        private static int _LastWidth = 0;
        private static int _LastHeight = 0;
        private static ConfigReader _HudConfig;
        public static ConfigReader HudConfig
        {
            get
            {
                if (_HudConfig == null || _LastWidth != Drawing.Width || _LastHeight != Drawing.Height)
                {
                    string __filename = "hud" + Drawing.Width + "x" + Drawing.Height + ".ini";
                    string __filepath = Path.Combine(Directories.GameClientSolution, _hudcfg, __filename);
                    _HudConfig = new ConfigReader(__filepath);
                    _LastWidth = Drawing.Width;
                    _LastHeight = Drawing.Height;
                }
                return _HudConfig;
            }
        }
        public static float GlobalScale
        {
            get 
            {
                return HudConfig["Globals", "GlobalScale"].ToFloat(1.0F);
            }
        }
        public static float MinimapScale
        {
            get
            {
                return HudConfig["Globals", "MinimapScale"].ToFloat(1.0F);
            }
        }
        public static float GlobalScaleReplay
        {
            get
            {
                return HudConfig["Globals", "GlobalScaleReplay"].ToFloat(1.0F);
            }
        }
    }
}
