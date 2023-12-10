using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLib_CLI.draw
{
    public class Progress
    {
        public static List<Progress> activeBars = new List<Progress>();

        string task;
        float max = 25;
        float increment = 1;
        float current = 0;
        Vector2 cursorpos;

        int offset = 1;
        int barInd = 0;
        public Progress(float increment, string task)
        {
            this.increment = max * increment;
            this.task = task;
            Console.WriteLine();
            cursorpos = new Vector2(Console.CursorTop, Console.CursorLeft);
            Console.WriteLine();

            activeBars.Add(this);
            barInd = activeBars.Count - 1;
            Adjust(2);
            Draw();
        }

        public void Message(string message = "")
        {
            Adjust(1);
            offset += 1;
            Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X + offset);
            Console.WriteLine(Format.GetWhiteSpace("", barInd, '\t') + message);
        }

        public void Update(string message = "")
        {
            if (message != string.Empty)
            {
                Message(message);
            }

            current += increment;
            Draw();
        }

        private void Draw()
        {

            string blocks = string.Empty;
            for (int i = 0; i < current; i++)
            {
                blocks += "=";
            }
            Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X);
            Console.Write(string.Format("{0}{1} [{2}{3}]...{4}%", Format.GetWhiteSpace("", barInd, '\t'), task, blocks, Format.GetWhiteSpace("", (int)(max - current), ' '), ((current / max) * 100).ToString("0")));
            Console.SetCursorPosition((int)cursorpos.Y, (int)cursorpos.X + offset + 1);

            if (current == max)
            {
                activeBars.RemoveAt(barInd);
                Console.WriteLine();
                Adjust(1);
            }
        }

        protected void Adjust(int by)
        {
            if (barInd > 0)
            {
                for (int i = barInd - 1; i >= 0; i--)
                {
                    activeBars[i].offset += by;
                }


            }
        }
    }
}
