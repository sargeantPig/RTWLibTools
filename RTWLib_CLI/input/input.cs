using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_CLI.input
{
    public static class Input
    {

        public static int GetIntInput(string message, Func<int, bool> conditional)
        {
            InputDialog id = new InputDialog(message);
            return id.GetIntInput(conditional);
        } 

    }
}
