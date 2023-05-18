using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_CLI
{
    public static class CLIHelper
    {
        public static void ScreenChange(string title)
        { 
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();
        }
    }
}
