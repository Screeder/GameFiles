
namespace LeagueSharp.GameFiles
{
    static class GameFiles
    {
        /// <summary>
        /// Extend string to return a double
        /// </summary>
        /// <param name="value">default value if parse fail</param>
        /// <returns>double from string or default value</returns>
        public static double ToDouble(this string entry, double value)
        {
            double __return = value;
            if (entry != string.Empty)
            {
                double.TryParse(entry, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out __return);
            }
            return __return;
        }
        /// <summary>
        /// Extend string to return a float
        /// </summary>
        /// <param name="value">default value if parse fail</param>
        /// <returns>float from string or default value</returns>
        public static float ToFloat(this string entry, float value)
        {
            float __return = value;
            if (entry != string.Empty)
            {
                float.TryParse(entry, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out __return);
            }
            return __return;
        }
    }
}
