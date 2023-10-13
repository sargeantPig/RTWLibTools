using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.data.unit.unit_data
{
    public class Formation
    {
        /// <summary>
        /// [0] soldier spacing (in meters) side to side
        /// [1] soldier spacing back to back
        /// </summary>
        public float[] formationTight;
        /// <summary>
        /// [0] soldier spacing (in meters) side to side
        /// [1] soldier spacing back to back
        /// </summary>
        public float[] formationSparse;
        /// <summary>
        /// number of ranks in the formation
        /// </summary>
        public int formationRanks;
        /// <summary>
        /// special formations that are possible. one or two of square, horde, phalanx, testudo or wedge
        /// </summary>
        public string formations;

        public Formation()
        {
            formationTight = new float[2];
            formationSparse = new float[2];
        }
    }
}
