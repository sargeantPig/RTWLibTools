using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.interfaces
{
    public interface IWrapper
    {
        public string OutputPath { get; set; }

        public string LoadPath { get; set; }

        public string Output();

        public void Parse(string path = "");
    }
}
