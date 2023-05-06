using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    /*Converted from C code  - http://www.paulbourke.net/dataformats/tga/tgatest.c*/
    public class TGA
    {
        public struct HEADER
        {
            public char idlength;
            public char colourmaptype;
            public char datatypecode;
            public short colourmaporigin;
            public short colourmaplength;
            public char colourmapdepth;
            public short x_origin;
            public short y_origin;
            public short width;
            public short height;
            public char bitsperpixel;
            public char imagedescriptor;
        }

        public struct PIXEL
        {
            public byte r, g, b, a;
        }

        public HEADER header = new HEADER();
        public PIXEL[] pixels; 

        public void Read(params string[] args)
        {
            int n = 0, i, j;
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

            // Display the header fields
            header.idlength = (char)fptr.ReadByte();
            Console.Error.WriteLine("ID length:         {0}", header.idlength);
            header.colourmaptype = (char)fptr.ReadByte();
            Console.Error.WriteLine("Colourmap type:    {0}", header.colourmaptype);
            header.datatypecode = (char)fptr.ReadByte();
            Console.Error.WriteLine("Image type:        {0}", header.datatypecode);
            header.colourmaporigin = ReadShort(fptr);
            Console.Error.WriteLine("Colour map offset: {0}", header.colourmaporigin);
            header.colourmaplength = ReadShort(fptr);
            Console.Error.WriteLine("Colour map length: {0}", header.colourmaplength);
            header.colourmapdepth = (char)fptr.ReadByte();
            Console.Error.WriteLine("Colour map depth:  {0}", header.colourmapdepth);
            header.x_origin = ReadShort(fptr);
            Console.Error.WriteLine("X origin:          {0}", header.x_origin);
            header.y_origin = ReadShort(fptr);
            Console.Error.WriteLine("Y origin:          {0}", header.y_origin);
            header.width = ReadShort(fptr);
            Console.Error.WriteLine("Width:             {0}", header.width);
            header.height = ReadShort(fptr);
            Console.Error.WriteLine("Height:            {0}", header.height);
            header.bitsperpixel = (char)fptr.ReadByte();
            Console.Error.WriteLine("Bits per pixel:    {0}", header.bitsperpixel);
            header.imagedescriptor = (char)fptr.ReadByte();
            Console.Error.WriteLine("Descriptor:        {0}", header.imagedescriptor);

            // Allocate space for the image
            pixels = new PIXEL[header.width * header.height];
            for (i = 0; i < header.width * header.height; i++)
            {
                pixels[i].r = 0;
                pixels[i].g = 0;
                pixels[i].b = 0;
                pixels[i].a = 0;
            }

            // What can we handle
            if (header.datatypecode != 2 && header.datatypecode != 10)
            {
                Console.Error.WriteLine("Can only handle image type 2 and 10");
                Environment.Exit(-1);
            }
            if (header.bitsperpixel != 16 &&
                header.bitsperpixel != 24 && header.bitsperpixel != 32)
            {
                Console.Error.WriteLine("Can only handle pixel depths of 16, 24, and 32");
                Environment.Exit(-1);
            }

            // Skip over unnecessary stuff
            skipover += header.idlength;
            skipover += header.colourmaptype * header.colourmaplength;
            Console.Error.WriteLine("Skip over {0} bytes", skipover);
            fptr.Seek(skipover, SeekOrigin.Current);

            // Read the image
            bytes2read = header.bitsperpixel / 8;
            while (n < header.width * header.height)
            {
                if (header.datatypecode == 2)
                {                     // Uncompressed
                    UncompressedBlock(n, i, bytes2read, p, fptr);
                    n++;
                }
                else if (header.datatypecode == 10)
                {             // Compressed
                    CheckEoF(i, bytes2read, 1, p, fptr);
                    n = CompressedBlock(n, out i, out j, bytes2read, p, fptr);
                }
            }
            fptr.Close();
        }

        private int CompressedBlock(int n, out int i, out int j, int bytes2read, byte[] p, FileStream fptr)
        {
            j = p[0] & 0x7f;
            MergeBytes(ref pixels[n], p.AsSpan<byte>(1).ToArray(), bytes2read);
            n++;
            if ((p[0] & 0x80) != 0)
            {         // RLE chunk
                for (i = 0; i < j; i++)
                {
                    MergeBytes(ref pixels[n], p.AsSpan<byte>(1).ToArray(), bytes2read);
                    n++;
                }
            }
            else
            {                   // Normal chunk
                for (i = 0; i < j; i++)
                {
                    UncompressedBlock(n, i, bytes2read, p, fptr);
                    n++;
                }
            }

            return n;
        }

        private void UncompressedBlock(int n, int i, int bytes2read, byte[] p, FileStream fptr)
        {
            CheckEoF(i, bytes2read, 0, p, fptr);
            MergeBytes(ref pixels[n], p, bytes2read);
        }

        private static void CheckEoF(int i, int bytes2read, int offset, byte[] p, FileStream fptr)
        {
            if (fptr.Read(p, 0, bytes2read + offset) != bytes2read + offset)
            {
                Console.Error.WriteLine("Unexpected end of file at pixel {0}", i);
                Environment.Exit(-1);
            }
        }

        public void Write(string filename)
        {
            FileStream fptr;
            // Write the result as a uncompressed TGA
            if ((fptr = File.OpenWrite(filename)) == null)
            {
                Console.Error.WriteLine("Failed to open outputfile");
                Environment.Exit(-1);
            }
            fptr.WriteByte(0);
            fptr.WriteByte(0);
            fptr.WriteByte(2);                         // uncompressed RGB
            fptr.WriteByte(0); fptr.WriteByte(0);
            fptr.WriteByte(0); fptr.WriteByte(0);
            fptr.WriteByte(0);
            fptr.WriteByte(0); fptr.WriteByte(0);           // X origin
            fptr.WriteByte(0); fptr.WriteByte(0);           // y origin
            fptr.WriteByte((byte)(header.width & 0x00FF));
            fptr.WriteByte((byte)((header.width & 0xFF00) / 256));
            fptr.WriteByte((byte)(header.height & 0x00FF));
            fptr.WriteByte((byte)((header.height & 0xFF00) / 256));
            fptr.WriteByte((byte)header.bitsperpixel);                        // 24 bit bitmap
            fptr.WriteByte(0);
            for (int i = 0; i < header.height * header.width; i++)
            {
                fptr.WriteByte(pixels[i].b);
                fptr.WriteByte(pixels[i].g);
                fptr.WriteByte(pixels[i].r);
                if(header.bitsperpixel == 32)
                    fptr.WriteByte(pixels[i].a);
            }
            fptr.Flush();
            fptr.Close();
        }

        public void MergeBytes(ref PIXEL pixel, byte[] p, int bytes)
        {
            if (bytes == 4)
            {
                pixel.r = p[2];
                pixel.g = p[1];
                pixel.b = p[0];
                pixel.a = p[3];
            }
            else if (bytes == 3)
            {
                pixel.r = p[2];
                pixel.g = p[1];
                pixel.b = p[0];
                pixel.a = 255;
            }
            else if (bytes == 2)
            {
                pixel.r = (byte)((p[1] & 0x7c) << 1);
                pixel.g = (byte)(((p[1] & 0x03) << 6) | ((p[0] & 0xe0) >> 2));
                pixel.b = (byte)((p[0] & 0x1f) << 3);
                pixel.a = (byte)(p[1] & 0x80);
            }
        }

        private short ReadShort(FileStream fptr)
        {
            byte[] buffer = new byte[2];
            fptr.Read(buffer, 0, 2);
            return BitConverter.ToInt16(buffer, 0);
        }

    }

}