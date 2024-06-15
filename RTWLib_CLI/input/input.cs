namespace RTWLib_CLI.input;

using System;

public static class Input
{

    public static int GetIntInput(string message, Func<int, bool> conditional)
    {
        InputDialog id = new InputDialog(message);
        return id.GetIntInput(conditional);
    }

    public static string GetStringInput(string message, Func<string, bool> conditional)
    {
        InputDialog id = new InputDialog(message);
        return id.GetStringInput(conditional);
    }

}
