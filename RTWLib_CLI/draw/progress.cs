namespace RTWLib_CLI.draw;

using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Numerics;

public class Progress
{
    public static List<Progress> activeBars = [];
    private readonly string task;
    private readonly float max = 25;
    private readonly float increment = 1;
    private float current;
    private Vector2 cursorpos;
    private int offset = 1;
    private readonly int barInd;
    public Progress(float increment, string task)
    {
        this.increment = this.max * increment;
        this.task = task;
        Console.WriteLine();
        this.cursorpos = new Vector2(Console.CursorTop, Console.CursorLeft);
        Console.WriteLine();

        activeBars.Add(this);
        this.barInd = activeBars.Count - 1;
        this.Adjust(2);
        this.Draw();
    }

    public void Message(string message = "")
    {
        this.Adjust(1);
        this.offset += 1;
        Console.SetCursorPosition((int)this.cursorpos.Y, (int)this.cursorpos.X + this.offset);
        Console.WriteLine(Format.GetWhiteSpace("", this.barInd, '\t') + message);
    }

    public void Update(string message = "")
    {
        if (message != string.Empty)
        {
            this.Message(message);
        }

        this.current += this.increment;
        this.Draw();
    }

    private void Draw()
    {

        string blocks = string.Empty;
        for (int i = 0; i < this.current; i++)
        {
            blocks += "=";
        }
        Console.SetCursorPosition((int)this.cursorpos.Y, (int)this.cursorpos.X);
        Console.Write(string.Format("{0}{1} [{2}{3}]...{4}%", Format.GetWhiteSpace("", this.barInd, '\t'), this.task, blocks, Format.GetWhiteSpace("", (int)(this.max - this.current), ' '), ((this.current / this.max) * 100).ToString("0")));
        Console.SetCursorPosition((int)this.cursorpos.Y, (int)this.cursorpos.X + this.offset + 1);

        if (this.current == this.max)
        {
            activeBars.RemoveAt(this.barInd);
            Console.WriteLine();
            this.Adjust(1);
        }
    }

    protected void Adjust(int by)
    {
        if (this.barInd > 0)
        {
            for (int i = this.barInd - 1; i >= 0; i--)
            {
                activeBars[i].offset += by;
            }


        }
    }
}
