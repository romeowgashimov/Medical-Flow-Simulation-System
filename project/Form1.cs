using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Manager mng;
        Director dir;

        private void Form1_Load(object sender, EventArgs e)
        {
            mng = new Manager(textBox1, textBox3);
            dir = new Director(textBox2);
            mng.AddQueue4(new NonPriorQueue(listBox5), dir);
            mng.AddQueue(new PersonQueue(0, listBox1), dir);
            mng.AddQueue(new PersonQueue(1, listBox2), dir);
            mng.AddQueue(new PersonQueue(2, listBox3), dir);
            mng.AddQueue(new PersonQueue(3, listBox4), dir);            
            mng.AddDir(dir);
            button1.Enabled = radioButton1.Checked;
            timer1.Enabled = radioButton2.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mng.OnTimer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1_Tick(this, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mng.SetStatistic();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = radioButton1.Checked;
            timer1.Enabled = radioButton2.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mng.OnClear();
        }
    }
}
