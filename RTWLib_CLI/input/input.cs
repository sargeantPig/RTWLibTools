namespace RTWLib_CLI.input;

using System;

public static class Input
{

    public static int GetIntInput(string message, Func<int, bool> conditional)
    {
        InputDialog id = new InputDialog(message);
        return id.GetIntInput(conditional);
    }

}
