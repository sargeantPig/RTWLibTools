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
    public class RandCMD
    {
        TWConfig config;
        RandWrap rnd;
        EDB edb;
        EDU edu;
        DS ds;
        DR dr;
        SMF smf;
        TGA mr;
        TGA bm;
        CityMap cm = new CityMap();

        public RandCMD(TWConfig config)
        {
            this.config = config;
            edb = new EDB(config.GetPath(Operation.Save, "edb"), config.GetPath(Operation.Load, "edb"));
            edu = new EDU(config.GetPath(Operation.Save, "edu"), config.GetPath(Operation.Load, "edu"));
            ds = new DS(config.GetPath(Operation.Save, "ds"), config.GetPath(Operation.Load, "ds"));
            dr = new DR(config.GetPath(Operation.Save, "dr"), config.GetPath(Operation.Load, "dr"));
            smf = new SMF(config.GetPath(Operation.Save, "smf"), config.GetPath(Operation.Load, "smf"));
            mr = new TGA(config.GetPath(Operation.Load, "mr"), "");
            bm = new TGA(config.GetPath(Operation.Load, "bm"), "");
            rnd = new RandWrap("0");
        }

        public string Ownership(int maxPerUnit = 3, int minimumPerUnit = 1)
        {
            if (edu == null)
                return "EDU not loaded - run 'rand initialsetup'";

            return RandEDU.RandomiseOwnership(edu, rnd, smf, maxPerUnit, minimumPerUnit);
        }

        public string CitiesBasic()
        {
            if (ds == null)
                return "DS not loaded - run 'rand initialsetup'";
            return RandDS.RandCitiesBasic(smf, rnd, ds, cm);
        }

        public string CitiesVoronoi()
        {
            if (ds == null)
                return "DS not loaded - run 'rand initialsetup'";
            return RandDS.RandCitiesVoronoi(smf, rnd, ds, cm);
        }

        public string PaintFactionMap()
        {
            FactionMap factionMap = new FactionMap();
            factionMap.PaintRegionMap(mr, bm, ds, dr, smf, config.GetPath(Operation.Save, "dir_campaign"));
            return "Maps Painted";
        }

        public string SetSeed(string seed)
        {
            rnd.RefreshRndSeed(seed);
            return "Seed set to: " + seed;
        }


        public string InitialSetup() {

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

        public string Output()
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
                path = config.GetPath(Operation.Save, "edu");
                RFH.Write(path, edu.Output());
            }
            if (ds != null)
            {
                path = config.GetPath(Operation.Save, "ds");
                RFH.Write(path, ds.Output());
            }
            return "output complete";
        }
    }
}
