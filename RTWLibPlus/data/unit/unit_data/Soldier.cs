using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.data.unit.unit_data
{
    public class Soldier
    {
        /// <summary>
        /// name of the soldier model to use
        /// </summary>
        public string name;
        /// <summary>
        /// number of ordinary soldiers in the unit
        /// </summary>
        public int number;
        /// <summary>
        /// number of extras (pigs, dogs, elephants, chariots) attached to the unit
        /// </summary>
        public int extras;
        /// <summary>
        /// collision mass of the men. 1.0 is normal. Only applies to infantry
        /// </summary>
        public float collMass;

        public Soldier()
        {
        }
    }
}
