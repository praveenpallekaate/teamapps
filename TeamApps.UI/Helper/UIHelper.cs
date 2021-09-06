using System;
using System.Linq;
using ChartJs.Blazor.Util;

namespace TeamApps.UI
{
    /// <summary>
    /// UI Helper
    /// </summary>
    public class UIHelper
    {
        private static readonly string[] _set1 = new string[] { "a", "p", "q", "k", "d", "j", "w" };
        private static readonly string[] _set2 = new string[] { "g", "n", "y", "u", "b", "x" };
        private static readonly string[] _set3 = new string[] { "s", "o", "v", "c", "i", "z", "r" };
        private static readonly string[] _set4 = new string[] { "e", "h", "t", "m", "l", "f" };

        /// <summary>
        /// Gets chart colors
        /// </summary>
        public static string[] GlobalChartColors
        {
            get
            {
                return new string[]
                {
                    ColorUtil.ColorHexString(255, 99, 132),
                    ColorUtil.ColorHexString(235, 169, 89),
                    ColorUtil.ColorHexString(75, 192, 192),
                    ColorUtil.ColorHexString(27, 137, 214),
                    ColorUtil.ColorHexString(240, 143, 192),
                    ColorUtil.ColorHexString(255, 188, 188),
                    ColorUtil.ColorHexString(75, 101, 135),
                    ColorUtil.ColorHexString(255, 198, 0),
                    ColorUtil.ColorHexString(46, 81, 194),
                    ColorUtil.ColorHexString(247, 246, 242),
                    ColorUtil.ColorHexString(0, 76, 26),
                    ColorUtil.ColorHexString(108, 70, 207),
                    ColorUtil.ColorHexString(240, 229, 207),
                    ColorUtil.ColorHexString(131, 81, 81),
                    ColorUtil.ColorHexString(200, 198, 198),
                };
            }
        }

        /// <summary>
        /// Get the class to be applied for user avatar
        /// </summary>
        /// <param name="name">Name for avatar color</param>
        /// <param name="isAppHeader">Is avatar in header</param>
        /// <returns>Classes to be applied</returns>
        public static string GetAvatarClass(string name, bool isAppHeader = false)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                if (_set1.Contains(name.ToLower()[0].ToString()))
                {
                    result += " set1";
                }
                else if (_set2.Contains(name.ToLower()[0].ToString()))
                {
                    result += " set2";
                }
                else if (_set3.Contains(name.ToLower()[0].ToString()))
                {
                    result += " set3";
                }
                else
                {
                    result += " set4";
                }
            }

            return result;
        }
    }
}
