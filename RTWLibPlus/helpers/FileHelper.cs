using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class RTWFileHelper
    {
        public static void Write(string path, string content)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

    }
}
