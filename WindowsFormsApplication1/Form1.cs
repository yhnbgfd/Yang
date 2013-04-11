using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsApplication1.Method;
using WindowsFormsApplication1.DataDef;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region valueDef

        private Dictionary<int, Color> D_ColorTemptDict = new Dictionary<int, Color>();
        private List<int> choiceDate = new List<int>();             //定义获取勾选的数字
        
        private int SelectNum = 5;
        private int TotalNum = 26;

        private Color ChoiceColor = Color.Red;

        private int m_Count_Operate;
        private int m_Count_Generate;

        private int[] Winning_Numbers;             //中奖号码
        private string path = Application.StartupPath + @"\";           //软件目录
        private int CurrentPage = 0;                        //当前打印的页面
        private string CurrentMarkforPrint = "";

        private string[] sort = {   "0", 
                                "△", "△△", 
                                "■", "△■", "△△■", 
                                "■■", "△■■", "△△■■", 
                                "☆", "△☆", "△△☆", "■☆", "△■☆", "△△■☆", "■■☆", "△■■☆", "△△■■☆", 
                                "☆☆", "△☆☆", "△△☆☆", "■☆☆", "△■☆☆", "△△■☆☆", "■■☆☆", "△■■☆☆", "△△■■☆☆", 
                                "☆☆☆", "△☆☆☆", "△△☆☆☆", "■☆☆☆", "△■☆☆☆", "△△■☆☆☆", "■■☆☆☆", "△■■☆☆☆", "△△■■☆☆☆", 
                                "☆☆☆☆", "△☆☆☆☆", "△△☆☆☆☆", "■☆☆☆☆", "△■☆☆☆☆", "△△■☆☆☆☆", "■■☆☆☆☆", "△■■☆☆☆☆", "△△■■☆☆☆☆", 
                                "☆☆☆☆☆", "△☆☆☆☆☆", "△△☆☆☆☆☆", "■☆☆☆☆☆", "△■☆☆☆☆☆", "△△■☆☆☆☆☆", "■■☆☆☆☆☆", "△■■☆☆☆☆☆", "△△■■☆☆☆☆☆", 
                                "超出" 
                            };
        private Color[] allColor = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Purple, 
                                     Color.Black, Color.Gray, Color.Maroon, Color.Chocolate, Color.Wheat };

        private bool isGenerate = false;

        //新分支，改用array保存总库
        private int totalData = 0;
        private int[] FilterStatistics;
        private int[] SpecialMark;      //特殊五角星标记
        private int[] DiffColorNum;
        private List<Color[]> ColorValue = new List<Color[]>();
        List<int[]> l_totalDataBase = new List<int[]>();

        Logging savelog = new Logging();

        #endregion

        public Form1()
        {
            InitializeComponent();
            comboBox5.SelectedIndex = 3;
            comboBox2.SelectedIndex = 0;
            comboBox6.SelectedIndex = 2;
            comboBox3.SelectedIndex = 2;
            comboBox4.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox_Mehod2_marked.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            checkedListBox1.SetItemChecked(0, true);
            for (int i = 1; i <= 10; i++)
            {
                string controlName = "checkedListBox" + i.ToString();
                CheckedListBox clb = (CheckedListBox)findControl(panel1, controlName);
                clb.SetItemChecked(0, true);
                clb.SetItemChecked(1, true);
                if(i<7)
                    clb.SetItemChecked(2, true);
            }
            InitDataGridView1();
            InitDataGridView2();

            GetFileList(false); //读取已存记录
            for (int i = 0; i < TotalNum;i++ )
            {
                choiceDate.Add(i+1);
            }
        }
        /* 特殊五角星 @param arr tvalue  @param ctab ctab */
        private void SpecialStar(int local, int ctab)
        {
            SpecialMark[local] += ctab;
        }
        
        private void InitDataGridView1()
        {
            //    ☆ □ △ ■
            DataGridViewRowCollection rows = this.dataGridView1.Rows;
            for (int i = 0; i < 55; i++ )
            {
                rows.Add( i + 1, sort[i], 0, " 查看" );
            }
        }
        private void InitDataGridView2()
        {
            string[] sort = {   "0",
                                "☆", 
                                "☆☆", 
                                "☆☆☆", 
                                "☆☆☆☆", 
                                "☆☆☆☆☆", 
                                "超出" 
                            };
            DataGridViewRowCollection rows = this.dataGridView3.Rows;
            for (int i = 0; i < 7; i++)
            {
                rows.Add(i + 1, sort[i], 0, " 查看");
            }
        }

        public void ErrorBox()
        {
            MessageBoxButtons errButton = MessageBoxButtons.OK;
            MessageBox.Show("程序出现了一个未知的错误，错误已记录到日志，请联系管理员。", "错误", errButton);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "0")
                {
                    richTextBox1.Text = "";
                    return;
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Length > 4)
                {
                    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("数据量比较大，查看结果可能导致程序卡死，确定要查看吗?", "警告", messButton);
                    if (dr == DialogResult.Cancel)//如果点击“确定”按钮
                    {
                        return;
                    }
                }

                richTextBox1.Text = "";
                int tab = e.RowIndex;
                CurrentMarkforPrint = sort[tab];
                string text = "";
                for (int ia = 0; ia < totalData; ia++)
                {
                    if (tab == 54 && FilterStatistics[ia] >= tab)
                    {
                        for (int i = 0; i < SelectNum; i++)
                        {
                            if (l_totalDataBase[ia][i] < 10)
                                text += " ";
                            text += l_totalDataBase[ia][i] + " ";
                        }
                            text += "\n";
                    }
                    else if (FilterStatistics[ia] == tab)
                    {
                        for (int i = 0; i < SelectNum; i++)
                        {
                            if (l_totalDataBase[ia][i] < 10)
                                text += " ";
                            text += l_totalDataBase[ia][i] + " ";
                        }
                            text += "\n";
                    }
                }
                richTextBox1.Text = text;
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "0")
                {
                    richTextBox2.Text = "";
                    return;
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Length > 4)
                {
                    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("数据量比较大，查看结果可能导致程序卡死，确定要查看吗?", "警告", messButton);
                    if (dr == DialogResult.Cancel)//如果点击“确定”按钮
                    {
                        return;
                    }
                }
                richTextBox2.Text = "";
                int tab = e.RowIndex;
                CurrentMarkforPrint = sort[tab];
                string text = "";
                for (int ia = 0; ia < totalData; ia++ )
                {
                    if (tab == 6 && SpecialMark[ia] >= tab)
                    {
                        for (int i = 0; i < SelectNum; i++)
                        {
                            if (l_totalDataBase[ia][i] < 10)
                                text += " ";
                            text += l_totalDataBase[ia][i] + " ";
                        }
                        text += "\n";
                    }
                    else if (SpecialMark[ia] == tab)
                    {
                        for (int i = 0; i < SelectNum; i++)
                        {
                            if (l_totalDataBase[ia][i] < 10)
                                text += " ";
                            text += l_totalDataBase[ia][i] + " ";
                        }
                        text += "\n";
                    }
                }
                richTextBox2.Text = text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /* 检查checkedListBox勾选 */
        private List<int> checkchoice_checkedListBox()
        {
            TotalNum = 0;//清空统计
            List<int> choiceDate = new List<int>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox1.GetItemText(checkedListBox1.Items[i]))); };
                if (checkedListBox2.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox2.GetItemText(checkedListBox2.Items[i]))); };
                if (checkedListBox3.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox3.GetItemText(checkedListBox3.Items[i]))); };
                if (checkedListBox4.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox4.GetItemText(checkedListBox4.Items[i]))); };
                if (checkedListBox5.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox5.GetItemText(checkedListBox5.Items[i]))); };
                if (checkedListBox6.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox6.GetItemText(checkedListBox6.Items[i]))); };
                if (checkedListBox7.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox7.GetItemText(checkedListBox7.Items[i]))); };
                if (checkedListBox8.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox8.GetItemText(checkedListBox8.Items[i]))); };
                if (checkedListBox9.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox9.GetItemText(checkedListBox9.Items[i]))); };
                if (checkedListBox10.GetItemChecked(i)) { choiceDate.Add(int.Parse(checkedListBox10.GetItemText(checkedListBox10.Items[i]))); };
            }
            TotalNum = choiceDate.Count;
            label6.Text = TotalNum.ToString();
            return choiceDate;
        }

        /* 查找控件方法 */
        private Control findControl(Control control, string controlName)
        {
            Control c1;
            foreach (Control c in control.Controls)
            {
                if (c.Name == controlName)
                {
                    return c;
                }
                else if (c.Controls.Count > 0)
                {
                    c1 = findControl(c, controlName);
                    if (c1 != null)
                    {
                        return c1;
                    }
                }
            }
            return null;
        } 

        /* 为点击的按钮着色 */
        private void digit_Button_Click(Button button)
        {
            if (button.BackColor == ChoiceColor)
                button.BackColor = Color.Transparent;
            else
                button.BackColor = ChoiceColor;
            //白色字体
            if (button.BackColor == Color.Green || button.BackColor == Color.Blue 
                || button.BackColor == Color.Indigo || button.BackColor == Color.Purple
                || button.BackColor == Color.Black || button.BackColor == Color.Maroon)
                button.ForeColor = Color.White;
            else
                button.ForeColor = Color.Black;

            if (!D_ColorTemptDict.ContainsKey(int.Parse(button.Text)))
                D_ColorTemptDict.Add(int.Parse(button.Text), button.BackColor);
            else if (button.BackColor == Color.Transparent)
                D_ColorTemptDict.Remove(int.Parse(button.Text));
            else
                D_ColorTemptDict[int.Parse(button.Text)] = button.BackColor;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                for (int j = 0; j < checkedListBox5.Items.Count; j++)
                    checkedListBox5.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox5.Items.Count; j++)
                    checkedListBox5.SetItemChecked(j, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "点击左边“查看”按钮显示结果";
            for (int i = 0; i < 55;i++ )
                dataGridView1.Rows[i].Cells[2].Value = 0;   //  全部先设成0
            for (int ia = 0; ia < totalData; ia++)
            {
                if (FilterStatistics[ia] >= 54)
                    dataGridView1.Rows[54].Cells[2].Value = (int)dataGridView1.Rows[54].Cells[2].Value + 1;
                else
                {
                    dataGridView1.Rows[FilterStatistics[ia]].Cells[2].Value = (int)dataGridView1.Rows[FilterStatistics[ia]].Cells[2].Value + 1;
                }
            }

            /* 特殊五角星tab */
            richTextBox2.Text = "点击左边“查看”按钮显示结果";
            for (int i = 0; i < 7; i++)
                dataGridView3.Rows[i].Cells[2].Value = 0;
            for(int i=0; i<totalData; i++)
            {
                if (SpecialMark[i] >= 6)
                    dataGridView3.Rows[6].Cells[2].Value = (int)dataGridView3.Rows[6].Cells[2].Value + 1;
                else
                    dataGridView3.Rows[SpecialMark[i]].Cells[2].Value = (int)dataGridView3.Rows[SpecialMark[i]].Cells[2].Value + 1;
            }

            string buttonName = ((Button)sender).Name;
            if(buttonName.IndexOf("3")>0)       // 查看特殊按钮的名字是：button3
                tabControl1.SelectedIndex = 4;  // 跳到特殊五星tab
            else
                tabControl1.SelectedIndex = 3;  // 调到普通结果tab
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, true);
            }
            else
            {
                for (int j =0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, false);
            }
        }

        private void GetWinningNumbers()
        {
            /* 获取输入的中奖号码 */
            Winning_Numbers = new int[SelectNum];
            for (int i = 0; i < SelectNum; i++)
            {
                string controlName = "textBox_win" + (i + 1).ToString();
                TextBox tb = (TextBox)findControl(panel5, controlName);
                if (tb.Text == "" || tb.Text == "0" || (i > 0 && int.Parse(tb.Text) < Winning_Numbers[i-1]))
                {
                    MessageBox.Show("请输入正常的目标号码", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Winning_Numbers = null;
                    return;
                }
                Winning_Numbers[i] = int.Parse(tb.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* 清空各种数据 */
            D_ColorTemptDict.Clear();                           //清空颜色记录
            l_totalDataBase.Clear();                            //总库清空
            ColorValue.Clear();
            /* 获取选择数据 */
            SelectNum = int.Parse(comboBox5.Text);              //M取N，获取N
            choiceDate = checkchoice_checkedListBox();          //获取勾选的数字，赋值TotalNum
            /* 生成数据 */
            GenerateData gData = new GenerateData();
            gData.s = SelectNum;                                //s为要取的个数  n
            gData.e = TotalNum;                                 //e为要取得基数的总数   m
            gData.dataSets = choiceDate;                        //选择的数据集
            //生成数据
            l_totalDataBase = gData.run2();                     // 生成数据总库
            ColorValue = gData.runColor();                      // 生成颜色数据总库
            totalData = gData.CountTotalData(TotalNum, SelectNum);  //总库的数据总量
            InitFilterStatisticsAndDiffColorNum(totalData);     //初始化总库 筛选统计数组、不同颜色统计数组

            /* 跳到下一个tab:逻辑查询操作tab */
            FinishingInterface();                               // 整理还原tab3的界面
            tabControl1.SelectedIndex = 2;                      //跳到编号为3的tab
            isGenerate = true;                                  //是否已生成数据=true，避免保存出错
        }

        /* 分支2，初始化筛选统计数组、不同颜色统计数组 */
        private void InitFilterStatisticsAndDiffColorNum(int totalData)
        {
            FilterStatistics = new int[totalData];
            DiffColorNum = new int[totalData];
            SpecialMark = new int[totalData]; 
            foreach(int i in FilterStatistics)
            {
                FilterStatistics[i] = 0;
                SpecialMark[i] = 0;
                DiffColorNum[i] = 0;
            }
        }

        /* 整理还原tab3的界面 */
        private void FinishingInterface()
        {
            /* 清空各种数据 */
            m_Count_Operate = 0;                        //操作次数清零
            //m_SpecialStar.Clear();                      //特殊五星库清空
            count_generate.Text = "";                   //左下角提示清空
            count_operate.Text = "";                    //左下角提示清空
            comboBox_method2_operate.SelectedIndex = SelectNum - 1;
            //激活被选中数字的按钮
            for (int i = 1; i <= 70; i++)
            {
                string controlName = "button_c" + i.ToString();
                Button bb = (Button)findControl(panel2, controlName);
                bb.Enabled = false;
                bb.BackColor = Color.White;
                bb.ForeColor = Color.Black;
            }
            for (int i = 0; i < TotalNum; i++)
            {
                string controlName = "button_c" + choiceDate[i].ToString();
                Button bb = (Button)findControl(panel2, controlName);
                bb.Enabled = true;
                bb.BackColor = Color.Transparent;
            }
            //屏蔽过量的五星定位输入框
            for (int i = 3; i < 8; i++)
            {
                string textBoxName = "textBox" + (i + 4).ToString();
                TextBox tb = (TextBox)findControl(panel2, textBoxName);
                if (i >= SelectNum)
                    tb.Enabled = false;
                else
                    tb.Enabled = true;
            }

            //还原查看结果页面
            richTextBox1.Text = "点击左边“查看”按钮显示结果";
            for (int i = 0; i < 55; i++)
                dataGridView1.Rows[i].Cells[2].Value = 0;   //  全部先设成0
            richTextBox2.Text = "点击左边“查看”按钮显示结果";
            for (int i = 0; i < 7; i++)
                dataGridView3.Rows[i].Cells[2].Value = 0;

            /* 屏蔽过量的中奖号码输入框 */
            for (int i = 1; i < 9; i++)
            {
                string WinControlName = "textBox_win" + i.ToString();
                TextBox bw = (TextBox)findControl(panel5, WinControlName);
                if (SelectNum < i)
                {
                    bw.Enabled = false;
                }
                else
                {
                    bw.Enabled = true;
                }
            }

            //上方提示几选几
            label3.Text = TotalNum + "取" + SelectNum;
            label2.Text = "0";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int j = 0; j < checkedListBox2.Items.Count; j++)
                    checkedListBox2.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox2.Items.Count; j++)
                    checkedListBox2.SetItemChecked(j, false);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                for (int j = 0; j < checkedListBox3.Items.Count; j++)
                    checkedListBox3.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox3.Items.Count; j++)
                    checkedListBox3.SetItemChecked(j, false);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                for (int j = 0; j < checkedListBox4.Items.Count; j++)
                    checkedListBox4.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox4.Items.Count; j++)
                    checkedListBox4.SetItemChecked(j, false);
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                for (int j = 0; j < checkedListBox6.Items.Count; j++)
                    checkedListBox6.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox6.Items.Count; j++)
                    checkedListBox6.SetItemChecked(j, false);
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                for (int j = 0; j < checkedListBox7.Items.Count; j++)
                    checkedListBox7.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox7.Items.Count; j++)
                    checkedListBox7.SetItemChecked(j, false);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                for (int j = 0; j < checkedListBox8.Items.Count; j++)
                    checkedListBox8.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox8.Items.Count; j++)
                    checkedListBox8.SetItemChecked(j, false);
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                for (int j = 0; j < checkedListBox9.Items.Count; j++)
                    checkedListBox9.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox9.Items.Count; j++)
                    checkedListBox9.SetItemChecked(j, false);
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_0.Checked)
            {
                for (int j = 0; j < checkedListBox10.Items.Count; j++)
                    checkedListBox10.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox10.Items.Count; j++)
                    checkedListBox10.SetItemChecked(j, false);
            }
        }

        private void checkedListBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox11.Items.Count; j++)
            {
                if (checkedListBox11.GetItemChecked(j))
                {
                    checkedListBox1.SetItemChecked(j, true);
                    checkedListBox2.SetItemChecked(j, true);
                    checkedListBox3.SetItemChecked(j, true);
                    checkedListBox4.SetItemChecked(j, true);
                    checkedListBox5.SetItemChecked(j, true);
                    checkedListBox6.SetItemChecked(j, true);
                    checkedListBox7.SetItemChecked(j, true);
                    checkedListBox8.SetItemChecked(j, true);
                    checkedListBox9.SetItemChecked(j, true);
                    checkedListBox10.SetItemChecked(j, true);
                }
                else
                {
                    checkedListBox1.SetItemChecked(j, false);
                    checkedListBox2.SetItemChecked(j, false);
                    checkedListBox3.SetItemChecked(j, false);
                    checkedListBox4.SetItemChecked(j, false);
                    checkedListBox5.SetItemChecked(j, false);
                    checkedListBox6.SetItemChecked(j, false);
                    checkedListBox7.SetItemChecked(j, false);
                    checkedListBox8.SetItemChecked(j, false);
                    checkedListBox9.SetItemChecked(j, false);
                    checkedListBox10.SetItemChecked(j, false);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            int num = int.Parse(textBox2.Text);
            label6.Text = textBox2.Text;
            bool checkState = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox2.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox3.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox4.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox5.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox6.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox7.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox8.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox9.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
                checkedListBox10.SetItemChecked(i, checkState); if (--num == 0) checkState = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox11.Items.Count; j++)
            {
                checkedListBox1.SetItemChecked(j, false);
                checkedListBox2.SetItemChecked(j, false);
                checkedListBox3.SetItemChecked(j, false);
                checkedListBox4.SetItemChecked(j, false);
                checkedListBox5.SetItemChecked(j, false);
                checkedListBox6.SetItemChecked(j, false);
                checkedListBox7.SetItemChecked(j, false);
                checkedListBox8.SetItemChecked(j, false);
                checkedListBox9.SetItemChecked(j, false);
                checkedListBox10.SetItemChecked(j, false);
                checkedListBox11.SetItemChecked(j, false);
            }
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox10.Checked = false;
            checkBox_0.Checked = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Red;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Orange;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Yellow;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Green;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Blue;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Indigo;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Purple;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button_c1.BackColor = ChoiceColor;
        }
        /* 1-70个按钮的通用事件 */
        private void button_cx_Click(object sender, EventArgs e)
        {
            digit_Button_Click((Button)sender);
        }
        /* 标记按钮1 */
        private void button76_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            if (buttonName.IndexOf("minus") > 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您当前要进行的是减操作，确定进行吗?", "减操作", messButton);
                if (dr == DialogResult.Cancel)//如果点击“确定”按钮
                {
                    return;
                }
            }
            
            if (comboBox7.SelectedIndex == 0)
            {
                m_Count_Generate = Filter1(sender, ChoiceColor);
                count_generate.Text = "本次操作共标记" + m_Count_Generate + "注";
                m_Count_Generate = 0;
            }
            else 
            { 
                foreach(Color col in allColor)
                {
                    m_Count_Generate = Filter1(sender, col);
                }
                count_generate.Text = "本次操作共标记" + m_Count_Generate + "注";
                m_Count_Generate = 0;
            }
            
        }
        /* 筛选方法1 */
        private int Filter1(object sender, Color col)
        {
            string buttonName = ((Button)sender).Name;
            //get the pomenont param
            string logicalOperation = comboBox2.Text;
            int count = int.Parse(comboBox3.Text);
            //this is get the customer mark,we should defind it.tempt method
            int cTab;

            switch (comboBox4.SelectedIndex)
            {
                case 0://三角形
                    cTab = 1;
                    break;
                case 1://四边形
                    cTab = 3;
                    break;
                case 2://五边形
                    cTab = 9;
                    break;
                case 3://超出
                    cTab = 54;
                    break;
                case 4: //特殊五星
                    cTab = 1;
                    break;
                default:
                    cTab = 0;
                    break;
            }
            
            //接下来要把标记后totalDataBase.tab的增量算出来并存起来。
            if (cTab != 0)
            {
                if (buttonName.IndexOf("minus") > 0)    //  减操作，ctab取反
                    cTab = -cTab;
                for (int i = 0; i < totalData; i++)
                {
                    int temptCount = 0;
                    for (int i2 = 0; i2 < SelectNum; i2++)
                    {
                        ColorValue[i][i2] = Color.White;   //  先设成白色？好像不需要
                        if (D_ColorTemptDict.ContainsKey(l_totalDataBase[i][i2]) && D_ColorTemptDict[l_totalDataBase[i][i2]] != Color.White)//如果这个数字有颜色
                        {
                            ColorValue[i][i2] = D_ColorTemptDict[l_totalDataBase[i][i2]];//颜色存到总库
                        }
                        //if the kvp color equal the marked color,temptCount +1
                        if (ColorValue[i][i2] == col)
                        {
                            temptCount++;
                        }
                    }
                    //judge the tmpet whether reach the requirement,then deal the datevalue(tag)
                    switch (logicalOperation)
                    {
                        case "<=":
                            if (temptCount <= count && count < 10/*|| (count > 10 && (temptCount == count / 10 || temptCount == count % 10))*/)
                            {
                                if (FilterStatistics[i] + cTab < 0)//0,不让减了
                                    continue;
                                FilterStatistics[i] += cTab;
                                m_Count_Generate++;
                            }
                            break;
                        case "=":
                            if (temptCount == count || (count > 10 && (temptCount == count / 10 || temptCount == count % 10)))
                            {
                                if (comboBox4.SelectedIndex != 4)   //不是特殊五角星
                                {
                                    if (FilterStatistics[i] + cTab < 0)//0,不让减了
                                        continue;
                                    FilterStatistics[i] += cTab;
                                }
                                else            //特殊五角星
                                {
                                    SpecialStar(i, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        case ">=":
                            if (temptCount >= count/* || (count > 10 && (temptCount == count / 10 || temptCount == count % 10))*/)
                            {
                                if (FilterStatistics[i] + cTab < 0)//0,不让减了
                                    continue;
                                FilterStatistics[i] += cTab;
                                m_Count_Generate++;
                            }
                            break;
                        case "!=":
                            if (temptCount != count || (count > 10 && (temptCount == count / 10 || temptCount == count % 10)))
                            {
                                if (comboBox4.SelectedIndex != 4)
                                {
                                    if (FilterStatistics[i] + cTab < 0)//0,不让减了
                                        continue;
                                    FilterStatistics[i] += cTab;
                                }
                                else
                                {
                                    SpecialStar(i, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else //cTab == 0
            {
                m_Count_Generate = 0;
            }
            m_Count_Operate++;
            count_operate.Text = "您已经进行了" + m_Count_Operate + "次操作";
            return m_Count_Generate;
        }

        /* 标记按钮2 ------ 五色齐 */
        private void button_method2mark_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            if (buttonName.IndexOf("minus") > 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您当前要进行的是减操作，确定进行吗?", "减操作", messButton);
                if (dr == DialogResult.Cancel)//如果点击“确定”按钮
                {
                    return;
                }
            }
            m_Count_Generate = Filter2(sender);
            count_generate.Text = "本次操作共标记" + m_Count_Generate + "注";
            m_Count_Generate = 0;
        }

        private int Filter2(object sender)
        {
            string buttonName = ((Button)sender).Name;
            //database color mark
            bool newColor = true;
            bool zeroColor = false; // 标记是否有空颜色，有即不属于任何n色齐
            for (int ia = 0; ia < totalData; ia++)
            {
                DiffColorNum[ia] = 0;
                zeroColor = false;
                for (int i2 = 0; i2 < SelectNum; i2++)
                {
                    ColorValue[ia][i2] = Color.White;   //  先设成白色？好像不需要
                    if (D_ColorTemptDict.ContainsKey(l_totalDataBase[ia][i2]))//如果这个数字有颜色
                    {
                        ColorValue[ia][i2] = D_ColorTemptDict[l_totalDataBase[ia][i2]];//颜色存到总库
                        for (int j = 0; j < i2; j++)
                        {
                            if (ColorValue[ia][i2] == ColorValue[ia][j])//如果跟前面的颜色相同，即重复了，标记为不是新颜色
                                newColor = false;//
                        }
                        if (newColor)
                        {
                            DiffColorNum[ia]++;
                        }
                        newColor = true;
                    }
                    else
                    {
                        zeroColor = true;
                    }
                }
                if (zeroColor)
                {
                    DiffColorNum[ia] = 0;
                }
            }
            
            //get the pomenomt param
            int operation = comboBox_method2_operate.SelectedIndex;
            int marked = comboBox_Mehod2_marked.SelectedIndex;
            int diffColorNum;
            switch (operation)
            {
                case 0:
                    diffColorNum = 1;
                    break;
                case 1:
                    diffColorNum = 2;
                    break;
                case 2:
                    diffColorNum = 3;
                    break;
                case 3:
                    diffColorNum = 4;
                    break;
                case 4:
                    diffColorNum = 5;
                    break;
                case 5:
                    diffColorNum = 6;
                    break;
                case 6:
                    diffColorNum = 7;
                    break;
                case 7:
                    diffColorNum = 8;
                    break;
                case 8://一色五色全
                    diffColorNum = 15;
                    break;
                case 9://二色五色全
                    diffColorNum = 25;
                    break;
                case 10://二色六色全
                    diffColorNum = 26;
                    break;
                case 11://三色六色全
                    diffColorNum = 36;
                    break;
                case 12://二色七色全
                    diffColorNum = 27;
                    break;
                case 13://三色七色全
                    diffColorNum = 37;
                    break;
                default:
                    diffColorNum = 0;
                    break;
            }
            int cTab;
            switch (marked)
            {
                case 0://三角形
                    cTab = 1;
                    break;
                case 1://四边形
                    cTab = 3;
                    break;
                case 2://五边形
                    cTab = 9;
                    break;
                case 3:
                    cTab = 54;
                    break;
                case 4://特殊五角星
                    cTab = 1;
                    break;
                default:
                    cTab = 0;
                    break;
            }
            if (cTab != 0)
            {
                if (buttonName.IndexOf("minus") > 0)
                    cTab = -cTab;
                for (int ia = 0; ia < totalData; ia++)
                {
                    switch(comboBox6.Text)
                    {
                        case "=":
                            if (DiffColorNum[ia] == diffColorNum || (diffColorNum > 10 && (DiffColorNum[ia] == diffColorNum / 10 || DiffColorNum[ia] == diffColorNum % 10)))
                            {
                                if (marked != 4)
                                {
                                    if (cTab < 0 && DiffColorNum[ia] == 0)
                                        continue;
                                    FilterStatistics[ia] += cTab;
                                }
                                else //特殊五角星
                                {
                                    SpecialStar(ia, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        case "!=":
                            if (DiffColorNum[ia] != diffColorNum || (diffColorNum > 10 && (DiffColorNum[ia] == diffColorNum / 10 || DiffColorNum[ia] == diffColorNum % 10)))
                            {
                                if (marked != 4)
                                {
                                    if (cTab < 0 && DiffColorNum[ia] == 0)
                                        continue;
                                    FilterStatistics[ia] += cTab;
                                }
                                else //特殊五角星
                                {
                                    SpecialStar(ia, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        case ">=":
                            if (DiffColorNum[ia] >= diffColorNum /*|| (diffColorNum > 10 && (DiffColorNum[ia] == diffColorNum / 10 || DiffColorNum[ia] == diffColorNum % 10))*/)
                            {
                                if (marked != 4)
                                {
                                    if (cTab < 0 && FilterStatistics[ia] == 0)
                                        continue;
                                    FilterStatistics[ia] += cTab;
                                }
                                else //特殊五角星
                                {
                                    SpecialStar(ia, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        case "<=":
                            if (DiffColorNum[ia] <= diffColorNum /*|| (diffColorNum > 10 && (DiffColorNum[ia] == diffColorNum / 10 || DiffColorNum[ia] == diffColorNum % 10))*/)
                            {
                                if (marked != 4)
                                {
                                    if (cTab < 0 && FilterStatistics[ia] == 0)
                                        continue;
                                    FilterStatistics[ia] += cTab;
                                }
                                else //特殊五角星
                                {
                                    SpecialStar(ia, cTab);
                                }
                                m_Count_Generate++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                m_Count_Generate = 0;
            }
            //count the use operate data
            m_Count_Operate++;
            count_operate.Text = "您已经进行了" + m_Count_Operate + "次操作";
            return m_Count_Generate;
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox1.Text = "";
            LinkLabel p = sender as LinkLabel;          //获取控件的名称p.Name
            int tab = 0;
            switch (int.Parse(p.Name.Substring(p.Name.LastIndexOf("l") + 1)))
            {
                case 0:
                    tab = 0;
                    break;
                case 1:
                    tab = 1;
                    break;
                case 2:
                    tab = 2;
                    break;
                case 3:
                    tab = 3;
                    break;
                case 4:
                    tab = 6;
                    break;
                case 5:
                    tab = 9;
                    break;
                default:
                    break;
            }
            string text = "";
            for (int ia = 0; ia < totalData; ia++)
            {
                if (FilterStatistics[ia] == tab)
                {
                    for (int i = 0; i < SelectNum; i++)
                    {
                        if (l_totalDataBase[ia][i] < 10)
                            text += " ";
                        text += l_totalDataBase[ia][i] + " ";
                    }
                   text += "\n";
                }
            }
            richTextBox1.Text = text;
        }

        /* 标记按钮3 ------ 五星定位 */
        private void button4_Click_1(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            if (buttonName.IndexOf("minus") > 0)
            { 
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您当前要进行的是减操作，确定进行吗?", "减操作", messButton);
                if (dr == DialogResult.Cancel)//如果点击“确定”按钮
                {
                    return;
                }
            }
            string[] text = new string[8];
            text[0] = textBox4.Text;
            text[1] = textBox5.Text;
            text[2] = textBox6.Text;
            text[3] = textBox7.Text;
            text[4] = textBox8.Text;
            text[5] = textBox9.Text;
            text[6] = textBox10.Text;
            text[7] = textBox11.Text;
            bool isAllX = true;
            for(int i=0;i<8;i++)
            {
                if (text[i] != "x" && text[i] != "")
                {
                    isAllX = false;
                }
            }
            if (isAllX)
                return;
            int cTab;
            switch (comboBox1.SelectedIndex)
            {
                case 0://三角形
                    cTab = 1;
                    break;
                case 1://四边形
                    cTab = 3;
                    break;
                case 2://五边形
                    cTab = 9;
                    break;
                case 3:
                    cTab = 54;
                    break;
                case 4:
                    cTab = 1;
                    break;
                default:
                    cTab = 0;
                    break;
            }
            bool isTrue = false;
            if (cTab != 0)
            {
                if (buttonName.IndexOf("minus") > 0)
                    cTab = -cTab;
                for (int ia = 0; ia < totalData; ia++ )
                {

                    for (int i = 0; i < SelectNum; i++)
                    {
                        if (text[i] == "x" || text[i] == "")
                        {
                            isTrue = true;
                            continue;
                        }
                        else if (l_totalDataBase[ia][i] != int.Parse(text[i]))
                        {
                            isTrue = false;
                            break;
                        }
                        else
                            isTrue = true;
                    }
                    if (isTrue)
                    {
                        if (comboBox1.SelectedIndex != 4)
                        {
                            if (cTab < 0 && FilterStatistics[ia] == 0)
                                continue;
                            FilterStatistics[ia] += cTab;
                        }
                        else    //特殊五角星
                        {
                            SpecialStar(ia, cTab);
                        }
                        m_Count_Generate++;
                    }
                }
            }
            else
            {
                m_Count_Generate = 0;
            }
            //count the use operate data
            count_generate.Text = "本次操作共标记" + m_Count_Generate + "注";
            m_Count_Operate++;
            count_operate.Text = "您已经进行了" + m_Count_Operate + "次操作";
            m_Count_Generate = 0;
        }

        /* 清空颜色 */
        private void button7_Click(object sender, EventArgs e)
        {
            choiceDate = checkchoice_checkedListBox();          //获取勾选的数字
            for (int i = 0; i < TotalNum; i++ )
            {
                string controlName = "button_c" + choiceDate[i].ToString();
                Button bb = (Button)findControl(panel2, controlName);
                bb.BackColor = Color.Transparent;
                bb.ForeColor = Color.Black;
            }
            D_ColorTemptDict.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Gray;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Black;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "20" || comboBox3.Text == "30" || comboBox3.Text == "40" || 
                comboBox3.Text == "50" || comboBox3.Text == "60" || comboBox3.Text == "70")
                if (comboBox2.SelectedIndex != 2 && comboBox2.SelectedIndex !=3)    //未勾选等于or不等于
                    comboBox2.SelectedIndex = 2;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            markRadioChecked(0);
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            markRadioChecked(1);
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            markRadioChecked(2);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            markRadioChecked(3);
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            markRadioChecked(4);
        }

        private void markRadioChecked(int i)
        {
            comboBox4.SelectedIndex = i;
            comboBox_Mehod2_marked.SelectedIndex = i;
            comboBox1.SelectedIndex = i;
            comboBox8.SelectedIndex = i;
        }

        //只能输入数字跟删除
        private void numTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)(e.KeyChar) == Keys.Back || (Keys)(e.KeyChar) == Keys.Delete)
            {
                return;
            }
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
        }

        private void 保存当前进度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!isGenerate) //如果没生成数据，直接返回，避免出bug
            {
                MessageBox.Show("保存失败，请先生成数据。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now; ;
            string date = currentTime.ToString();
            if(date.Length == 17)   // 小时只有一位的时候，补个0给他
            {
                date = date.Replace(" ", " 0");
            }
            string RecordFile = path + ("record." + date + "." + TotalNum.ToString() + "q" + SelectNum.ToString() + ".xml").Replace(":", "-").Replace("/", "-");

            string choiceDate_xml = "";
            foreach(int i in choiceDate)
            {
                choiceDate_xml += i.ToString() + ",";
            }

            XmlElement list = null, arrays = null, array = null, root = null;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);
                root = xmldoc.CreateElement("root");
                xmldoc.AppendChild(root);                           //添加根节点

                list = xmldoc.CreateElement("list");
                list.InnerText = (choiceDate_xml);                  //设置内容
                list.SetAttribute("m", TotalNum.ToString());        //设置属性
                list.SetAttribute("n", SelectNum.ToString());
                xmldoc.DocumentElement.AppendChild(list);           //添加到节点

                xmldoc.Save(RecordFile);

                arrays = xmldoc.CreateElement("arrays");
                xmldoc.DocumentElement.AppendChild(arrays);

                //保存l_totalDataBase
                for (int ia = 0; ia < totalData; ia++)
                {
                    string tvalue_xml = "";
                    array = xmldoc.CreateElement("array");
                    for (int i = 0; i < SelectNum; i++)
                    {
                        tvalue_xml += l_totalDataBase[ia][i].ToString() + ",";
                    }
                    tvalue_xml += FilterStatistics[ia].ToString() + ",";    //  普通标记
                    tvalue_xml += SpecialMark[ia].ToString() + "";          //  特殊标记
                    array.InnerText = (tvalue_xml);
                    //array.SetAttribute("ctab", FilterStatistics[ia].ToString());
                    //array.SetAttribute("stab", SpecialMark[ia].ToString());    //特殊五角星标记
                    arrays.AppendChild(array);
                }
                xmldoc.Save(RecordFile);
                GC.Collect();
            }
            catch (Exception ex)
            {
                savelog.errorlog(path, ex.ToString());
                ErrorBox();
            }
            MessageBox.Show("保存成功。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GetFileList(false);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int PrintColumn = 1;
            bool isEnd = false;
            Font font_title = new Font(new FontFamily("黑体"), 18);
            Font font = new Font("宋体", 14);
            Brush bru = Brushes.Blue;
            string[] line = richTextBox1.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
            string msg = CurrentMarkforPrint + " = " + (line.Length-1) + "\n\n";
            for (int i = CurrentPage * 135; i < (CurrentPage+1) * 135; i++)             //135 = 45*3
            {
                if (i >= line.Length)
                {
                    isEnd = true;
                    break;
                }
                if (PrintColumn < 3)        //  3列打印
                {
                    msg += line[i] + "   ";
                    PrintColumn++;
                }
                else
                {
                    msg += line[i] + "\n";
                    PrintColumn = 1;
                }
                if (i == line.Length - 1)
                {
                    isEnd = true;
                }
            }
            printDoc1(e, msg, font, bru);
            e.HasMorePages = !isEnd; //是否有下一页
        }
        //just print
        private void printDoc1(System.Drawing.Printing.PrintPageEventArgs e, string msg, Font font, Brush bru)
        {
            e.Graphics.DrawString(msg, font, bru, 50, 50);
            CurrentPage ++;
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.printDialog1.Document = printDocument1;
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void button_btnPageSetup_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.pageSetupDialog1.Document = printDocument1;
            this.pageSetupDialog1.ShowDialog(); 
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.printPreviewDialog1.Document = printDocument1;
            this.printPreviewDialog1.ShowDialog();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 4 && comboBox2.SelectedIndex != 2 && comboBox2.SelectedIndex != 3)
            {
                comboBox2.SelectedIndex = 2;
            }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Maroon;
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Chocolate;
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceColor = Color.Wheat;
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            comboBox5.Text = "2";
            label6.Text = "12";
            textBox2.Text = "12";
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            comboBox5.Text = "5";
            label6.Text = "26";
            textBox2.Text = "26";
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            comboBox5.Text = "7";
            label6.Text = "36";
            textBox2.Text = "36";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.pageSetupDialog2.Document = printDocument2;
            this.pageSetupDialog2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.printPreviewDialog2.Document = printDocument2;
            this.printPreviewDialog2.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.printDialog2.Document = printDocument2;
            if (this.printDialog2.ShowDialog() == DialogResult.OK)
            {
                this.printDocument2.Print();
            }
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            {
                int PrintColumn = 1;
                bool isEnd = false;
                Font font_title = new Font(new FontFamily("黑体"), 18);
                Font font = new Font("宋体", 14);
                Brush bru = Brushes.Blue;
                string[] line = richTextBox2.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
                string msg = CurrentMarkforPrint + " = " + (line.Length - 1) + "\n\n";
                for (int i = CurrentPage * 135; i < (CurrentPage + 1) * 135; i++)             //135 = 45*3
                {
                    if (i >= line.Length)
                    {
                        isEnd = true;
                        break;
                    }
                    if (PrintColumn < 3)        //  3列打印
                    {
                        msg += line[i] + "   ";
                        PrintColumn++;
                    }
                    else
                    {
                        msg += line[i] + "\n";
                        PrintColumn = 1;
                    }
                    if(i == line.Length - 1)       //到了最后一个，没有下一页
                    {
                        isEnd = true;
                    }
                }
                printDoc1(e, msg, font, bru);
                e.HasMorePages = !isEnd; //是否有下一页
            }
        }

        private void 小工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> array = new List<int>();
            array = checkchoice_checkedListBox();
            Form2 fm2 = new Form2(array);
            fm2.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            GetFileList(false);
        }

        private void GetFileList(bool IsAlert)
        {
            dataGridView2.Rows.Clear(); //  清除所有记录
            try
            {
                int row = 0;
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo d in dir.GetFiles())
                {
                    if(d.Name.IndexOf("xml") < 0)
                        continue;
                    string[] aa = d.Name.Split(new string[] { "." }, StringSplitOptions.None);
                    if (row >= dataGridView2.RowCount)
                        dataGridView2.Rows.Add();
                    dataGridView2.Rows[row].Cells[0].Value = row + 1;
                    dataGridView2.Rows[row].Cells[1].Value = aa[1];
                    dataGridView2.Rows[row].Cells[2].Value = aa[2];
                    dataGridView2.Rows[row].Cells[3].Value = " 删除";
                    dataGridView2.Rows[row].Cells[4].Value = " 加载";
                    row++;
                }
                row = 0;
            }
            catch (Exception ex)
            {
                savelog.errorlog(path, ex.ToString());
                ErrorBox();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4) //  加载
            {
                // selectNum,totalNum,choiceData,m_totalDataBase
                string pathName = path + "record." + dataGridView2.Rows[e.RowIndex].Cells[1].Value + "." + dataGridView2.Rows[e.RowIndex].Cells[2].Value + ".xml";
                LoadData ld = new LoadData();
                AllDataInOne adio = new AllDataInOne();
                adio = ld.Load(pathName);
                SelectNum = adio.n;
                TotalNum = adio.m;
                choiceDate = adio.choiceDate;
                l_totalDataBase = adio.l_totalDataBase;
                FilterStatistics = adio.FilterStatistics;
                SpecialMark = adio.SpecialMark;
                totalData = adio.totalData;
                //生成颜色空数据
                GenerateData gData = new GenerateData();
                gData.s = SelectNum;                                //s为要取的个数  n
                gData.e = TotalNum;                                 //e为要取得基数的总数   m
                ColorValue = gData.runColor();

                FinishingInterface();
                D_ColorTemptDict.Clear();                           //清空颜色记录
                tabControl1.SelectedIndex = 2;                      //跳到编号为3的tab
                isGenerate = true;
                MessageBox.Show("加载完成", "加载完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(e.ColumnIndex ==3)   //删除
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要删除记录吗?", "删除记录", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    string pathName = path + "record." + dataGridView2.Rows[e.RowIndex].Cells[1].Value + "." + dataGridView2.Rows[e.RowIndex].Cells[2].Value + ".xml";
                    try
                    {
                        DeleteFile df = new DeleteFile();
                        df.del(pathName);
                    }
                    catch (Exception ex)
                    {
                        savelog.errorlog(path, ex.ToString());
                        ErrorBox();
                    }
                    GetFileList(false);
                }
                else
                {
                    return;
                }
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要删除全部记录吗?", "删除记录", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DeleteFile df = new DeleteFile();
                foreach (FileInfo d in dir.GetFiles())
                {
                    if (d.Name.IndexOf("xml") < 0)
                        continue;
                    df.del(path + d.Name);
                }
                GetFileList(false);
            }
        }
        /* 目标号码定位 */
        private void button11_Click(object sender, EventArgs e)
        {
            GetWinningNumbers();        //获取中奖号码
            if (Winning_Numbers == null)
            {
                return;
            }
            Dichotomy dd = new Dichotomy();
            int local = dd.Find(Winning_Numbers, l_totalDataBase, totalData);
            int da54 = FilterStatistics[local];
            if (da54 > 54)
                da54 = 54;
            label2.Text = sort[da54];
        }

        private void comboBox_method2_operate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_method2_operate.SelectedIndex > 7)
            {
                if(comboBox6.SelectedIndex < 2 )
                {
                    comboBox6.SelectedIndex = 2;
                }
            }
        }

        private void button13_add_Click(object sender, EventArgs e)
        {

        }
    }
}
