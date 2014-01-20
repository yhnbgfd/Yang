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
    public partial class Form2 : Form
    {
        public Form2(List<int> list)
        {
            InitializeComponent();
            Init(list);
        }

        private void Init(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DataGridViewButtonColumn m_btncolumn = new DataGridViewButtonColumn();
                m_btncolumn.HeaderText = list[i].ToString();
                m_btncolumn.Width = 45;
                dataGridView1.Columns.Insert(i, m_btncolumn);
            }
            for (int i = 0; i < 2; i++)
                dataGridView1.Rows.Add();
            dataGridView1.RowHeadersWidth = 60;
            dataGridView1.Rows[0].HeaderCell.Value = "△";
            dataGridView1.Rows[1].HeaderCell.Value = "□";
            dataGridView1.Rows[2].HeaderCell.Value = "☆";
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                add_click(e.RowIndex, e.ColumnIndex);
            }
        }

        private void add_click(int RowIndex, int ColumnIndex)
        {
            DataGridViewCell cell = dataGridView1.Rows[RowIndex].Cells[ColumnIndex];
            cell.Value += dataGridView1.Rows[RowIndex].HeaderCell.Value.ToString();
            if (cell.Value.ToString().Length >= 3)
            {
                //Console.WriteLine(cell.Value.ToString());
                if (RowIndex >= 2)
                {
                    cell.Value = "☆☆☆";
                }
                else
                {
                    cell.Value = "";
                    add_click(RowIndex + 1, ColumnIndex);
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印内容 为 整个Form
            //Image myFormImage;
            //myFormImage = new Bitmap(this.Width, this.Height);
            //Graphics g = Graphics.FromImage(myFormImage);
            //g.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            //e.Graphics.DrawImage(myFormImage, 0, 0);

            //打印内容 为 局部的 this.groupBox1
            //Bitmap _NewBitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            //dataGridView1.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));
            //e.Graphics.DrawImage(_NewBitmap, 0, 0, _NewBitmap.Width, _NewBitmap.Height);

            //打印内容 为 自定义文本内容 
            Font font_title = new Font(new FontFamily("黑体"), 18);
            Font font = new Font("宋体", 14);
            Brush bru = Brushes.Blue;
            object value;
            {
                string msg;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                    {
                        if (i == 0)
                        {
                            e.Graphics.DrawString(dataGridView1.Columns[j].HeaderText
                                , font_title, Brushes.Black, new PointF(i * 100 + 100, j * 25 + 100));
                        }
                        value = dataGridView1.Rows[i].Cells[j].Value;
                        msg = value == null ? "" : value.ToString();
                        e.Graphics.DrawString(msg, font, bru, new PointF((i + 1) * 100 + 100, j * 25 + 100));
                        
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.ShowDialog(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.printPreviewDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
