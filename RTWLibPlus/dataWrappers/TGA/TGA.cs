namespace RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.interfaces;
using System;
using System.IO;
using System.Numerics;

/*Converted from C code  - http://www.paulbourke.net/dataformats/tga/tgatest.c*/
public class TGA : IWrapper
{
    private HEADER Header { get; set; } = new();
    private PIXEL[] pixels { get; set; }

    public TGA(string loadPath, string outputpath)
    {
        this.OutputPath = outputpath;
        this.LoadPath = loadPath;
    }
    public TGA(params string[] args)
    {
        this.Read(args);
        this.LoadPath = args[1];
        if (args.Length > 2)
        { this.OutputPath = args[2]; }

    }
    public TGA() { }

    public TGA Copy(string load, string output)
    {
        TGA copy = new()
        {
            Header = this.Header,
            Pixels = (PIXEL[])this.Pixels.Clone(),
            OutputPath = output,
            LoadPath = load
        };
        return copy;
    }

    public void Parse() => this.Read("tgafile", this.LoadPath);
    public void Clear()
    {
        this.Header = new HEADER();
        this.Pixels = Array.Empty<PIXEL>();
    }

    public void Read(params string[] args)
    {
        int n = 0, i;
        int bytes2read, skipover = 0;
        byte[] p = new byte[5];
        FileStream fptr;

        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: {0} tgafile", args[0]);
            Environment.Exit(-1);
        }

        /* Open the file */
        if ((fptr = File.OpenRead(args[1])) == null)
        {
            Console.Error.WriteLine("File open failed");
            Environment.Exit(-1);
        }

        // Display the Header fields
        this.RefHeader.Idlength = (char)fptr.ReadByte();
        //Console.Error.WriteLine("ID length:         {0}", Header.idlength);
        this.RefHeader.Colourmaptype = (char)fptr.ReadByte();
        //Console.Error.WriteLine("Colourmap type:    {0}", Header.colourmaptype);
        this.RefHeader.Datatypecode = (char)fptr.ReadByte();
        //Console.Error.WriteLine("Image type:        {0}", Header.datatypecode);
        this.RefHeader.Colourmaporigin = ReadShort(fptr);
        // Console.Error.WriteLine("Colour map offset: {0}", Header.colourmaporigin);
        this.RefHeader.Colourmaplength = ReadShort(fptr);
        //Console.Error.WriteLine("Colour map length: {0}", Header.colourmaplength);
        this.RefHeader.Colourmapdepth = (char)fptr.ReadByte();
        //Console.Error.WriteLine("Colour map depth:  {0}", Header.colourmapdepth);
        this.RefHeader.Xorigin = ReadShort(fptr);
        //Console.Error.WriteLine("X origin:          {0}", Header.x_origin);
        this.RefHeader.Yorigin = ReadShort(fptr);
        // Console.Error.WriteLine("Y origin:          {0}", Header.y_origin);
        this.RefHeader.Width = ReadShort(fptr);
        //Console.Error.WriteLine("Width:             {0}", Header.width);
        this.RefHeader.Height = ReadShort(fptr);
        //Console.Error.WriteLine("Height:            {0}", Header.height);
        this.RefHeader.Bitsperpixel = (char)fptr.ReadByte();
        //Console.Error.WriteLine("Bits per pixel:    {0}", Header.bitsperpixel);
        this.RefHeader.Imagedescriptor = (char)fptr.ReadByte();
        //Console.Error.WriteLine("Descriptor:        {0}", Header.imagedescriptor);

        // Allocate space for the image
        this.Pixels = new PIXEL[this.Header.Width * this.Header.Height];
        for (i = 0; i < this.Header.Width * this.Header.Height; i++)
        {
            this.Pixels[i].R = 0;
            this.Pixels[i].G = 0;
            this.Pixels[i].B = 0;
            this.Pixels[i].A = 0;
        }

        // What can we handle
        if (this.Header.Datatypecode is not (char)2 and not (char)10)
        {
            Console.Error.WriteLine("Can only handle image type 2 and 10");
            Environment.Exit(-1);
        }
        if (this.Header.Bitsperpixel is not (char)16 and
            not (char)24 and not (char)32)
        {
            Console.Error.WriteLine("Can only handle pixel depths of 16, 24, and 32");
            Environment.Exit(-1);
        }

        // Skip over unnecessary stuff
        skipover += this.Header.Idlength;
        skipover += this.Header.Colourmaptype * this.Header.Colourmaplength;
        //Console.Error.WriteLine("Skip over {0} bytes", skipover);
        fptr.Seek(skipover, SeekOrigin.Current);

        // Read the image
        bytes2read = this.Header.Bitsperpixel / 8;
        while (n < this.Header.Width * this.Header.Height)
        {
            if (this.Header.Datatypecode == 2)
            {                     // Uncompressed
                this.UncompressedBlock(n, i, bytes2read, p, fptr);
                n++;
            }
            else if (this.Header.Datatypecode == 10)
            {             // Compressed
                CheckEoF(i, bytes2read, 1, p, fptr);
                n = this.CompressedBlock(n, out i, out int j, bytes2read, p, fptr);
            }
        }
        fptr.Flush();
        fptr.Close();
    }

    private int CompressedBlock(int n, out int i, out int j, int bytes2read, byte[] p, FileStream fptr)
    {
        j = p[0] & 0x7f;
        MergeBytes(ref this.Pixels[n], p.AsSpan(1).ToArray(), bytes2read);
        n++;
        if ((p[0] & 0x80) != 0)
        {         // RLE chunk
            for (i = 0; i < j; i++)
            {
                MergeBytes(ref this.Pixels[n], p.AsSpan(1).ToArray(), bytes2read);
                n++;
            }
        }
        else
        {                   // Normal chunk
            for (i = 0; i < j; i++)
            {
                this.UncompressedBlock(n, i, bytes2read, p, fptr);
                n++;
            }
        }

        return n;
    }

    private void UncompressedBlock(int n, int i, int bytes2read, byte[] p, FileStream fptr)
    {
        CheckEoF(i, bytes2read, 0, p, fptr);
        MergeBytes(ref this.Pixels[n], p, bytes2read);
    }

    private static void CheckEoF(int i, int bytes2read, int offset, byte[] p, FileStream fptr)
    {
        if (fptr.Read(p, 0, bytes2read + offset) != bytes2read + offset)
        {
            Console.Error.WriteLine("Unexpected end of file at pixel {0}", i);
            Environment.Exit(-1);
        }
    }

    public string Output()
    {
        FileStream fptr;
        // Write the result as a uncompressed TGA
        if ((fptr = File.OpenWrite(this.OutputPath)) == null)
        {
            Console.Error.WriteLine("Failed to open outputfile");
            Environment.Exit(-1);
        }
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(2);                         // uncompressed RGB
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(0);
        fptr.WriteByte(0);           // X origin
        fptr.WriteByte(0);
        fptr.WriteByte(0);           // y origin
        fptr.WriteByte((byte)(this.Header.Width & 0x00FF));
        fptr.WriteByte((byte)((this.Header.Width & 0xFF00) / 256));
        fptr.WriteByte((byte)(this.Header.Height & 0x00FF));
        fptr.WriteByte((byte)((this.Header.Height & 0xFF00) / 256));
        fptr.WriteByte((byte)this.Header.Bitsperpixel);                        // 24 bit bitmap
        fptr.WriteByte(0);
        for (int i = 0; i < this.Header.Height * this.Header.Width; i++)
        {
            fptr.WriteByte(this.Pixels[i].B);
            fptr.WriteByte(this.Pixels[i].G);
            fptr.WriteByte(this.Pixels[i].R);
            if (this.Header.Bitsperpixel == 32)
            {
                fptr.WriteByte(this.Pixels[i].A);
            }
        }
        fptr.Flush();
        fptr.Close();

        return "TGA Written to " + this.OutputPath;
    }

    public static void MergeBytes(ref PIXEL pixel, byte[] p, int bytes)
    {
        if (bytes == 4)
        {
            pixel.R = p[2];
            pixel.G = p[1];
            pixel.B = p[0];
            pixel.A = p[3];
        }
        else if (bytes == 3)
        {
            pixel.R = p[2];
            pixel.G = p[1];
            pixel.B = p[0];
            pixel.A = 255;
        }
        else if (bytes == 2)
        {
            pixel.R = (byte)((p[1] & 0x7c) << 1);
            pixel.G = (byte)(((p[1] & 0x03) << 6) | ((p[0] & 0xe0) >> 2));
            pixel.B = (byte)((p[0] & 0x1f) << 3);
            pixel.A = (byte)(p[1] & 0x80);
        }
    }

    private static short ReadShort(FileStream fptr)
    {
        byte[] buffer = new byte[2];
        fptr.Read(buffer, 0, 2);
        return BitConverter.ToInt16(buffer, 0);
    }

    public Vector2 ConvertIndexToCoordinates(int i) => new(i % this.Header.Width, i / this.Header.Width);

    public int ConvertCoordinatesToIndex(Vector2 coord)
    {
        if (coord.X < 0)
        {
            coord.X = 0;
        }

        if (coord.Y < 0)
        {
            coord.Y = 0;
        }

        if (coord.Y >= this.Header.Height)
        {
            coord.Y = this.Header.Height - 1;
        }

        if (coord.X >= this.Header.Width)
        {
            coord.X = this.Header.Width - 1;
        }

        return (int)coord.X + (this.Header.Width * (int)coord.Y);
    }

    public string GetName() => Path.GetFileName(this.LoadPath);
    public string OutputPath { get; set; }
    public string LoadPath { get; set; }

    public HEADER RefHeader => this.Header;

    public PIXEL[] Pixels
    {
        get => this.pixels;
        set => this.pixels = value;
    }
}
