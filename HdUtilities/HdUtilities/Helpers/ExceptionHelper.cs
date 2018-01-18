using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace HdUtilities.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetLineNumberFromException(Exception exception)
        {
            StackTrace stackTrace = new StackTrace(exception, true);
            int fileLineNumber = stackTrace.GetFrame(stackTrace.FrameCount - 1).GetFileLineNumber();
            if (fileLineNumber == 0)
                return "- Line [.PDB File Missing]";
            return "- Line [" + fileLineNumber.ToString() + "]";
        }

        public static Color ConvertRGBStringToColor(string rgbString)
        {
            try
            {
                if (rgbString == "null" || rgbString.Length == 0)
                    return Color.Transparent;
                if (rgbString.StartsWith("rgba"))
                {
                    string[] strArray = rgbString.Substring(5, rgbString.Length - 6).Split(',');
                    return Color.FromArgb(Convert.ToInt32(Convert.ToDouble(strArray[3]) * (double)byte.MaxValue), Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                }
                string[] strArray1 = rgbString.Substring(4, rgbString.Length - 5).Split(',');
                return Color.FromArgb(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]), Convert.ToInt32(strArray1[2]));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name + " --> Color value passed in [" + rgbString + "]:" + Environment.NewLine, ex);
            }
        }

        public static string ConvertColorToHex(Color c)
        {
            string str1 = "#";
            byte num = c.R;
            string str2 = num.ToString("X2");
            num = c.G;
            string str3 = num.ToString("X2");
            num = c.B;
            string str4 = num.ToString("X2");
            return str1 + str2 + str3 + str4;
        }
    }
}