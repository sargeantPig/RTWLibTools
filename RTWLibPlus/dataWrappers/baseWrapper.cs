namespace RTWLibPlus.dataWrappers;

using RTWLibPlus.data;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseWrapper
{
    public delegate BaseWrapper WrapperCreator(List<IBaseObj> data, TWConfig config);
    public string OutputPath { get; set; }
    public string LoadPath { get; set; }
    public List<IBaseObj> Data { get; set; } = [];
    /// <summary>
    /// Location is a collection of strings that represent the tags. Once the final string is found it will return the corresponding kv
    /// </summary>
    /// <param name="location"></param>
    public static bool ModifyValue(List<IBaseObj> items, string newValue, int locInd = 0, bool done = false, params string[] location)
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
                    done = ModifyValue(item.GetItems(), newValue, ++locInd, done, location);
                }
                else
                {
                    locInd++;
                }
            }
        }

        return done;
    }
    public static void DeleteValue(List<IBaseObj> items, string ident, int locInd = 0)
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
                DeleteValue(item.GetItems(), ident, ++locInd);
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

            if (ident == item.Ident && !identFound)
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

    public Dictionary<int, List<IBaseObj>> GetChunks(string stopAt, string ident)
    {
        Dictionary<int, List<IBaseObj>> chunks = [];
        int ready = 0;
        bool identFound = false;
        int startI = -1;
        int count = 0;
        for (int i = 0; i < this.Data.Count; i++)
        {
            IBaseObj item = this.Data[i];
            if (identFound && ready == 2)
            {
                chunks.Add(i, this.Data.GetRange(startI, count));
                count = 0;
                identFound = false;
                ready = 0;
                i -= 2;
            }

            if (stopAt == item.Ident && ready == 1)
            {
                ready += 1;
            }

            if (ready == 1)
            {
                count++;
            }

            if (ident == item.Ident && ready == 0)
            {
                identFound = true;
                ready += 1;
                startI = i;
                count++;
            }

        }

        return chunks;
    }

    public Dictionary<int, int> GetChunkIndexes(string stopAt, string ident)
    {
        Dictionary<int, int> chunks = [];
        int ready = 0;
        bool identFound = false;
        int startI = -1;
        int count = 0;
        for (int i = 0; i < this.Data.Count; i++)
        {
            IBaseObj item = this.Data[i];
            if (identFound && ready == 2)
            {
                chunks.Add(startI, count);
                count = 0;
                identFound = false;
                ready = 0;
                i -= 2;
            }

            if (stopAt == item.Ident && ready == 1)
            {
                ready += 1;
            }

            if (ready == 1)
            {
                count++;
            }

            if (ident == item.Ident && ready == 0)
            {
                identFound = true;
                ready += 1;
                startI = i;
                count++;
            }

        }

        return chunks;
    }

    public static bool AddObjToList(List<IBaseObj> items, IBaseObj obj, int locInd = 0, bool done = false, params string[] location)
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
                    done = AddObjToList(item.GetItems(), obj, ++locInd, done, location);
                }
            }
        }
        return done;
    }
    public static bool InsertNewObjectByCriteria(List<IBaseObj> items, IBaseObj obj, params string[] criteriaTags)
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
        List<IBaseObj> found = [];
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
        List<IBaseObj> found = [];


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
        List<IBaseObj> found = [];
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
                found.Add(GetItemAtLocation(item.GetItems(), 0, lookFor));
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

    public List<IBaseObj> GetItemsByCriteriaSimpleDepth(List<IBaseObj> list, string stopAt, string lookFor, Dictionary<string, bool> criteria, params string[] criteriaTags)
    {
        if (criteria.Count == 0)
        {
            criteria = criteriaTags.ToDictionary(item => item, item => false);
        }

        List<IBaseObj> found = [];
        foreach (BaseObj item in list)
        {
            if (criteriaTags.Any(tag => tag == item.Tag))
            {
                criteria[item.Tag] = true;
            }

            bool criteriaMet = criteria.Values.All((value) => value);
            if (item.Ident == lookFor && criteriaMet)
            {
                found.Add(item);
            }

            if (item.Items.Count > 0)
            {
                List<IBaseObj> nest = this.GetItemsByCriteriaSimpleDepth(item.Items, stopAt, lookFor, criteria, criteriaTags);
                found.AddRange(nest);
            }

            if (item.Ident == stopAt && criteriaMet)
            {
                return found;
            }
        }
        return found;
    }

    public List<IBaseObj> GetItemsByKeyDict(List<IBaseObj> data, string stopAt, string lookFor, KeyValuePair<string, string> criteria)
    {
        List<IBaseObj> result = [];
        bool found = false;
        foreach (BaseObj item in data)
        {
            if (item.Ident == criteria.Key && item.Value == criteria.Value)
            {
                found = true;
            }

            if (found && item.Ident == lookFor)
            {
                result.Add(item);
            }

            if (item.Items.Count > 0)
            {
                result.AddRange(this.GetItemsByKeyDict(item.Items, stopAt, lookFor, criteria));
            }

            if (item.Ident == stopAt)
            {
                return result;
            }
        }
        return result;
    }

    public List<IBaseObj> GetAllItemsButStopAt(Func<string, bool> stopAt, params string[] criteriaTags)
    {
        bool[] criteria = new bool[criteriaTags.Length];
        int currCrit = 0;
        List<IBaseObj> found = [];
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
        List<IBaseObj> found = [];
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
    public static List<IBaseObj> GetItemList(List<IBaseObj> items, int locInd = 0, params string[] location)
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
                    return GetItemList(item.GetItems(), ++locInd, location);
                }
            }
        }
        return null;
    }
    public static IBaseObj GetItemAtLocation(List<IBaseObj> items, int locInd = 0, params string[] location)
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
                    return GetItemAtLocation(item.GetItems(), ++locInd, location);
                }
                else
                {
                    locInd += 1;
                }
            }
        }
        return null;
    }
    public static KeyValuePair<string, string> GetKeyValueAtLocation(List<IBaseObj> items, int locInd = 0, params string[] location)
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
                    return GetKeyValueAtLocation(item.GetItems(), ++locInd, location);
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

            if (CheckForValue(item.GetItems(), lookFor))
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

            if (CheckForValue(item.GetItems(), contentsValue))
            {
                return atLook;
            }
        }
        return null;
    }
    public static int GetIndexByCriteria(List<IBaseObj> items, params string[] criteria)
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
    private static bool CheckForValue(List<IBaseObj> items, string lookFor)
    {
        foreach (BaseObj item in items)
        {
            if (item.Value == lookFor)
            {
                return true;
            }

            if (item.GetItems().Count > 0)
            {
                return CheckForValue(item.GetItems(), lookFor);
            }
        }
        return false;
    }
}

