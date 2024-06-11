namespace RTWLib_CLI.input;

using System;
using RTWLib_CLI.draw;

public class InputDialog
{
    private readonly string message;

    public InputDialog(string message)
    {
        this.message = message;
    }

    private void Display()
    {
        Console.WriteLine(this.message.ApplyBorder('@', 1, 2));
    }

    public int GetIntInput(Func<int, bool> conditional)
    {
        this.Display();
        string input;
        int num = -1;

        while (!conditional(num))
        {
            input = Console.ReadLine();
            if (!int.TryParse(input, out num))
            {
                num = -1;
            }

            if (!conditional(num))
            {
                Console.WriteLine("invalid input");
            }
        }
        return num;
    }


}
