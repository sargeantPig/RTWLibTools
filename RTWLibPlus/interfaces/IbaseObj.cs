namespace RTWLibPlus.interfaces;
using System.Collections.Generic;

public interface IBaseObj
{
    public string Ident { get; set; }
    public string Tag { get; set; }
    public string Value { get; set; }
    public string Output();
    public List<IBaseObj> GetItems();
    public IBaseObj Copy();
}
