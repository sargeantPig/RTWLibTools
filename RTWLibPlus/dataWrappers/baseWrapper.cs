using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class BaseWrapper
    {
        public List<IbaseObj> data = new List<IbaseObj>();
        /// <summary>
        /// Location is a collection of strings that represent the tags. Once the final string is found it will return the coressponding kv
        /// </summary>
        /// <param name="location"></param>
        public KeyValuePair<string, string> GetKeyValueAtLocation(List<IbaseObj> items, int locInd = 0, params string[] location)
        {
            string tag = string.Empty;
            string value = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                tag = ((baseObj)items[i]).Tag;
                value = ((baseObj)items[i]).Value;

                if (location[locInd] == ((baseObj)items[i]).Tag)
                {
                    if (locInd == location.Count() - 1)
                        return new KeyValuePair<string, string>(tag, value);
                    else return GetKeyValueAtLocation(items[i].GetItems(), ++locInd, location);
                }
            }
            return new KeyValuePair<string, string>();
        }

        public bool ModifyValue(List<IbaseObj> items, string newValue, int locInd = 0, bool done = false, params string[] location)
        {
            string tag = string.Empty;
            string value = string.Empty; 

            for (int i = 0; i < items.Count; i++)
            {
                baseObj item = (baseObj)items[i];
                tag = item.Tag;
                value = item.Value;

                if (location[locInd] == item.Tag)
                {
                    if (locInd == location.Count() - 1)
                    {
                        item.Value = newValue;
                        done = true;
                    }
                    else done = ModifyValue(item.GetItems(), newValue, ++locInd, done, location);
                }
            }
            return done;
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

        public List<IbaseObj> GetItemList(List<IbaseObj> items, int locInd = 0, params string[] location)
        {
            string tag = string.Empty;
            string value = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                tag = ((baseObj)items[i]).Tag;
                value = ((baseObj)items[i]).Value;

                if (location[locInd] == ((baseObj)items[i]).Tag)
                {
                    if (locInd == location.Count() - 1)
                        return items[i].GetItems();
                    else return GetItemList(items[i].GetItems(), ++locInd, location);
                }
            }
            return null;
        }
    }
}
