using RTWLib_CLI.draw;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;

namespace RTWLib_CLI.cmd
{
    public static class Rand
    {
        static EDB edb = new EDB();
        static EDU edu = new EDU();
        static DS ds = new DS();
        static DR dr = new DR();
        static CityMap cm = new CityMap();
        static SMF smf = new SMF();

        static TGA mr = new TGA();
        static TGA bm = new TGA(RemasterRome.GetPath(false, "bm"), "");

        public static string Ownership(int factionList = 0, int maxPerUnit = 3, int minimumPerUnit = 1)
        {
            if (edu == null)
                return "EDU not loaded - run 'rand initialsetup'";

            return RandEDU.RandomiseOwnership(edu, factionList, maxPerUnit, minimumPerUnit);
        }

        public static string CitiesBasic()
        {
            if (ds == null)
                return "DS not loaded - run 'rand initialsetup'";
            return RandDS.RandCitiesBasic(ds, cm);
        }

        public static string CitiesVoronoi()
        {
            if (ds == null)
                return "DS not loaded - run 'rand initialsetup'";
            return RandDS.RandCitiesVoronoi(ds, cm);
        }

        public static string InitialSetup() {

            List<IWrapper> list = new List<IWrapper>() {edu, edb, ds, dr, smf, mr};
            Progress p = new Progress(1f/(list.Count+1), "Setting up");
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Parse();
                p.Update("Loaded: " + RFH.GetPartOfPath(list[i].LoadPath, "randomiser"));
            }
            
            cm = new CityMap(mr, dr);
            p.Update("Forming: " + "city map");
            return "Files Loaded";
        }

        public static string Output()
        {
            string path = string.Empty;

            List<IWrapper> list = new List<IWrapper>() { edu, ds};
            Progress p = new Progress(0.50f, "Loading Files");
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Output();
                p.Update();
                Thread.Sleep(500);
            }


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
