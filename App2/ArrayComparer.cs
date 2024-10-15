using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    internal class ArrayComparer
    {
        public bool Greater(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new Exception("Масиви повинні бути однакового розміру.");
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] < array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool Less(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new Exception("Масиви повинні бути однакового розміру.");
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] > array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
