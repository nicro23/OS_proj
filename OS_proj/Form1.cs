using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_proj
{
    public partial class Form1: Form
    {
        List<process> pl;
        process p;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p = new process();
            p.x = 100;
            p.y = 100;
            p.show(7);
            /*button1.Text = "clicked";
            Label l;
            l = new Label();
            l.Text = "sajdlkas";
            l.Show();
            l.Width = 100;
            l.Height = 20;
            l.Location = new Point(100, 100);*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("FCFS");
            comboBox1.Items.Add("SJF");
            comboBox1.Items.Add("RR");
            comboBox1.Items.Add("Priority");
        }
    }
    public class process
    {
        TextBox BT, AT, P, WT, TA;
        Label name;
        public int x, y;

        public void show(int i)
        {
            name = new Label();
            name.Text = "P" + i.ToString() + ":";
            name.Location = new Point(x, y);
            name.Show();
            BT = new TextBox();
            BT.Location = new Point(x, y);
            AT = new TextBox();
            AT.Location = new Point(x, y);
            P = new TextBox();
            P.Location = new Point(x, y);
            WT = new TextBox();
            WT.Location = new Point(x, y);
            TA = new TextBox();
            TA.Location = new Point(x, y);
        }
    }
}
