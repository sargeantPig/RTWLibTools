namespace RTWLibPlus.map;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using System.Collections.Generic;
using System.Numerics;

public class CityMap
{
    public Dictionary<string, Vector2> CityCoordinates { get; set; }

    public bool[,] WaterMap { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }

    public CityMap() { }

    public CityMap(TGA image, DR dr)
    {
        this.CityCoordinates = [];
        this.GetCityCoords(image, dr);
        this.Height = image.RefHeader.Height;
        this.Width = image.RefHeader.Width;
        this.WaterMap = new bool[this.Width, this.Height];
    }

    public Vector2 GetClosestWater(Vector2 from)
    {
        float distance = 100000;
        Vector2 water = new();
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                if (!this.WaterMap[x, y])
                {
                    continue;
                }

                Vector2 current = new(x, y);
                float tempDis = Vector2.Distance(from, current);
                if (tempDis < distance)
                {
                    distance = tempDis;
                    water = current;
                    this.WaterMap[x, y] = false;
                }

            }
        }
        return water;
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

    private void GetWaterMap(TGA image)
    {
        for (int i = 0; i < image.Pixels.Length; i++)
        {
            Vector2 coord = this.ConvertIndexToCoordinates(i, image.RefHeader.Width);

            PIXEL pixel = image.Pixels[i];

            if (pixel.R == 41 &&
                pixel.G == 140 &&
                pixel.B == 233)// check for sea
            {
                this.WaterMap[(int)coord.X, (int)coord.Y] = true;
            }
            else
            {
                this.WaterMap[(int)coord.X, (int)coord.Y] = false;
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
