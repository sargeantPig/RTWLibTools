using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public KeyValuePair<string, string> GetKeyValueAtLocation(params string[] location)
        {
            string tag = string.Empty;
            string value = string.Empty;
            int locInd = 0;
            for (int i = 0; i < data.Count; i++)
            {
                tag = ((baseObj)data[i]).Tag;
                value = ((baseObj)data[i]).Value;

                if (location[locInd] == ((baseObj)data[i]).Tag)
                {
                    if (locInd == location.Count() - 1)
                        return new KeyValuePair<string, string>(tag, value);
                    else return GetKeyValueAtLocationDepth(location, ++locInd, data[i].GetItems());
                }
            }
            return new KeyValuePair<string, string>();
        }
        private KeyValuePair<string, string> GetKeyValueAtLocationDepth(string[] location, int locInd, List<IbaseObj> items)
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
                    else return GetKeyValueAtLocationDepth(location, ++locInd, items[i].GetItems());
                }
            }

            return new KeyValuePair<string, string>();
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
