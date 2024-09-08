namespace RTWLibPlus.map;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        this.Height = image.RefHeader.Height;
        this.Width = image.RefHeader.Width;
        this.CityCoordinates = [];
        this.GetCityCoords(image, dr);
        this.WaterMap = new bool[this.Width, this.Height];
        this.GetWaterMap(image);

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
                }
            }
        }
        this.WaterMap[(int)water.X, (int)water.Y] = false;
        //water.Y = this.Height - water.Y;
        return water;
    }

    public Dictionary<string, string[]> GetClosestRegions(Dictionary<string, string[]> factionRegions, int maxDistance)
    {
        Dictionary<string, string[]> closeFactions = [];

        foreach (KeyValuePair<string, string[]> a in factionRegions)
        {
            List<string> closeFactionList = [];
            closeFactions.Add(a.Key, []);
            foreach (string areg in a.Value)
            {
                Vector2 acoords = this.CityCoordinates[areg];
                foreach (KeyValuePair<string, string[]> b in factionRegions)
                {
                    if (a.Key == b.Key)
                    {
                        continue;
                    }
                    foreach (string breg in b.Value)
                    {
                        Vector2 bcoords = this.CityCoordinates[breg];

                        double distance = acoords.DistanceTo(bcoords);

                        if (distance < maxDistance)
                        {
                            closeFactionList.Add(b.Key);
                        }
                    }
                }
            }
            closeFactions[a.Key] = new HashSet<string>([.. closeFactionList]).ToArray();
        }
        return closeFactions;
    }

    private void GetCityCoords(TGA image, DR dr)
    {
        for (int i = 0; i < image.Pixels.Length; i++)
        {
            Vector2 coord = ConvertIndexToCoordinates(i, image.RefHeader.Width);
            Vector2 upCoord = new(coord.X, coord.Y + 1);
            int upInd = ConvertCoordinatesToIndex(upCoord, image.RefHeader.Width, image.RefHeader.Height);

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
            Vector2 coord = ConvertIndexToCoordinates(i, image.RefHeader.Width);
            Vector2 upCoord = new(coord.X, coord.Y + 1);
            Vector2 rightCoord = new(coord.X + 1, coord.Y);
            Vector2 leftCoord = new(coord.X - 1, coord.Y);
            Vector2 downCoord = new(coord.X, coord.Y - 1);

            coord.X = Math.Clamp(coord.X, 0, this.Width - 1);
            coord.Y = Math.Clamp(coord.Y, 0, this.Height - 1);
            PIXEL pixel = image.Pixels[i];
            PIXEL up, left, right, down;
            int iup = ConvertCoordinatesToIndex(upCoord, image.RefHeader.Width, image.RefHeader.Height);
            int ileft = ConvertCoordinatesToIndex(leftCoord, image.RefHeader.Width, image.RefHeader.Height);
            int iright = ConvertCoordinatesToIndex(rightCoord, image.RefHeader.Width, image.RefHeader.Height);
            int idown = ConvertCoordinatesToIndex(downCoord, image.RefHeader.Width, image.RefHeader.Height);
            up = image.Pixels[iup];
            left = image.Pixels[ileft];
            down = image.Pixels[idown];
            right = image.Pixels[iright];
            if (pixel.R == 41 && pixel.G == 140 && pixel.B == 233 &&
                up.R == 41 && up.G == 140 && up.B == 233 &&
                left.R == 41 && left.G == 140 && left.B == 233 &&
                right.R == 41 && right.G == 140 && right.B == 233 &&
                down.R == 41 && down.G == 140 && down.B == 233)// check for sea
            {
                this.WaterMap[(int)coord.X, (int)coord.Y] = true;
            }
            else
            {
                this.WaterMap[(int)coord.X, (int)coord.Y] = false;
            }
        }
    }

    private static Vector2 ConvertIndexToCoordinates(int i, int width) => new(i % width, i / width);

    private static int ConvertCoordinatesToIndex(Vector2 coord, int width, int height)
    {
        if (coord.X < 0 || coord.Y < 0 || coord.Y >= height || coord.X >= width)
        {
            return 0;
        }

        return (int)coord.X + (width * (int)coord.Y);
    }

}
