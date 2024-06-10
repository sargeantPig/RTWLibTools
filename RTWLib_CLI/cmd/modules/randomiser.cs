namespace RTWLib_CLI.cmd.modules;

using RTWLib_CLI.draw;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;

public class RandCMD
{
    TWConfig config;
    RandWrap rnd;
    DMB dmb;
    EDB edb;
    EDU edu;
    DS ds;
    DR dr;
    SMF smf;
    TGA mr;
    TGA bm;
    CityMap cm = new();

    public RandCMD(TWConfig config)
    {
        this.config = config;
        this.edb = new EDB(config.GetPath(Operation.Save, "edb"), config.GetPath(Operation.Load, "edb"));
        this.edu = new EDU(config.GetPath(Operation.Save, "edu"), config.GetPath(Operation.Load, "edu"));
        this.ds = new DS(config.GetPath(Operation.Save, "ds"), config.GetPath(Operation.Load, "ds"));
        this.dr = new DR(config.GetPath(Operation.Save, "dr"), config.GetPath(Operation.Load, "dr"));
        this.smf = new SMF(config.GetPath(Operation.Save, "smf"), config.GetPath(Operation.Load, "smf"));
        this.dmb = new DMB(config.GetPath(Operation.Save, "dmb"), config.GetPath(Operation.Load, "dmb"));
        this.mr = new TGA(config.GetPath(Operation.Load, "mr"), "");
        this.bm = new TGA(config.GetPath(Operation.Load, "bm"), "");
        this.rnd = new RandWrap("0");
    }

    public string Ownership(int maxPerUnit = 3, int minimumPerUnit = 1)
    {
        if (this.edu == null)
        {
            return "EDU not loaded - run 'rand initialsetup'";
        }

        return RandEDU.RandomiseOwnership(this.edu, this.rnd, this.smf, maxPerUnit, minimumPerUnit);
    }

    public string CitiesBasic()
    {
        if (this.ds == null)
        {
            return "DS not loaded - run 'rand initialsetup'";
        }

        return RandDS.RandCitiesBasic(this.smf, this.rnd, this.ds, this.dr, this.cm);
    }

    public string CitiesVoronoi()
    {
        if (this.ds == null)
        {
            return "DS not loaded - run 'rand initialsetup'";
        }

        return RandDS.RandCitiesVoronoi(this.smf, this.rnd, this.ds, this.dr, this.cm);
    }

    public string StratArmiesUseOwnedUnits()
    {
        if (this.ds == null)
        {
            return "DS not loaded - run 'rand initialsetup'";
        }
        else if (this.edu == null)
        {
            return "EDU not loaded - run 'rand initialsetup'";
        }

        return RandDS.SwitchUnitsToRecruitable(this.edu, this.ds);
    }

    public string PaintFactionMap()
    {
        FactionMap factionMap = new();
        factionMap.PaintRegionMap(this.mr, this.bm, this.ds, this.dr, this.smf, this.config.GetPath(Operation.Save, "dir_campaign"));
        return "Maps Painted";
    }

    public string SetSeed(string seed)
    {
        this.rnd.RefreshRndSeed(seed);
        return "Seed set to: " + seed;
    }


    public string InitialSetup()
    {

        List<IWrapper> list = [this.edu, this.edb, this.ds, this.dr, this.smf, this.mr, this.bm, this.dmb];
        Console.WriteLine("Setting up");
        //Progress p = new(1f / (list.Count + 1), "Setting up");
        for (int i = 0; i < list.Count; i++)
        {

            list[i].Clear();
            Console.WriteLine("Loading: " + RFH.GetPartOfPath(list[i].LoadPath, "randomiser"));
            //p.Message("Loading: " + RFH.GetPartOfPath(list[i].LoadPath, "randomiser"));
            list[i].Parse();
            //p.Update("Complete");
        }
        this.edu.PrepareEDU();
        this.dmb.AddFallBacksForAllTypes();
        this.cm = new CityMap(this.mr, this.dr);
        Console.WriteLine("Forming: City Map");
        //p.Message("Forming: City Map");
        //p.Update("Complete");

        return "Files Loaded";
    }

    public string Output()
    {
        string path = string.Empty;

        List<IWrapper> list = [this.edu, this.ds, this.dmb];
        Console.WriteLine("Writing Files...");

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                Console.WriteLine("Writing... " + RFH.GetPartOfPath(list[i].OutputPath, "randomiser"));
                RFH.Write(list[i].OutputPath, list[i].Output());
            }
        }
        return "output complete";
    }
}
