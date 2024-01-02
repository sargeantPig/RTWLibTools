﻿namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System.Collections.Generic;

public abstract class BaseObj : IBaseObj
{
    private WhiteSpaceConfig whiteSpaceConfig;
    private ObjRecord record;

    public BaseObj() { }

    public BaseObj(string tag, string value, int depth) => this.record = new ObjRecord(tag, tag.Split(this.WhiteSpaceConfig.WhiteChar)[0], value, depth);

    public abstract IBaseObj Copy();
    public abstract string Output();
    public List<IBaseObj> GetItems() => this.Items;

    public int FirstOfIndex(string find)
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            IBaseObj item = this.Items[i];
            if (item.Ident == find)
            {
                return i;
            }

            else if (item.GetItems().Count > 0)
            {
                int result = item.FirstOfIndex(find);
                if (result != -1)
                { return result; }
            }
        }
        return -1;
    }

    public void AddToItems(IBaseObj objToAdd) => this.Items.Add(objToAdd);

    public void InsertToItems(IBaseObj objToAdd, int index) => this.Items.Insert(index + 1, objToAdd);

    public bool FindAndModify(string find, string modto)
    {
        foreach (IBaseObj item in this.Items)
        {
            if (item.Ident == find)
            {
                item.Value = modto;
                return true;
            }

            else if (item.GetItems().Count > 0)
            {
                if (item.FindAndModify(find, modto))
                { return true; }
            }
        }
        return false;
    }

    public string Find(string find)
    {
        foreach (IBaseObj item in this.Items)
        {
            if (item.Ident == find)
            {
                return item.Value;
            }

            else if (item.GetItems().Count > 0)
            {
                string result = item.Find(find);
                if (result != "null")
                { return result; }
            }
        }
        return "null";
    }

    public IBaseObj GetObject(string find)
    {
        foreach (IBaseObj item in this.Items)
        {
            if (item.Ident == find)
            {
                return item;
            }

            else if (item.GetItems().Count > 0)
            {
                IBaseObj result = item.GetObject(find);
                if (result != null)
                { return result; }
            }
        }
        return null;
    }
    public string NormalFormat(char whiteSpace, int end) => string.Format("{0}{1} {2}{3}",
            Format.GetWhiteSpace("", end, whiteSpace),
            this.record.Tag, this.record.Value,
            Format.UniversalNewLine());
    public string IgnoreValue(char whiteSpace, int end) => string.Format("{0}{1}{2}",
            Format.GetWhiteSpace("", end, whiteSpace),
            this.record.Tag,
            Format.UniversalNewLine());

    public string Tag
    {
        get => this.record.Tag;
        set => this.record.Tag = value;
    }
    public string Ident
    {
        get => this.record.Ident;
        set => this.record.Ident = value;
    }
    public string Value
    {
        get => this.record.Value;
        set => this.record.Value = value;
    }
    public int NewLinesAfter
    {
        get => this.record.NewLinesAfter;
        set => this.record.NewLinesAfter = value;
    }
    public int Depth
    {
        get => this.record.Depth;
        set => this.record.Depth = value;
    }
    public WhiteSpaceConfig WhiteSpaceConfig
    {
        get => this.whiteSpaceConfig;
        set => this.whiteSpaceConfig = value;
    }

    public char WhiteSpaceChar
    {
        get => this.whiteSpaceConfig.WhiteChar;
        set => this.whiteSpaceConfig.WhiteChar = value;
    }

    public int WhiteSpaceMultiplier
    {
        get => this.whiteSpaceConfig.WhiteDepthMultiplier;
        set => this.whiteSpaceConfig.WhiteDepthMultiplier = value;
    }

    public List<IBaseObj> Items { get; set; } = new();


}
