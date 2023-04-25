using System.Collections.Generic;


namespace RTWLibPlus.interfaces
{
    public interface IbaseObj
    {
        public string Output();
        public List<IbaseObj> GetItems();
        public void SetItems(List<IbaseObj> values);
    }
}
