namespace RTWLibPlus.randomiser;
using System;
using System.Security.Cryptography;
using System.Text;

public class RandWrap
{
    private string seed;

    public RandWrap(string seed) => this.SetRndSeed(seed);

    public void SetRndSeed(string seed = "")
    {
        this.seed = seed;
        if (this.seed == "-")
        {

            byte[] randomNumber = new byte[4]; // 4 bytes for a 32-bit integer
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            int rnd_seed = BitConverter.ToInt32(randomNumber);
            this.seed = Convert.ToString(rnd_seed);
        }

        int rnd_hash = BitConverter.ToInt32(SHA1.HashData(Encoding.UTF8.GetBytes(this.seed)));
        RND = new Random(rnd_hash);
        //Console.WriteLine("Using Seed: {0} (hashed from {1})", seed, rnd_hash);
    }

    public void RefreshRndSeed() => this.SetRndSeed(this.seed);

    public static int Rint(int min, int max) => RND.Next(min, max);

    public static Random RND { get; private set; } = new();

    public string GetSeed => this.seed;

}
