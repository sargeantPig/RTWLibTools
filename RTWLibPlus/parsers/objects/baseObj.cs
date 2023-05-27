using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Collections.Generic;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public abstract class BaseObj: IbaseObj
    {
        public WhiteSpaceConfig wsConfig;
        public List<IbaseObj> items = new List<IbaseObj>();
        
        public string Tag { get; set; } = string.Empty;
        public string Ident { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int depth { get; set; }

        public int newLinesAfter { get; set; } = 0;

        public BaseObj() { }

        public BaseObj(string tag, string value, int depth)
        {
            this.Tag = tag;
            this.Value = value;
            this.depth = depth;
            this.Ident = Tag.Split(wsConfig.WhiteChar)[0];
        }

        public abstract IbaseObj Copy();


        public abstract string Output();

        public List<IbaseObj> GetItems()
        {
            return items;
        }

        public string NormalFormat(char whiteSpace, int end)
        {
            return String.Format("{0}{1} {2}{3}",
                Format.GetWhiteSpace("", end, whiteSpace),
                Tag, Value,
                Environment.NewLine);
        }

        public string IgnoreValue(char whiteSpace, int end)
        {
            return String.Format("{0}{1}{2}",
                Format.GetWhiteSpace("", end, whiteSpace),
                Tag,
                Environment.NewLine);
        }
    }   
}
