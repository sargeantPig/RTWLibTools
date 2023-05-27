using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Collections.Generic;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public abstract class BaseObj : IbaseObj
    {
        public WhiteSpaceConfig wsConfig;

        public List<IbaseObj> items = new List<IbaseObj>();

        private ObjRecord Record;
   
        public BaseObj() { }

        public BaseObj(string tag, string value, int depth)
        {
            Record = new ObjRecord(tag, tag.Split(wsConfig.WhiteChar)[0], value, depth);
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
                Record.Tag, Record.Value,
                Environment.NewLine);
        }
        public string IgnoreValue(char whiteSpace, int end)
        {
            return String.Format("{0}{1}{2}",
                Format.GetWhiteSpace("", end, whiteSpace),
                Record.Tag,
                Environment.NewLine);
        }

        public string Tag
        {
            get { return Record.Tag; }
            set { Record.Tag = value; }
        }
        public string Ident
        {
            get { return Record.Ident; }
            set { Record.Ident = value; }
        }
        public string Value
        {
            get { return Record.Value; }
            set { Record.Value = value; }
        }
        public int NewLinesAfter
        {
            get { return Record.NewLinesAfter; }
            set { Record.NewLinesAfter = value; }
        }
        public int Depth
        {
            get { return Record.Depth; }
            set { Record.Depth = value; }
        }

    }
}
