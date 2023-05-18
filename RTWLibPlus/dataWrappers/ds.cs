using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class DS : BaseWrapper, IWrapper
    {
        

        public DS() {
            LoadPath = RemasterRome.GetPath(false, "ds");
            OutputPath = RemasterRome.GetPath(true, "ds");
        }

        public DS(List<IbaseObj> data)
        {
            this.data = data;
            SetLastOfGroup();
            LoadPath = RemasterRome.GetPath(false, "ds");
            OutputPath = RemasterRome.GetPath(true, "ds");
        }

        public void Parse(string path = "") {
            this.data = RFH.ParseFile(Creator.DScreator, ' ', false, LoadPath);
            SetLastOfGroup();
        }


        public string Output()
        {
            string output = string.Empty;
            foreach (DSObj obj in data)
            {
                output += obj.Output();
            }
            RFH.Write(OutputPath, output);
            return output;
        }

        private void SetLastOfGroup()
        {
            string previous = string.Empty;
            foreach (DSObj obj in data)
            {
                string ident = obj.Tag.Split(DSObj.whiteSpace)[0];

                if (ident != previous)
                    obj.lastOfGroup = true;

                previous = ident;
            }
        }

        public static string ChangeCharacterCoordinates(string character, Vector2 coords)
        {
            string[] split = character.Split(',').TrimAll();
            string x = string.Format("x {0}", (int)coords.X);
            string y = string.Format("y {0}", (int)coords.Y);
            split[split.Length - 1] = y;
            split[split.Length - 2] = x;
            return split.ToString(',', ' ');
        }
    }
}
