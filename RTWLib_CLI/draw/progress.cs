using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLib_CLI.draw
{
    public class Progress
    {
        string task;
        float max = 25;
        float increment = 1;
        float current = 0;
        Vector2 cursorpos;

        int offset = 1;

        public Progress(float increment, string task) {
            this.increment = max * increment;
            this.task = task;
            Console.WriteLine();
            cursorpos = new Vector2(Console.CursorTop, Console.CursorLeft);
            Console.WriteLine();
            Draw();
        }

        public void Update(string message = "")
        {
            if (message != string.Empty)
            {
                offset += 1;
                Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X + offset);
                Console.WriteLine(message);
            }

            current += increment;
            Draw();
        }

        private void Draw() {

            string blocks = string.Empty;
            for(int i = 0; i < current; i++)
            {
                blocks += "=";
            }
            Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X);
            Console.Write(string.Format("{0} [{2}{3}]...{1}%", task, ((current/max) *100).ToString("0") , blocks, Format.GetWhiteSpace("", (int)(max - current), ' ')));
            Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X + offset + 1);

        }
    }
}
