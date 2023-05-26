using RTWLibPlus.dataWrappers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.map
{
    public class CityMap
    {
        public Dictionary<string, Vector2> CityCoordinates = new Dictionary<string, Vector2>();

        public int height = 0;
        public int width = 0;

        public CityMap() { }

        public CityMap(TGA image, DR dr) {
            GetCityCoords(image, dr);
            height = image.header.height;
            width = image.header.width;
        }

        private void GetCityCoords(TGA image, DR dr)
        {
            for(int i =0; i < image.pixels.Length; i++) 
            {
                Vector2 coord = ConvertIndexToCoordinates(i, image.header.width);
                Vector2 upCoord = new Vector2 ( coord.X, coord.Y+1);
                int upInd = ConvertCoordinatesToIndex(upCoord, image.header.width, image.header.height);

                TGA.PIXEL pixel = image.pixels[i];
                TGA.PIXEL upPixel = image.pixels[upInd];

                if (image.pixels[i].r == 0 &&
                    image.pixels[i].g == 0 &&
                    image.pixels[i].b == 0)// check for cities - should be a black pixel
                {
                    string region = dr.GetRegionByColour(upPixel.r, upPixel.g, upPixel.b);

                    CityCoordinates.Add(region, coord);
                }

            }
        }

        private Vector2 ConvertIndexToCoordinates(int i, int width)
        {
            return new Vector2 (i%width, i/width);
        }

        private int ConvertCoordinatesToIndex(Vector2 coord, int width, int height)
        {
            if (coord.X < 0 || coord.Y < 0 || coord.Y >= height || coord.X >= width)
                return 0;

            return (int)((int)coord.X + (width*(int)coord.Y));
        }

    }
}
