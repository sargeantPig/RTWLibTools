namespace RTWLib_CLI.cmd.modules;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;

public class RandCMD(TWConfig config)
{
    private readonly TWConfig config = config;
    private readonly RandWrap rnd = new("0");
    private readonly DMB dmb = new(config.GetPath(Operation.Save, "dmb"), config.GetPath(Operation.Load, "dmb"));
    private readonly EDB edb = new(config.GetPath(Operation.Save, "edb"), config.GetPath(Operation.Load, "edb"));
    private readonly EDU edu = new(config.GetPath(Operation.Save, "edu"), config.GetPath(Operation.Load, "edu"));
    private readonly DS ds = new(config.GetPath(Operation.Save, "ds"), config.GetPath(Operation.Load, "ds"));
    private readonly DR dr = new(config.GetPath(Operation.Save, "dr"), config.GetPath(Operation.Load, "dr"));
    private readonly SMF smf = new(config.GetPath(Operation.Save, "smf"), config.GetPath(Operation.Load, "smf"));
    private readonly TGA mr = new(config.GetPath(Operation.Load, "mr"), "");
    private readonly TGA bm = new(config.GetPath(Operation.Load, "bm"), "");
    private CityMap cm = new();

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

        return RandDS.SwitchUnitsToRecruitable(this.edu, this.ds, this.rnd);
    }

    public string PaintFactionMap()
    {
        FactionMap factionMap = new();
        factionMap.PaintRegionMap(this.mr, this.bm, this.ds, this.dr, this.smf, this.config.GetPath(Operation.Save, "dir_campaign"));
        return "Maps Painted";
    }

    public string SetSeed(string seed)
    {
        this.rnd.SetRndSeed(seed);
        if (seed == "-")
        {
            this.rnd.RefreshRndSeed();
        }
        return "Seed set to: " + rnd.GetSeed;
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
