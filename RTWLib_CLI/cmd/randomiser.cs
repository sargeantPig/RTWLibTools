using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_CLI.cmd
{
    public static class Rand
    {
        static EDU edu;
        static DS ds;
        static DR dr;
        static CityMap cm;
        static TGA mr;

        public static string Ownership(int factionList = 0, int maxPerUnit = 3, int minimumPerUnit = 1)
        {
            string path = RemasterRome.GetPath(false, "edu");
            if(edu == null)
                 edu = new EDU(RFH.ParseFile(Creator.EDUcreator, ' ', false, path));
            return RandEDU.RandomiseOwnership(edu, factionList, maxPerUnit, minimumPerUnit);
        }

        public static string CitiesBasic()
        {
            string dspath = RemasterRome.GetPath(false, "ds");
            string drpath = RemasterRome.GetPath(false, "dr");
            string mrpath = RemasterRome.GetPath(false, "mr");
            
            if (ds == null)
                ds = new DS(RFH.ParseFile(Creator.DScreator, ' ', false, dspath));
            if(dr == null)
                dr = new DR(RFH.ParseFile(Creator.DRcreator, '\t', false, drpath));
            if (mr == null)
                mr = new TGA("tgafile", mrpath);
            if (cm == null)
                cm = new CityMap(mr, dr);

            return RandDS.RandCitiesBasic(ds, cm);
        }


        public static string Output()
        {
            string path = string.Empty;
            if (edu != null)
            {
                path = RemasterRome.GetPath(true, "edu");
                RFH.Write(path, edu.Output());
            }
            if (ds != null)
            {
                path = RemasterRome.GetPath(true, "ds");
                RFH.Write(path, ds.Output());
            }
            return "output complete";
        }
    }
}
