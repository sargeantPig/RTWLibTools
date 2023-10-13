using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.data.unit.unit_data
{
    public class MountEffect
    {
        /// <summary>
        /// mount types that this unit has bonuses against
        /// </summary>
        public List<string> mountType;
        /// <summary>
        /// the bonus or negative for each mount type
        /// </summary>
        public List<int> modifier;

        public MountEffect()
        {
            mountType = new List<string>();
            modifier = new List<int>();
        }
    }
}
