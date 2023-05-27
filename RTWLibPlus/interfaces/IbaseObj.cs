using RTWLibPlus.parsers.objects;
using System.Collections.Generic;


namespace RTWLibPlus.interfaces
{
    public interface IbaseObj
    {
        public string Ident { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }
        public string Output();
        public List<IbaseObj> GetItems();
        public IbaseObj Copy();
    }
}
