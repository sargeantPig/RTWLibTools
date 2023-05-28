using RTWLibPlus.data;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace RTWLibPlus.randomiser
{
    /*public static class TWRand
    {
        public static string seed = "0";
        public static Random rnd;
        public static void RefreshRndSeed()
        {
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)));
            rnd = new Random(hash);
        }

       

        public static int Rint(int min, int max)
        {
            return rnd.Next(min, max);
        }

    }*/
}
