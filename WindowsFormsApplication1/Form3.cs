using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        #region define const param
                             
        #endregion

        public int[] awardNumber;                       //中奖号码
        private int m = 0;
        private int n = 0;
        private int range = 0;
        private int mod = 0;                            //余数
        private int divisor = 0;                        //除数
        private int rowid = 0;
        private int mark = 8;                           //单元格下标
        private int count = 1;                            //从第二行开始排列的计数器
        private List<int[]> allNumbersByX;
        private List<int> _list;
        private int x = 0;                                  //项长
        private Color default_color = Color.White;
        private int[] tempt;
        private Object o = null;
        private bool flag;
        private int dateCount = 0; //初始化的期数值
        public Form3(int mtemp,int ntemp)
        {
            InitializeComponent();
            this.m = mtemp;
            this.n = ntemp;
            this.label15.Text = (mtemp == 26) ? "9阶分组" : "12阶分组";
            InitialDisplay();
        }
        private void InitialDisplay()
        {
            for (int i = 1; i < 8; i++)
            {
                this.dataGridView1.Rows[0].Cells[i].ReadOnly = true;
                this.dataGridView1.Rows[0].Cells[i].Style.BackColor = Color.Gray;
            }
        }
        //初始化数组
        private List<int> initArray(int rid)
        {
            List<int> temp_li = new List<int>();
            for (int i = 0; i < m; i++)
            {
                Object e = dataGridView1.Rows[rid].Cells[8 + i].Value;
                temp_li.Add(int.Parse(e.ToString()));
            }
            return temp_li;
        }
        //N阶分组  && 分组
        private void sortToGroupByN()
        {
            string s;
            int temp = 0;
            List<string> strlistOne = new List<string>();
            List<string> strlistTwo = new List<string>();
            List<string> strlistThree = new List<string>();
            int[] sortCount = new int[3];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < awardNumber.Length; j++)
                {
                    if (_list[i] == awardNumber[j])
                    {
                        s = (i / 3+1).ToString();
                        if (i % 3 == 0)
                        {
                            strlistOne.Add(s);
                        }
                        if (i % 3 == 1)
                        {
                            strlistTwo.Add(s);
                        }
                        if (i % 3 == 2)
                        {
                            strlistThree.Add(s);
                        }
                    }
                }
            }
            sortCount[0] = strlistOne.Count();
            sortCount[1] = strlistTwo.Count();
            sortCount[2] = strlistThree.Count();
            strlistOne.AddRange(strlistTwo);
            strlistOne.AddRange(strlistThree);
            strlistOne.ToArray();
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows[rowid-1].Cells[44 + i].Value = strlistOne[i];
            }
            temp = sortCount[1];
            if (temp > 0)
            {
                for (int i = sortCount[0]; i < n - sortCount[2]; i++)
                {
                    dataGridView1.Rows[rowid - 1].Cells[44 + i].Style.ForeColor = Color.Red;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                 dataGridView1.Rows[rowid - 1].Cells[51 + i].Value = sortCount[i].ToString();
            }
        }
        //阶段
        private void stage()
        {
            int count123 = 0;
            int count456 = 0;
            int count789 = 0;
            int count_el = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < awardNumber.Length; j++)
                {
                    if (_list[i] == awardNumber[j])
                    {
                        if (i / 3 < 3)
                        {
                            count123++;
                        }
                        else if (i / 3 < 6)
                        {
                            count456++;
                        }
                        else if (i / 3 < 9)
                        {
                            count789++;
                        }else
                        {
                            count_el++;
                        }
                    }
                }
            }
            List<string> list = new List<string>();
            list.Add(count123.ToString());
            list.Add(count456.ToString());
            list.Add(count789.ToString());
            list.Add(count_el.ToString());
            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Rows[rowid-1].Cells[54 + i].Value = list[i];
            }

        }
        //阶错
        private void factorialError()
        {
            int count147 = 0;
            int count258 = 0;
            int count369 = 0;
            int count_el = 0;
            int k = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < awardNumber.Length; j++)
                {
                    if (_list[i] == awardNumber[j])
                    {
                        k = i / 3;
                        switch (k)
                        {
                            case 0:
                                count147++;
                                break;
                            case 1:
                                count258++;
                                break;
                            case 2:
                                count369++;
                                break;
                            case 3:
                                count147++;
                                break;
                            case 4:
                                count258++;
                                break;
                            case 5:
                                count369++;
                                break;
                            case 6:
                                count147++;
                                break;
                            case 7:
                                count258++;
                                break;
                            case 8:
                                count369++;
                                break;
                            default:
                                count_el++;
                                break;
                        }
                    }
                }
            }
            List<string> list = new List<string>();
            list.Add(count147.ToString());
            list.Add(count258.ToString());
            list.Add(count369.ToString());
            list.Add(count_el.ToString());
            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Rows[rowid-1].Cells[58 + i].Value = list[i];
            }
        }
        //目
        private void flagCount()
        {
            int mu = 0;//目
            int cheng = 0;//承
            int ge = 0; //个
            int qi = 0;//旗目
            List<int> plist = new List<int>();//上期阶的list
            List<int> nlist = new List<int>();//本期阶的list

            nlist = findIndex(rowid-1);
            //生成目
           
            mu = distinctArray(nlist).Count();
            //生成承
            if (rowid-2>=0)
            {
                plist = findIndex(rowid - 2);
                cheng = produce(distinctArray(plist), distinctArray(nlist))[0];

                //生成个
                ge = createGe(produce(distinctArray(plist), distinctArray(nlist))[1], nlist);
                //旗目
                qi = getQi(plist, nlist);
            }
            dataGridView1.Rows[rowid].Cells[62].Value = mu.ToString();
            if(string.IsNullOrEmpty(cheng.ToString()) == false)
            {
                dataGridView1.Rows[rowid - 1].Cells[63].Value = cheng.ToString();    //你妹,这里重复了，仔细检查,现在承是计算对了,注意显示的位置
            }

            if (string.IsNullOrEmpty(ge.ToString()) == false)
            {
                dataGridView1.Rows[rowid - 1].Cells[64].Value = ge.ToString();       //个也要这样处理位置,具体逻辑你自己去检查,个的显示不正确
            }

            if (string.IsNullOrEmpty(qi.ToString()) == false)
            {
                dataGridView1.Rows[rowid - 1].Cells[65].Value = qi.ToString();       //旗也要这样处理位置,具体逻辑你自己去检查,旗的显示不正确
            }              
        }
        private int getQi(List<int> list1,List<int> list2)
        {
            int count = 0;
            int i =0;
            while (i < list1.Count())
            {
                if (i == 0)
                {
                    foreach (int x in list2)
                    {
                        if (x < list1[i])
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    foreach (int x in list2)
                    {
                        if (x < list1[i]&&x>=list1[i-1])
                        {
                            count++;
                        }
                    }
                }
                i++;
            }
            //特殊情况
            if (list2[n - 1] < m && list2[n - 1] >= list1[n - 1])
            {
                count++;
            }
            return count;
        }
        //获取下标
        private List<int> findIndex(int rowid)
        {
            List<int> count = new List<int>();
            for (int i = 0; i < m; i++)
            {
                if (dataGridView1.Rows[rowid].Cells[8 + i].Style.ForeColor == Color.Red)
                {
                    count.Add(i);
                }
            }
            return count;
        }
        //生成承
        private int[] produce(List<int> a, List<int> b)
        {
            int[] count = new int[2];
            for (int i = 0; i < a.Count(); i++)
            {
                for (int j = 0; j < b.Count(); j++)
                {
                    if (a[i] == b[j])
                    {
                        count[0]++;
                        count[1] = a[i];
                    }
                }
            }
            return count;
        }
        //生成个
        private int createGe(int x,List<int> _nlist)//_nlist表示当前期
        {
            int count = 0;
            foreach (int i in _nlist)
            {
                int y = i / 3;
                if (x == y)
                {
                    count++;
                }
            }
            return count;
        }
        //获取组
        private List<int> distinctArray(List<int> li)
        {
            List<int> a = new List<int>();
            foreach (int number in li)                          //这样去循环遍历，这是自动的，以后LIST和DICTIONARY都这样写遍历
            {
                int arrayNumber = number / 3;
                if (a.Contains(arrayNumber) == false)
                {
                    a.Add(arrayNumber);
                }
            }
            return a;
        }
        //中奖号码突出
        private void paint(int index,int count)
        {
            for(int i = 0;i<count;i++)
            {
                this.dataGridView1.Rows[rowid].Cells[index + i].Style.ForeColor = Color.Red;
                this.dataGridView1.Rows[rowid].Cells[index + i].Style.Font = new Font("宋体", 8, FontStyle.Underline | FontStyle.Bold);
            }
        }
        //排列算法
        private List<int[]> change(List<int[]> allNumsByX)
        {
            List<int[]> list = new List<int[]>(allNumsByX.Count());
            int index = 8;
            int count = 0;
            for (int i = 0; i < allNumsByX.Count(); i++)
            {
                count = 0;
                List<int> temptAwardNumbers = new List<int>();
                List<int> temptNoAwardNumbers = new List<int>();
                int[] targetArray = new int[allNumsByX[i].Length];
                for (int k = 0; k < allNumsByX[i].Length; k++)
                {
                    bool isTarget = false;                    
                    for (int j = 0; j < awardNumber.Length; j++)
                    {
                        if (allNumsByX[i][k] == awardNumber[j])
                        {
                            isTarget = true;
                            count++;
                        }
                    }
                    if (isTarget == true)
                    {
                        temptAwardNumbers.Add(allNumsByX[i][k]);
                    }
                    else
                    {
                        temptNoAwardNumbers.Add(allNumsByX[i][k]);
                    }
                }
                index += allNumsByX[i].Length;
                temptAwardNumbers.AddRange(temptNoAwardNumbers);
                targetArray = temptAwardNumbers.ToArray();
                allNumbersByX[i] = targetArray;
            }
            list = allNumsByX;
            return list;
        }
        //写数据
        private void insertData(List<int[]> list,int rowid)
        {
            int f = 8;
            for (int i = 0; i < list.Count(); i++)
            {
                for (int k = 0; k < list[i].Length; k++)
                {
                    this.dataGridView1.Rows[rowid].Cells[f].Value = list[i][k].ToString();
                    f++;
                }
            }
             
        }
        private Boolean init(int divisor,int _mod,int _range)
        {
            allNumbersByX = new List<int[]>(_range);
            Object o = null;
            allNumbersByX = new List<int[]>(divisor);
            for (int i = 0; i < _range; i++)//分为几项
            {
                if (_mod != 0)
                {
                    x = divisor + 1;
                    tempt = new int[x];
                    int k = 0;
                    for (int j = mark; j < mark + x; j++)
                    {
                        o = this.dataGridView1.Rows[0].Cells[j].Value;
                        if (o != null)
                        {
                            tempt[k] = int.Parse(o.ToString());
                        }
                        else
                        {
                            return false;
                        }

                        k++;
                    }

                    mark += x;
                    _mod--;
                    allNumbersByX.Add(tempt);
                }
                else
                {
                    x = divisor;
                    tempt = new int[x];
                    int k = 0;
                    for (int j = mark; j < mark + x; j++)
                    {
                        o = this.dataGridView1.Rows[0].Cells[j].Value;
                        if (o != null)
                        {
                            tempt[k] = int.Parse(o.ToString());
                        }
                        else
                        {
                            return false;
                        }
                        k++;
                    }
                    mark += x;
                    allNumbersByX.Add(tempt);
                }
            }
            mark = 8;
            return true;
        }
        private Boolean init2(int divisor, int _range)
        {
            allNumbersByX = new List<int[]>(_range);
            for (int i = 0; i < _range; i++)//分为几项
            {
                tempt = new int[divisor];
                int k = 0;
                for (int j = i * divisor + 8; j < (i + 1) * divisor + 8; j++)
                {
                    o = this.dataGridView1.Rows[0].Cells[j].Value;
                    if (o != null)
                    {
                        tempt[k] = int.Parse(o.ToString());
                    }
                    else
                    {
                        return false;
                    }
                    k++;
                }
                allNumbersByX.Add(tempt);
            }
            return true;
        }
        //喷色1,当余数不为0是喷色方案
        private void paintColor(int rowid, int divisor, int _mod, int _range)
        {
            for (int i = 0; i < _range; i++)//分为几项
            {
                this.default_color = (i % 2 == 0) ? Color.LightBlue : Color.White;
                if (_mod != 0)
                {
                    x = divisor + 1;
                    for (int j = mark; j < mark + x; j++)
                    {
                        this.dataGridView1.Rows[rowid].Cells[j].Style.BackColor = this.default_color;
                    }
                    mark += x;
                    _mod--;
                }
                else
                {
                    x = divisor;
                    for (int j = mark; j < mark + x; j++)
                    {
                        this.dataGridView1.Rows[rowid].Cells[j].Style.BackColor = this.default_color;
                    }
                    mark += x;
                }
            }
            mark = 8;
        }
        //喷色2,当余数为0是喷色方案
        private void paintColor2(int rowid, int divisor, int _range)
        {
            for (int i = 0; i < _range; i++)//分为几项
            {
                this.default_color = (i % 2 == 0) ? Color.Blue : Color.White;
                for (int j = i * divisor + 8; j < (i + 1) * divisor + 8; j++)
                {
                    this.dataGridView1.Rows[rowid].Cells[j].Style.BackColor = this.default_color;
                }
            }
        }
        private void paintRow(int rowid)
        {
                List<int> templist =new List<int>();
                templist = initArray(rowid-1);
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (templist[i] == awardNumber[j])
                        {
                            dataGridView1.Rows[rowid-1].Cells[i + 8].Style.ForeColor = Color.Red;
                        }
                    }
                }
            
        }
        //初始化button事件
        private void button1_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool bo = String.IsNullOrEmpty(comboBox1.Text);
            bool tb1 = String.IsNullOrEmpty(textBox1.Text);
            if (tb1)
            {
                MessageBox.Show("请输入初始化的期数！");
                return;
            }
            if (bo)
            {
                MessageBox.Show("请输入项的值！");
                return;
            }
            else
            {
                this.range = int.Parse(this.comboBox1.Text.ToString());
                dateCount = int.Parse(textBox1.Text.ToString());
                dataGridView1.Rows[0].Cells[0].Value = dateCount.ToString();
                mod = m % range;
                divisor = m / range;
                if (mod == 0)
                {
                    paintColor2(0, divisor, range);
                    flag = init2(divisor, range);
                }
                else
                {
                    paintColor(0, divisor, mod, range);
                    flag = init(divisor, mod, range);
                }
                if (flag)
                {
                    dataGridView1.Rows[1].Cells[0].Value = (dateCount + rowid).ToString();
                    MessageBox.Show("初始化参数成功！");
                    rowid++;
                }
                else
                {
                    MessageBox.Show("初始化失败!请检查输入的数据!");
                }
            }
        }
        //排列事件
        private void button2_Click(object sender, EventArgs e)
        {
            bool fl = true;
            if (flag)
            {
                List<int[]> newList = new List<int[]>(range);
                awardNumber = new int[n];
                for (int i = 0; i < n; i++)
                {
                    o = this.dataGridView1.Rows[count].Cells[i + 1].Value;
                    if (o != null)
                    {
                        awardNumber[i] = int.Parse(o.ToString());
                    }
                    else
                    {
                        fl = false;
                        break;
                    }

                }
                if (fl == false)
                {
                    MessageBox.Show("请在第" + count + "行中输入此期中奖号码！");
                }
                else
                {
                    paintRow(rowid);
                    this.mark = 8;
                    mod = m % range;
                    if (mod == 0)
                    {
                        paintColor2(rowid, divisor, range);
                    }
                    else
                    {
                        paintColor(rowid, divisor, mod, range);
                    }

                    newList = change(allNumbersByX);
                    insertData(newList, count);


                    _list = new List<int>();
                    _list = initArray(rowid - 1);//

                    sortToGroupByN();
                    stage();
                    factorialError();
                    flagCount();
                    count++;
                    rowid++;
                    dataGridView1.Rows[rowid].Cells[0].Value = (dateCount + rowid).ToString();
                }
            }
            else
            {
                MessageBox.Show("请初始化数据后再点击排列按钮！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            //dataGridView1.Columns.Clear();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

    }
}
