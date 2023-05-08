using RTWLibPlus.data;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RTWLibPlus.randomiser
{
    public static class TWRand
    {
        public static int seed = 0;
        public static Random rnd = new Random();
        public static void RefreshRndSeed()
        {
            TWRand.rnd = new Random(seed);
        }

        public static string[] GetFactionListAndShuffle(int listToFetch)
        {
            var factionList = RemasterRome.GetFactionList(listToFetch);
            factionList.Shuffle(TWRand.rnd);
            return factionList;
        }

        public static int Rint(int min, int max)
        {
            return rnd.Next(min, max);
        }

    }
}
