using RTWLibPlus.ds;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTWLibPlus.smf
{
    public class SMF
    {

        public List<IbaseObj> data = new List<IbaseObj>();

        public SMF(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (baseObj obj in data)
            {
                output += obj.Output();
            }
            return output;
        }

        private void Sanitise(List<IbaseObj> toSanitise)
        {
            foreach (baseObj obj in toSanitise)
            {
                obj.Value = obj.Value.Trim();
                obj.Tag = obj.Tag.Trim();
                obj.Ident = obj.Ident.Trim();
                obj.Value = obj.Value.Trim(',');
                obj.Value = obj.Value.Trim(':');
                obj.Value = obj.Value.Trim('"');
                obj.Tag = obj.Tag.Trim(',');
                obj.Tag = obj.Tag.Trim(':');
                obj.Tag = obj.Tag.Trim('"');
                obj.Ident = obj.Ident.Trim(',');
                obj.Ident = obj.Ident.Trim(':');
                obj.Ident = obj.Ident.Trim('"');
                Sanitise(obj.GetItems());
            }
        }
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
            return new KeyValuePair<string, string>(tag, value);
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

            return new KeyValuePair<string, string>(tag, value);
        }
    }
}
