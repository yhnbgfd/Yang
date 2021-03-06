﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1.Method
{
    class GenerateData
    {
        List<int[]>  l_totalDataBase = new List<int[]>();

        public int s;                                       //s为要取的个数
        public int e;                                       //e为要取得基数的总数
        public List<int> dataSets;                          
        //Algorithm2 start
        public int[] x;
        List<int[]> alInt = new List<int[]>();
        List<Color[]> ColorValue = new List<Color[]>();

        public List<int[]> run2()
        {
            Algorithm2(s, e);
            for (int i = 0; i < alInt.Count(); i++)
            {
                int[] tvalue = new int[s];
                for (int j = 0; j < s; j++)
                {
                    tvalue[j] = alInt[i][j];
                }
                l_totalDataBase.Add(tvalue);
            }
            return l_totalDataBase;
        }

        public List<Color[]> runColor()
        {
            int count = CountTotalData(e,s);
            Color[] cc = new Color[s];
            for (int j = 0; j < s; j++)
            {
                cc[j] = Color.White;
            }
            for (int i = 0; i < count; i++)
            {
                ColorValue.Add(cc);
            }
            return ColorValue;
        }

        /* 分支2，计算生成的总数据量 */
        public int CountTotalData(int Dividend, int Divisor)
        {
            long eDividend = Dividend;
            long eDivisor = Divisor;
            for (int i = 1; i < Divisor; i++)
            {
                eDividend *= Dividend - i;
                eDivisor *= Divisor - i;
            }
            double resule = (double)eDividend / (double)eDivisor;
            return ((int)resule);
        }

        void Algorithm2(int aa, int bb)//bb取aa
        {
            x = new int[aa];
            for (int i = 0; i < bb; i++)
            {
                x[0] = dataSets[i];
                for (int j = 1 + i; j < bb; j++)
                {
                    x[1] = dataSets[j];
                    if (aa >= 3)
                    for (int k = 1 + j; k < bb; k++)
                    {
                        x[2] = dataSets[k];
                        if (aa >= 4)
                            for (int l = 1 + k; l < bb; l++)
                            {
                                x[3] = dataSets[l];
                                if (aa >= 5)
                                    for (int m = 1 + l; m < bb; m++)
                                    {
                                        x[4] = dataSets[m];
                                        if (aa >= 6)
                                            for (int n = 1 + m; n < bb; n++)
                                            {
                                                x[5] = dataSets[n];
                                                if (aa >= 7)
                                                    for (int o = 1 + n; o < bb; o++)
                                                    {
                                                        x[6] = dataSets[o];
                                                        if (aa >= 8)
                                                            for (int v = 1 + o; v < bb; v++)
                                                            {
                                                                x[7] = dataSets[v];
                                                                output();
                                                            }
                                                        else
                                                            output();
                                                    }
                                                else
                                                    output();
                                            }
                                        else
                                            output();
                                    }
                                else
                                    output();
                            }
                        else
                            output();
                    }
                else
                    output();
                }

            }
        }
        private void output()
        {
            int[] i_test = new int[s];
            for (int i = 0; i < s; i++)
                i_test[i] = x[i];
            alInt.Add(i_test);
        }
        //end Algorithm2
    }
}
