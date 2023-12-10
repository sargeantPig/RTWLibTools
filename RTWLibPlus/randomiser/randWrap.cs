namespace RTWLibPlus.randomiser;
using System;
using System.Security.Cryptography;
using System.Text;

public class RandWrap
{
    private string seed;

    public RandWrap(string seed) => this.RefreshRndSeed(seed);

    public void RefreshRndSeed(string seed)
    {
        this.seed = seed;
        int hash = BitConverter.ToInt32(SHA1.HashData(Encoding.UTF8.GetBytes(seed)));
        this.RND = new Random(hash);
    }

    public void RefreshRndSeed() => this.RefreshRndSeed(this.seed);

    public int Rint(int min, int max) => this.RND.Next(min, max);

    public Random RND { get; private set; } = new();

}
