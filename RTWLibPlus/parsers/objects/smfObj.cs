using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public class SMFObj: BaseObj, IbaseObj
    {
        public SMFObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            WSConfigFactory factory = new WSConfigFactory();
            wsConfig = factory.Create_DR_DS_SMF_WhiteSpace();
            Ident = Tag.Split(wsConfig.WhiteChar)[0];
        }

        public SMFObj() { }

        public override IbaseObj Copy()
        {
            SMFObj copy = new SMFObj();
            copy.wsConfig.WhiteChar = wsConfig.WhiteChar;
            copy.depth = depth;
            copy.items = items.DeepCopy();
            copy.wsConfig.WhiteDepthMultiplier = wsConfig.WhiteDepthMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.newLinesAfter = newLinesAfter;
            return copy;
        }

        public override string Output() { return "Not Implemented"; }
    }
}
