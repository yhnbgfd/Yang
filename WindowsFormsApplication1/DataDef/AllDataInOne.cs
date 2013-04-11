using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1.DataDef
{
    class AllDataInOne
    {
        public List<int[]> l_totalDataBase = new List<int[]>(); //生成的具体数据组合
        public int[] FilterStatistics;                          //普通标记
        public int[] SpecialMark;                               //特殊五角星标记
        public int n;                                           //m取n
        public int m;                                           //m取n
        public List<int> choiceDate = new List<int>();          //选择的数字
        public int totalData = 0;                               //生成数据的总量
    }
}
