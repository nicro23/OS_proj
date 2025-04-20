using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace OS_proj
{
    /* to add:
     * 1. functional calc button
     * 2. change prio to check box instead of being in combo box
     * 3. make a best algo button that calcualtes the best algorithm
     * 4. draw a grant chart when the user presses calculate
     */
    public partial class Form1: Form
    {
        Label P_label, BT, AT, Prio, WT, TA, Algo, quantum;
        TextBox quantum_box;
        CheckBox priority;
        ComboBox Algo_box;
        Button b, calc;
        RadioButton pre, non_pre;
        List<process> pl;
        process p;
        int process_i;
        public Form1()
        {
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            pl = new List<process>();
            priority = new CheckBox();
            pre = new RadioButton();
            non_pre = new RadioButton();
            Algo_box = new ComboBox();
            process_i = 1;
        }

        private void Algo_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Algo_box.Items[Algo_box.SelectedIndex].ToString())
            {
                case "First Come First Served":
                    non_pre.Enabled = false;
                    non_pre.Checked = true;
                    pre.Enabled = false;
                    pre.Checked = false;

                    quantum_box.ReadOnly = true;
                    break;
                case "Shortest Job First":

                    quantum_box.ReadOnly = true;
                    non_pre.Enabled = true;
                    pre.Enabled = true;
                    break;
                case "Round Robin":
                    quantum_box.ReadOnly = false;
                    non_pre.Enabled = false;
                    non_pre.Checked = false;
                    pre.Enabled = false;
                    pre.Checked = true;

                    break;

            }
        }

        private void create_headers(int x_start,int y, int margin,int section_margin)
        {
            Label P_label = new Label();
            P_label.Text = "Process";
            P_label.AutoSize = true;
            P_label.Location = new Point(x_start, y);
            Controls.Add(P_label);
            x_start += P_label.Width;
            BT = new Label();
            BT.Text = "Burst time";
            BT.AutoSize = true;
            BT.Location = new Point(x_start + margin, y);
            Controls.Add(BT);
            x_start += BT.Width + margin; 
            AT = new Label();
            AT.Text = "Arrival time";
            AT.AutoSize = true;
            AT.Location = new Point(x_start + margin, y);
            Controls.Add(AT);
            x_start += AT.Width + margin;
            Prio = new Label();
            Prio.Text = "Priority";
            Prio.AutoSize = true;
            Prio.Location = new Point(x_start + margin, y);
            Controls.Add(Prio);
            x_start += Prio.Width + margin + section_margin;
            WT = new Label();
            WT.Text = "Waiting time";
            WT.AutoSize = true;
            WT.Location = new Point(x_start + margin, y);
            Controls.Add(WT);
            x_start += WT.Width + margin;
            TA = new Label();
            TA.Text = "Turnaround time";
            TA.AutoSize = true;
            TA.Location = new Point(x_start + margin, y);
            Controls.Add(TA);
            x_start += TA.Width + margin;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int margin, section_margin;
            margin = 10;
            section_margin = 20;
            Size = new Size(815, 490);

            create_headers(30, 20, margin, section_margin);


            //add choices
            Algo = new Label();
            Algo.Text = "Algorithm: ";
            Algo.AutoSize = true;
            Algo.Location = new Point(465, 20);
            Controls.Add(Algo);

            Algo_box = new ComboBox();
            Algo_box.Items.Add("First Come First Served");
            Algo_box.Items.Add("Shortest Job First");
            Algo_box.Items.Add("Round Robin");
            Algo_box.Items.Add("Priority");
            Algo_box.Size = new Size(130, 20);
            Algo_box.Location = new Point(465, 45);
            Controls.Add(Algo_box);

            Algo_box.SelectedIndexChanged += Algo_box_SelectedIndexChanged;

            //add checkbox
            priority.Text = "Priority";
            priority.Location = new Point(465, 70);
            priority.CheckedChanged += Priority_CheckedChanged;
            Controls.Add(priority);
            //add radio_buttons
            pre.Text = "Preemptive";
            pre.Location = new Point(465, 90);
            Controls.Add(pre);
            non_pre.Text = "Non-Preemptive";
            non_pre.Location = new Point(465, 110);
            Controls.Add(non_pre);

            //quantum input 
            quantum = new Label();
            quantum.Text = "Quantum: ";
            quantum.AutoSize = true;
            quantum.Location = new Point(465, 135);
            Controls.Add(quantum);

            quantum_box = new TextBox();
            quantum_box.ReadOnly = true;
            quantum_box.Size = new Size(quantum.Width, quantum.Height);
            quantum_box.Location = new Point(465, 150);
            Controls.Add(quantum_box);

            //create buttons
            b = new Button();
            b.Text = "New process";
            b.Location = new Point(465, 180);
            b.Size = new Size(90, 25);
            //b.AutoSize = true;
            b.Click += B_Click;
            Controls.Add(b);

            calc = new Button();
            calc.Text = "Calculate";
            calc.Location = new Point(465, 210);
            calc.AutoSize = true;
            calc.Click += Calc_Click;
            Controls.Add(calc);
            //add first process
            add_process(process_i, 35, 45, margin, section_margin);
            process_i++;
        }

        private void Priority_CheckedChanged(object sender, EventArgs e)
        {
            if(priority.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < pl.Count; i++)
                {
                    pl[i].Prio.ReadOnly = false;
                }
            }
            else
            {
                for (int i = 0; i < pl.Count; i++)
                {
                    pl[i].Prio.ReadOnly = true;
                }
            }
        }

        private void Calc_Click(object sender, EventArgs e)
        {
            switch (Algo_box.Items[Algo_box.SelectedIndex].ToString())
            {
                case "First Come First Served":
                //always non-preemptive
                    break;

                case "Shortest Job First":

                    break;

                case "Round Robin":
                    //always preemptive
                    break;
            }
        }

        private void B_Click(object sender, EventArgs e)
        {
            int y_margin = 15 + (process_i * 30);
            if (process_i <= 13)
            {
                add_process(process_i, 35, y_margin, 10, 20);
                process_i++;
            }
        }

        private void add_process(int i, int x_tmp, int y_tmp, int margin, int section_margin)
        {
            p = new process();
            p.x = x_tmp;
            p.y = y_tmp;

            //name header
            p.name.Text = "P" + i.ToString() + ":";
            p.name.Font = new Font(p.name.Font.FontFamily, 10);
            p.name.AutoSize = true;
            //set locations
            p.name.Location = new Point(p.x, p.y);
            x_tmp += p.name.Width / 2;
            p.BT.Location = new Point(x_tmp, p.y);
            x_tmp += BT.Width + margin;
            p.AT.Location = new Point(x_tmp, p.y);
            x_tmp += AT.Width + margin;
            p.Prio.Location = new Point(x_tmp, p.y);
            x_tmp += Prio.Width + margin + section_margin;
            p.WT.Location = new Point(x_tmp, p.y);
            x_tmp += WT.Width + margin;
            p.TA.Location = new Point(x_tmp, p.y);

            //set sizes
            p.BT.Size = new Size(BT.Width, BT.Height);
            p.AT.Size = new Size(AT.Width, AT.Height);
            p.Prio.Size = new Size(Prio.Width, Prio.Height);
            p.WT.Size = new Size(WT.Width, WT.Height);
            p.TA.Size = new Size(TA.Width, TA.Height);

            //set enable attrib
            if (pl.Count > 0)
            {
                if (pl[0].Prio.ReadOnly == true)
                    p.Prio.ReadOnly = true;
                else
                    p.Prio.ReadOnly = false;
            }
            else
            {
                p.Prio.ReadOnly = true;
            }
            p.WT.ReadOnly = true;
            p.TA.ReadOnly = true;
            //add to control
            Controls.Add(p.name);
            Controls.Add(p.BT);
            Controls.Add(p.AT);
            Controls.Add(p.Prio);
            Controls.Add(p.WT);
            Controls.Add(p.TA);

            //add process to list
            pl.Add(p);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
    public class process
    {
        public TextBox BT, AT, Prio, WT, TA;
        public Label name;
        public int x, y;

        public process()
        {
            name = new Label();
            BT = new TextBox();
            AT = new TextBox();
            Prio = new TextBox();
            WT = new TextBox();
            TA = new TextBox();
        }
    }
}
