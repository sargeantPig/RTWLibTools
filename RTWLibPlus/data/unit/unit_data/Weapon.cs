using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.data.unit.unit_data
{
    public class StatWeapons
    {
        /// <summary>
        /// [0] attack factor
        /// [1] attack bonus factor if charging
        /// </summary>
        public int[] atk;
        /// <summary>
        /// missile type fired (no if not a missile weapon type)
        /// </summary>
		public string missType;
        /// <summary>
        /// [0] range of missile
        /// [1] amount of missle ammunition per man
        /// </summary>
		public int[] missAttr;
        /// <summary>
        ///  Weapon type = melee, thrown, missile, or siege_missile
        /// </summary>
		public string WepFlags;
        /// <summary>
        /// Tech type = simple, other, blade, archery or siege
        /// </summary>
		public string TechFlags;
        /// <summary>
        /// Damage type = piercing, blunt, slashing or fire. (I don't think this is used anymore)
        /// </summary>
        public string DmgFlags;
        /// <summary>
        /// Sound type when weapon hits = none, knife, mace, axe, sword, or spear
        /// </summary>
		public string SoundFlags;
        /// <summary>
        /// Min delay between attacks(in 1/10th of a second)
        /// </summary>
		public float[] atkDly;

        public StatWeapons()
        {
            atk = new int[2];
            missAttr = new int[2];
            atkDly = new float[2];
        }
    }
}
