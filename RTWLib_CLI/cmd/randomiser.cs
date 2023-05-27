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
        static EDB edb = new EDB(RemasterRome.GetPath(Operation.Save, "edb"), RemasterRome.GetPath(Operation.Load, "edb"));
        static EDU edu = new EDU(RemasterRome.GetPath(Operation.Save, "edu"), RemasterRome.GetPath(Operation.Load, "edu"));
        static DS ds = new DS(RemasterRome.GetPath(Operation.Save, "ds"), RemasterRome.GetPath(Operation.Load, "ds"));
        static DR dr = new DR(RemasterRome.GetPath(Operation.Save, "dr"), RemasterRome.GetPath(Operation.Load, "dr"));
        static SMF smf = new SMF(RemasterRome.GetPath(Operation.Save, "smf"), RemasterRome.GetPath(Operation.Load, "smf"));
        static TGA mr = new TGA(RemasterRome.GetPath(Operation.Load, "mr"), "");
        static TGA bm = new TGA(RemasterRome.GetPath(Operation.Load, "bm"), "");
        static CityMap cm = new CityMap();

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

        public static string PaintFactionMap()
        {
            FactionMap factionMap = new FactionMap();
            factionMap.PaintRegionMap(mr, bm, ds, dr, smf);
            return "Maps Painted";
        }

        public static string SetSeed(string seed)
        {
            TWRand.RefreshRndSeed(seed);
            return "Seed set to: " + seed;
        }


        public static string InitialSetup() {

            List<IWrapper> list = new List<IWrapper>() {edu, edb, ds, dr, smf, mr, bm};
            Progress p = new Progress(1f/(list.Count+1), "Setting up");
            for (int i = 0; i < list.Count; i++)
            {
                
                list[i].Clear();
                p.Message("Loading: " + RFH.GetPartOfPath(list[i].LoadPath, "randomiser"));
                list[i].Parse();
                p.Update("Complete");
            }
            
            cm = new CityMap(mr, dr);
            p.Message("Forming: City Map");
            p.Update("Complete");
            
            return "Files Loaded";
        }

        public static string Output()
        {
            string path = string.Empty;

            List<IWrapper> list = new List<IWrapper>() { edu, ds};
            Progress p = new Progress(0.50f, "Writing Files");
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Output();
                p.Update();
            }


            if (edu != null)
            {
                path = RemasterRome.GetPath(Operation.Save, "edu");
                RFH.Write(path, edu.Output());
            }
            if (ds != null)
            {
                path = RemasterRome.GetPath(Operation.Save, "ds");
                RFH.Write(path, ds.Output());
            }
            return "output complete";
        }
    }
}
