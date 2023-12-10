namespace RTWLibPlus.map;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using System.Collections.Generic;
using System.Numerics;

public class CityMap
{
    public Dictionary<string, Vector2> CityCoordinates { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }

    public CityMap() { }

    public CityMap(TGA image, DR dr)
    {
        this.CityCoordinates = new();
        this.GetCityCoords(image, dr);
        this.Height = image.RefHeader.Height;
        this.Width = image.RefHeader.Width;
    }

    private void GetCityCoords(TGA image, DR dr)
    {
        for (int i = 0; i < image.Pixels.Length; i++)
        {
            Vector2 coord = this.ConvertIndexToCoordinates(i, image.RefHeader.Width);
            Vector2 upCoord = new(coord.X, coord.Y + 1);
            int upInd = this.ConvertCoordinatesToIndex(upCoord, image.RefHeader.Width, image.RefHeader.Height);

            PIXEL pixel = image.Pixels[i];
            PIXEL upPixel = image.Pixels[upInd];

            if (pixel.R == 0 &&
                pixel.G == 0 &&
                pixel.B == 0)// check for cities - should be a black pixel
            {
                string region = dr.GetRegionByColour(upPixel.R, upPixel.G, upPixel.B);

                this.CityCoordinates.Add(region, coord);
            }

        }
    }

    private Vector2 ConvertIndexToCoordinates(int i, int width) => new(i % width, i / width);

    private int ConvertCoordinatesToIndex(Vector2 coord, int width, int height)
    {
        if (coord.X < 0 || coord.Y < 0 || coord.Y >= height || coord.X >= width)
        {
            return 0;
        }

        return (int)coord.X + (width * (int)coord.Y);
    }

}
