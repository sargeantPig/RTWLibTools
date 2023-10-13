using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RTWLibPlus.data.unit.unit_data
{
    public class Mentality
    {
        /// <summary>
        /// Base morale
        /// </summary>
        public int morale;
        /// <summary>
        /// discipline of the unit (normal, low, disciplined or impetuous)
        /// </summary>
        public string discipline;
        /// <summary>
        /// training of the unit (how tidy the formation is)
        /// </summary>
        public string training;

        public Mentality(int morale, string discipline, string training)
        {
            this.morale = morale;
            this.discipline = discipline;
            this.training = training;
        }
    }

}