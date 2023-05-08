using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class exVector
    {
        public static double DistanceTo(this Vector2 a, Vector2 b)
        {
            double x = Math.Pow(b.X - a.X, 2);
            double y = Math.Pow(b.Y - a.Y, 2);
            double xy = x + y;
            return Math.Sqrt(xy);
        }

    }
}
