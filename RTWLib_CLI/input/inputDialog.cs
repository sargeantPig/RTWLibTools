using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RTWLib_CLI.draw;

namespace RTWLib_CLI.input
{
    public class InputDialog
    {

        string message;

        public InputDialog(string message)
        {
            this.message = message;
        }

        private void Display()
        {
            Console.WriteLine(message.ApplyBorder('@', 1, 2));
        }

        public int GetIntInput(Func<int, bool> conditional)
        {
            Display();
            string input;
            int num = -1;

            while (!conditional(num))
            {
                input = Console.ReadLine();
                if (!int.TryParse(input, out num))
                    num = -1;

                if (!conditional(num))
                    Console.WriteLine("invalid input");

            }
            return num;
        }


    }
}
