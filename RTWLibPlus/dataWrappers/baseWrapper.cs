using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class BaseWrapper
    {
        public List<IbaseObj> data = new List<IbaseObj>();
        /// <summary>
        /// Location is a collection of strings that represent the tags. Once the final string is found it will return the corresponding kv
        /// </summary>
        /// <param name="location"></param>
        public bool ModifyValue(List<IbaseObj> items, string newValue, int locInd = 0, bool done = false, params string[] location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
                {
                    if (locInd == location.Count() - 1)
                    {
                        item.Value = newValue;
                        done = true;
                    }
                    else if(item.GetItems().Count > 0) done = ModifyValue(item.GetItems(), newValue, ++locInd, done, location);

                    else locInd++;
                }
            }
            return done;
        }
        public void DeleteValue(List<IbaseObj> items, string ident, int locInd = 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if(ident == item.Ident)
                {
                    items.RemoveAt(i);
                    i--;
                    continue;
                }

                if (item.GetItems().Count > 0)
                {
                    ModifyValue(item.GetItems(), ident, ++locInd);
                }
            }
        }
        public bool AddObjToList(List<IbaseObj> items, IbaseObj obj, int locInd = 0, bool done = false, params string[] location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if (location[locInd] == item.Tag)
                {
                    if (locInd == location.Count() - 1)
                    {
                        item.GetItems().Add(obj);
                        done = true;
                    }
                    else done = AddObjToList(item.GetItems(), obj, ++locInd, done, location);
                }
            }
            return done;
        }
        public bool InsertNewObjectByCriteria(List<IbaseObj> items, IbaseObj obj, params string[] criteriaTags)
        {
            bool[] criteria = new bool[criteriaTags.Count()];
            int currCrit = 0;
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];
                if (item == null)
                    continue;

                if (item.Tag == criteriaTags[currCrit] || item.Ident == criteriaTags[currCrit])
                {
                    criteria[currCrit] = true;
                    currCrit++;
                }

                if (criteria.All((x) => x == true))
                {
                    items.Insert(i+1, obj);
                    return true;
                }
            }
            return false;
        }
        public List<IbaseObj> GetItemsByIdent(string ident) {
            List<IbaseObj> found = new List<IbaseObj>();
            foreach(var item in data)
            {
                baseObj bi = (baseObj)item;
                if(bi.Ident == ident)
                {
                    found.Add(bi);
                }
            }
            return found;
        }
        public List<IbaseObj> GetItemsByCriteria(string stopAt, string lookFor, params string[] criteriaTags)
        {
            bool[] criteria = new bool[criteriaTags.Count()];
            int currCrit = 0;
            List<IbaseObj> found = new List<IbaseObj>();
            foreach (var item in data)
            {
                baseObj bi = (baseObj)item;
                if (bi.Ident == criteriaTags[currCrit] || bi.Tag == criteriaTags[currCrit])
                {
                    criteria[currCrit] = true;
                    if(currCrit != criteriaTags.Count() -1)
                        currCrit++;
                }

                if (criteria.All((x) => x == true) && bi.Ident == lookFor)
                {
                    found.Add(bi) ;
                }
                if (stopAt == bi.Ident && criteria.All((x) => x == true))
                    return found;
            }
            return found;
        }

        public List<IbaseObj> GetItemsByCriteriaDepth(List<IbaseObj> list, string stopAt, string lookFor, params string[] criteriaTags)
        {
            bool[] criteria = new bool[criteriaTags.Count()];
            int currCrit = 0;
            List<IbaseObj> found = new List<IbaseObj>();
            foreach (var item in list)
            {
                baseObj bi = (baseObj)item;
                if (bi.Ident == criteriaTags[currCrit] || bi.Tag == criteriaTags[currCrit])
                {
                    criteria[currCrit] = true;
                    if (currCrit != criteriaTags.Count() - 1)
                        currCrit++;
                }

                if (criteria.All((x) => x == true))
                {
                    found.Add(GetItemAtLocation(item.GetItems(), 0, lookFor));
                    currCrit = 0;
                    criteria = new bool[criteriaTags.Count()];
                }
                if (stopAt == bi.Ident && criteria.All((x) => x == true))
                    return found;
            }
            return found;
        }

        public List<IbaseObj> GetAllItemsButStopAt(Func<string, bool> stopAt, params string[] criteriaTags)
        {
            bool[] criteria = new bool[criteriaTags.Count()];
            int currCrit = 0;
            List<IbaseObj> found = new List<IbaseObj>();
            foreach (var item in data)
            {
                baseObj bi = (baseObj)item;
                if (bi.Ident == criteriaTags[currCrit] || bi.Tag == criteriaTags[currCrit])
                {
                    criteria[currCrit] = true;
                    if (currCrit != criteriaTags.Count() - 1)
                        currCrit++;
                }

                if (criteria.All((x) => x == true))
                {
                    found.Add(bi);
                }
                if (stopAt(bi.Ident) && criteria.All((x) => x == true) && bi.Ident != criteriaTags[currCrit])
                    return found;
            }
            return found;
        }

        public List<IbaseObj> GetNumberOfItems(int stopAt, params string[] criteriaTags)
        {
            bool[] criteria = new bool[criteriaTags.Count()];
            int currCrit = 0;
            List<IbaseObj> found = new List<IbaseObj>();
            foreach (var item in data)
            {
                baseObj bi = (baseObj)item;
                if (bi.Ident == criteriaTags[currCrit] || bi.Tag == criteriaTags[currCrit])
                {
                    criteria[currCrit] = true;
                    if (currCrit != criteriaTags.Count() - 1)
                        currCrit++;
                }

                if (criteria.All((x) => x == true))
                {
                    found.Add(bi);
                }
                if (found.Count >= stopAt && criteria.All((x) => x == true) && bi.Ident != criteriaTags[currCrit])
                    return found;
            }
            return found;
        }

        public List<IbaseObj> GetItemList(List<IbaseObj> items, int locInd = 0, params string[] location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];
                
                if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
                {
                    if (locInd == location.Count() - 1)
                        return item.GetItems();
                    else return GetItemList(item.GetItems(), ++locInd, location);
                }
            }
            return null;
        }

        public IbaseObj GetItemAtLocation(List<IbaseObj> items, int locInd = 0, params string[] location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
                {
                    if (locInd == location.Count() - 1)
                        return item;
                    else if (item.GetItems().Count > 0) return GetItemAtLocation(item.GetItems(), ++locInd, location);

                    else locInd += 1;
                }
            }
            return null;
        }

        public KeyValuePair<string, string> GetKeyValueAtLocation(List<IbaseObj> items, int locInd = 0, params string[] location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if (location[locInd] == item.Tag || location[locInd] == item.Ident || location[locInd] == item.Value)
                {
                    if (locInd == location.Count() - 1)
                        return new KeyValuePair<string, string>(item.Tag, item.Value);
                    else if(item.GetItems().Count > 0 ) return GetKeyValueAtLocation(item.GetItems(), ++locInd, location);

                    else locInd += 1;
                }
            }
            return new KeyValuePair<string, string>();
        }

        public IbaseObj GetItemByValue(List<IbaseObj> items, string lookFor)
        {
            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];

                if (item.GetItems().Count == 0)
                    continue;
                if (CheckForValue(item.GetItems(), lookFor))
                {
                    return item;
                }
            }
            return null;
        }

        private bool CheckForValue(List<IbaseObj> items, string lookFor)
        {
            foreach(var item in items)
            {
                if(((baseObj)item).Value == lookFor)
                    return true;
                if (item.GetItems().Count > 0)
                    return CheckForValue(item.GetItems(), lookFor);

            }
            return false;
        }
    }
}
