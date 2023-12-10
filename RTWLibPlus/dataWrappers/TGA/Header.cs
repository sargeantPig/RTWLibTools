namespace RTWLibPlus.dataWrappers.TGA;
public class HEADER
{
    public char Idlength { get; set;  }
    public char Colourmaptype { get; set; }
    public char Datatypecode { get; set; }
    public short Colourmaporigin { get; set; }
    public short Colourmaplength { get; set; }
    public char Colourmapdepth { get; set; }
    public short Xorigin { get; set; }
    public short Yorigin { get; set; }
    public short Width { get; set; }
    public short Height { get; set; }
    public char Bitsperpixel { get; set; }
    public char Imagedescriptor { get; set; }

    public HEADER(char idlength, char colourmaptype, char datatypecode,
        short colourmaporigin, short colourmaplength, char colourmapdepth,
        short xorigin, short yorigin, short width,
        short height, char bitsperpixel, char imagedescriptor)
    {
        this.Idlength = idlength;
        this.Colourmaptype = colourmaptype;
        this.Datatypecode = datatypecode;
        this.Colourmaporigin = colourmaporigin;
        this.Colourmaporigin = colourmaporigin;
        this.Colourmaplength = colourmaplength;
        this.Colourmapdepth = colourmapdepth;
        this.Xorigin = xorigin;
        this.Yorigin = yorigin;
        this.Width = width;
        this.Height = height;
        this.Bitsperpixel = bitsperpixel;
        this.Imagedescriptor = imagedescriptor;
    }

    public HEADER()
    {

    }
}
