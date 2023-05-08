using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.map
{
    public static class Voronoi
    {
        public static Vector2[] GetVoronoiPoints(int amount, int width, int height)
        {
            TWRand.RefreshRndSeed();
            Vector2[] points = new Vector2[amount];
            for(int i = 0; i < amount; i++)
            {
                points[i] = new Vector2(TWRand.Rint(0, width), TWRand.Rint(0, height));
            }

            return points;
        }

        public static List<string[]> GetVoronoiGroups(Dictionary<string, Vector2> coords, Vector2[] points)
        {
            List<string[]> groups = new List<string[]>();

            groups.init(points.Length);

            foreach(KeyValuePair<string, Vector2> c in coords)
            {
                int closest = GetClosestPoint(points, c.Value);
                groups[closest] = groups[closest].Add(c.Key);
            }

            return groups;

        }

        public static int GetClosestPoint(Vector2[] points, Vector2 coord) {
            double distance = float.MaxValue;
            int index = 0;
            for(int i = 0; i < points.Length; i++)
            {
                double tempDis = coord.DistanceTo(points[i]);
                if (tempDis < distance) {
                    distance = tempDis;
                    index = i;
                }
            }

            return index;
        }
    }
}
