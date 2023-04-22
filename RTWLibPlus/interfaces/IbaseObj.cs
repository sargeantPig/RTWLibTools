using RTWLibPlus.edb;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace RTWLibPlus.interfaces
{
    public interface IbaseObj
    {
        public string Output();
        public List<IbaseObj> GetItems();
        public void SetItems(List<IbaseObj> values);
    }
}
