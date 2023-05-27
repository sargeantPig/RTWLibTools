using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using RTWLibPlus.parsers.configs;
using RTWLibPlus.parsers.configs.whiteSpace;

namespace RTWLibPlus.parsers.objects
{
    public class DRObj : BaseObj, IbaseObj
    {
        public DRObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            WSConfigFactory factory = new WSConfigFactory();
            wsConfig = factory.Create_DR_DS_SMF_WhiteSpace();
            Ident = Tag.Split(wsConfig.WhiteChar)[0];
        }

        public DRObj(){}

        public override IbaseObj Copy()
        {
            DRObj copy = new DRObj();
            copy.wsConfig.WhiteChar = wsConfig.WhiteChar;
            copy.Depth = Depth;
            copy.items = items.DeepCopy();
            copy.wsConfig.WhiteDepthMultiplier = wsConfig.WhiteDepthMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.NewLinesAfter = NewLinesAfter;
            return copy;
        }

        public override string Output() { return "Not Implemented"; }

    }
}
