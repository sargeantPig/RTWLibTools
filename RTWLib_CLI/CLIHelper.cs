namespace RTWLib_CLI;

using System;

public static class CLIHelper
{
    public static void ScreenChange(string title)
    {
        //Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
    }
}
