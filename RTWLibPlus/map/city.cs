using RTWLibPlus.dataWrappers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.map
{
    public class CityMap
    {
        public Dictionary<string, int[]> CityCoordinates = new Dictionary<string, int[]>();

        public CityMap(TGA image, DR dr) {
            GetCityCoords(image, dr);
           
        }

        private void GetCityCoords(TGA image, DR dr)
        {
            for(int i =0; i < image.pixels.Length; i++) 
            {
                int[] coord = ConvertIndexToCoordinates(i, image.header.width);
                int[] upCoord = new int[] { coord[0], coord[1]+1};
                int upInd = ConvertCoordinatesToIndex(upCoord, image.header.width, image.header.height);

                TGA.PIXEL pixel = image.pixels[i];
                TGA.PIXEL upPixel = image.pixels[upInd];

                if (image.pixels[i].r == 0 &&
                    image.pixels[i].g == 0 &&
                    image.pixels[i].b == 0)// check for cities - should be a black pixel
                {
                    string key = string.Format("{0} {1} {2}", upPixel.r, upPixel.g, upPixel.b);
                    string region = dr.RegionsByColour[key];

                    CityCoordinates.Add(region, coord);
                }

            }
        }

        private int[] ConvertIndexToCoordinates(int i, int width)
        {
            return new int[] {i%width, i/width};
        }

        private int ConvertCoordinatesToIndex(int[] coord, int width, int height)
        {
            if (coord[0] < 0 || coord[1] < 0 || coord[1] >= height || coord[0] >= width)
                return 0;

            return coord[0] + (width*coord[1]);
        }

    }
}
