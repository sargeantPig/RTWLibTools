using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.data.unit.unit_data
{
    public class StatPriArmour
    {
        /// <summary>
        /// [1] armour factor
        /// [2] defensive skill factor
        /// [3] shield factor
        /// </summary>
        public int[] priArm;
        /// <summary>
        /// sound type when hit = flesh, leather, or metal
        /// </summary>
        public string armSound;

        public StatPriArmour()
        {
            priArm = new int[3];
        }
    }
}
