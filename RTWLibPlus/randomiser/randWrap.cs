using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RTWLibPlus.randomiser
{
    public class RandWrap
    {
        Random rnd = new Random();
        string seed;

        public RandWrap(string seed)
        {
            RefreshRndSeed(seed);
        }

        public void RefreshRndSeed(string seed)
        {
            this.seed = seed;
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)));
            rnd = new Random(hash);
        }

        public void RefreshRndSeed()
        {
            RefreshRndSeed(seed);
        }

        public int Rint(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public Random RND
        {
            get { return rnd; }
        }

    }
}
