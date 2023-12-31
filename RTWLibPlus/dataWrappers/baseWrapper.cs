namespace RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class BaseWrapper
{
    private string outputPath;

    public string OutputPath
    {
        get
        {
            string path = ExString.CrossPlatPath(this.outputPath);
            string dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir))
            { return path; }

            return path.Split('/').Last();
        }

        set => this.outputPath = value;
    }
    public string LoadPath { get; set; }

    public List<IBaseObj> Data { get; set; } = new();
    /// <summary>
    /// Location is a collection of strings that represent the tags. Once the final string is found it will return the corresponding kv
    /// </summary>
    /// <param name="location"></param>
    public bool ModifyValue(List<IBaseObj> items, string newValue, int locInd = 0, bool done = false, params string[] location)
    {
        foreach (BaseObj item in items)
        {
            if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
            {
                if (locInd == location.Length - 1)
                {
                    item.Value = newValue;
                    done = true;
                }
                else if (item.GetItems().Count > 0)
                {
                    done = this.ModifyValue(item.GetItems(), newValue, ++locInd, done, location);
                }
                else
                {
                    locInd++;
                }
            }
        }

        return done;
    }
    public void DeleteValue(List<IBaseObj> items, string ident, int locInd = 0)
    {
        for (int i = 0; i < items.Count; i++)
        {
            IBaseObj item = items[i];

            if (ident == item.Ident)
            {
                items.RemoveAt(i);
                i--;
                continue;
            }

            if (item.GetItems().Count > 0)
            {
                this.DeleteValue(item.GetItems(), ident, ++locInd);
            }
        }
    }
    public void DeleteChunks(string stopAt, string ident)
    {
        bool identFound = false;
        int startI = -1;
        int count = 0;
        for (int i = 0; i < this.Data.Count; i++)
        {
            IBaseObj item = this.Data[i];

            if (ident == item.Ident)
            {
                identFound = true;
                startI = i;
            }

            if (identFound && stopAt == item.Ident)
            {
                this.Data.RemoveRange(startI, count);
                startI = -1;
                i -= count + 1;
                count = 0;
                identFound = false;
            }

            if (identFound)
            {
                count++;
            }
        }
    }
    public bool AddObjToList(List<IBaseObj> items, IBaseObj obj, int locInd = 0, bool done = false, params string[] location)
    {
        foreach (BaseObj item in items)
        {
            if (location[locInd] == item.Tag)
            {
                if (locInd == location.Length - 1)
                {
                    item.GetItems().Add(obj);
                    done = true;
                }
                else
                {
                    done = this.AddObjToList(item.GetItems(), obj, ++locInd, done, location);
                }
            }
        }
        return done;
    }
    public bool InsertNewObjectByCriteria(List<IBaseObj> items, IBaseObj obj, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        for (int i = 0; i < items.Count; i++)
        {
            BaseObj item = (BaseObj)items[i];
            if (item == null)
            {
                continue;
            }

            if (item.Tag == criteriaTags[currCrit] || item.Ident == criteriaTags[currCrit])
            {
                criteria[currCrit] = true;
                currCrit++;
            }

            if (criteria.All((x) => x))
            {
                items.Insert(i + 1, obj);
                return true;
            }
        }
        return false;
    }
    public List<IBaseObj> GetItemsByIdent(string ident)
    {
        List<IBaseObj> found = new();
        foreach (BaseObj item in this.Data)
        {
            if (item.Ident == ident)
            {
                found.Add(item);
            }
        }
        /*Parallel.ForEach(Data, item =>
        {
            if (item.Ident == ident)
            {
                found.Push(item);
            }
        });*/


        return found;
    }
    public List<IBaseObj> GetItemsByCriteria(string stopAt, string lookFor, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        List<IBaseObj> found = new();


        foreach (BaseObj item in this.Data)
        {
            if (item.Ident == criteriaTags[currCrit] || item.Tag == criteriaTags[currCrit])
            {
                criteria[currCrit] = true;
                if (currCrit != criteriaTags.Length - 1)
                {
                    currCrit++;
                }
            }

            if (criteria.All((x) => x) && item.Ident == lookFor)
            {
                found.Add(item);
            }
            if (stopAt == item.Ident && criteria.All((x) => x))
            {
                return found;
            }
        }
        return found;
    }
    public List<IBaseObj> GetItemsByCriteriaDepth(List<IBaseObj> list, string stopAt, string lookFor, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        List<IBaseObj> found = new();
        foreach (BaseObj item in list)
        {
            if (item.Ident == criteriaTags[currCrit] || item.Tag == criteriaTags[currCrit])
            {
                criteria[currCrit] = true;
                if (currCrit != criteriaTags.Length - 1)
                {
                    currCrit++;
                }
            }

            if (criteria.All((x) => x))
            {
                found.Add(this.GetItemAtLocation(item.GetItems(), 0, lookFor));
                currCrit = 0;
                criteria = new bool[criteriaTags.Length];
            }
            if (stopAt == item.Ident && criteria.All((x) => x))
            {
                return found;
            }
        }
        return found;
    }
    public List<IBaseObj> GetAllItemsButStopAt(Func<string, bool> stopAt, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        List<IBaseObj> found = new();
        foreach (BaseObj item in this.Data)
        {
            if (item.Ident == criteriaTags[currCrit] || item.Tag == criteriaTags[currCrit])
            {
                criteria[currCrit] = true;
                if (currCrit != criteriaTags.Length - 1)
                {
                    currCrit++;
                }
            }

            if (criteria.All((x) => x))
            {
                found.Add(item);
            }
            if (stopAt(item.Ident) && criteria.All((x) => x) && item.Ident != criteriaTags[currCrit])
            {
                return found;
            }
        }
        return found;
    }
    public List<IBaseObj> GetNumberOfItems(int stopAt, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        List<IBaseObj> found = new();
        foreach (BaseObj item in this.Data)
        {
            if (item.Ident == criteriaTags[currCrit] || item.Tag == criteriaTags[currCrit])
            {
                criteria[currCrit] = true;
                if (currCrit != criteriaTags.Length - 1)
                {
                    currCrit++;
                }
            }

            if (criteria.All((x) => x))
            {
                found.Add(item);
            }
            if (found.Count >= stopAt && criteria.All((x) => x) && item.Ident != criteriaTags[currCrit])
            {
                return found;
            }
        }
        return found;
    }
    public List<IBaseObj> GetItemList(List<IBaseObj> items, int locInd = 0, params string[] location)
    {
        foreach (BaseObj item in items)
        {

            if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
            {
                if (locInd == location.Length - 1)
                {
                    return item.GetItems();
                }
                else
                {
                    return this.GetItemList(item.GetItems(), ++locInd, location);
                }
            }
        }
        return null;
    }
    public IBaseObj GetItemAtLocation(List<IBaseObj> items, int locInd = 0, params string[] location)
    {
        foreach (BaseObj item in items)
        {
            if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
            {
                if (locInd == location.Length - 1)
                {
                    return item;
                }
                else if (item.GetItems().Count > 0)
                {
                    return this.GetItemAtLocation(item.GetItems(), ++locInd, location);
                }
                else
                {
                    locInd += 1;
                }
            }
        }
        return null;
    }
    public KeyValuePair<string, string> GetKeyValueAtLocation(List<IBaseObj> items, int locInd = 0, params string[] location)
    {
        foreach (BaseObj item in items)
        {
            if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
            {
                if (locInd == location.Length - 1)
                {
                    return new KeyValuePair<string, string>(item.Tag, item.Value);
                }
                else if (item.GetItems().Count > 0)
                {
                    return this.GetKeyValueAtLocation(item.GetItems(), ++locInd, location);
                }
                else
                {
                    locInd += 1;
                }
            }
        }
        return new KeyValuePair<string, string>();
    }
    public IBaseObj GetItemByValue(List<IBaseObj> items, string lookFor)
    {
        foreach (BaseObj item in items)
        {
            if (item.GetItems().Count == 0)
            {
                continue;
            }

            if (this.CheckForValue(item.GetItems(), lookFor))
            {
                return item;
            }
        }
        return null;
    }
    public string GetTagByContentsValue(List<IBaseObj> items, string lookFor, string contentsValue)
    {
        string atLook = string.Empty;

        foreach (BaseObj item in items)
        {
            if (item.Ident == lookFor)
            {
                atLook = item.Tag;
            }

            if (item.GetItems().Count == 0)
            {
                continue;
            }

            if (this.CheckForValue(item.GetItems(), contentsValue))
            {
                return atLook;
            }
        }
        return null;
    }
    public int GetIndexByCriteria(List<IBaseObj> items, params string[] criteria)
    {
        int critIndex = 0;
        for (int i = 0; i < items.Count; i++)
        {
            BaseObj item = (BaseObj)items[i];
            if (criteria[critIndex] == item.Tag || criteria[critIndex] == item.Ident || criteria[critIndex] == item.Value)
            {
                if (critIndex == criteria.Length - 1)
                {
                    return i;
                }
                else
                {
                    critIndex += 1;
                }
            }
        }
        return -1;
    }
    public void InsertAt(int index, IBaseObj objToAdd) => this.Data.Insert(index, objToAdd);
    private bool CheckForValue(List<IBaseObj> items, string lookFor)
    {
        foreach (BaseObj item in items)
        {
            if (item.Value == lookFor)
            {
                return true;
            }

            if (item.GetItems().Count > 0)
            {
                return this.CheckForValue(item.GetItems(), lookFor);
            }
        }
        return false;
    }
}

