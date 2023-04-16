using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class RFH
    {
        public static void Write(string path, string content)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        public static string CurrDirPath(params string[] path) {
            string finpath = Directory.GetCurrentDirectory();
            foreach (string s in path)
            {
                finpath = Path.Combine(finpath, s);
            }
            return finpath;
        }

       

    }
}
