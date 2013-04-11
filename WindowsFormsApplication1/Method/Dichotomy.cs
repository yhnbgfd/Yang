using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1.Method
{
    class Dichotomy
    {
        /**
         * 二分法 
         */
        public int Find(int[] WinNum, List<int[]> l_totalDataBase, int totalData)
        {
            int Location = 0;
            int min = 0;
            int max = totalData - 1;    //总数减一
            int middle = (max-min) / 2 + min;
            bool isbigger = false;
            while (!checkdiff(WinNum, l_totalDataBase[middle]))
            {
                if (checkdiff(WinNum, l_totalDataBase[middle+1]))
                {
                    return middle + 1;
                }
                for (int i = 0; i < WinNum.Length; i++)
                {
                    if (WinNum[i] > l_totalDataBase[middle][i])
                    {
                        isbigger = true;
                        break;
                    }
                    else if (WinNum[i] < l_totalDataBase[middle][i])
                    {
                        isbigger = false;
                        break;
                    }
                }
                if (isbigger)
                {
                    min = middle;
                }
                else
                {
                    max = middle;
                }
                middle = (max - min) / 2 + min;
            }
            Location = middle;
            return Location;
        }

        private bool checkdiff(int[] a, int[] b)
        {
            bool resule = true;
            for (int i = 0; i < a.Length; i++ )
            {
                if (a[i] != b[i])
                {
                    resule = false;
                }
            }
            return resule;
        }
    }
}
