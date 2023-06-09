﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RTWLibPlus.interfaces
{
    public interface IWrapper
    {
        public string OutputPath { get; set; }

        public string LoadPath { get; set; }

        public string Output();

        public void Parse();

        public void Clear();

        public string GetName();
    }
}
