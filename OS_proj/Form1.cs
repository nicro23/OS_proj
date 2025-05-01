using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
namespace OS_proj
{
    /* to add:
     * 1. functional calc button
     * 2. change prio to check box instead of being in combo box
     * 3. make a best algo button that calcualtes the best algorithm
     * 4. draw a grant chart when the user presses calculate
     */

    public partial class Form1 : Form
    {
        Label P_label, BT, AT, Prio, WT, TA, Algo, quantum;
        TextBox quantum_box;
        CheckBox priority;
        ComboBox Algo_box;
        Button b, calc;
        RadioButton pre, non_pre;
        List<process> pl, pl2 = new List<process>();
        process p;
        int process_i;
        List<StartAndEnd> List_S_And_E;
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
                    priority.Enabled = true;
                    priority.Checked = false;
                    break;
                case "Shortest Job First":

                    quantum_box.ReadOnly = true;
                    non_pre.Enabled = true;
                    pre.Enabled = true;
                    priority.Enabled = true;
                    priority.Checked = false;
                    break;
                case "Round Robin":
                    quantum_box.ReadOnly = false;
                    non_pre.Enabled = false;
                    non_pre.Checked = false;
                    pre.Enabled = false;
                    pre.Checked = true;
                    priority.Enabled = true;
                    priority.Checked = false;
                    break;
                case "Priority":
                    priority.Enabled = false;
                    priority.Checked = true;
                    break;
            }

        }
        private void create_headers(int x_start, int y, int margin, int section_margin)
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

                for (int i = 0; i < pl.Count; i++)
                {
                    pl[i].Prio.ReadOnly = !priority.Checked;
                }
        }
        //-----
        public bool IsValid(string T)
        {
            char C; int r;
            if (T.Length == 0)
            {
                MessageBox.Show("Please,Fill the Blank");
                return false;
            }
            else
            {
                for (int j = 0; j < T.Length; j++)
                {
                    C = T[j];
                    r = C - '0';
                    if (r < 0 || r > 9)
                    {
                        if (C != '.')
                        {
                            MessageBox.Show("Please,Insert Numbers only");
                            return false;
                        }
                        
                    }
                }
            }
            return true;
        }
        public bool isAllZeroInArrivalTime(List<process> Lp)
        {

            for (int i = 0; i < Lp.Count; i++)
            {
                Lp[i].f = 0;
                if (Lp[i].at != 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckBurstTimeIsGreaterThanAllArrivalTime(List<process> Lp,process p)
        {
            if (p.e < Lp[Lp.Count - 1].at)
            {
                return false;
            }
            return true;
        }
        public void SortedArrivaltime(List<process> Lp)
        {
            for(int i=0;i<Lp.Count; i++)
            {
                for(int j=i+1;j<Lp.Count; j++)
                {
                    if(Lp[i].at > Lp[j].at)
                    {
                        process p = Lp[i];
                        Lp[i] = Lp[j];
                        Lp[j] = p;
                    }
                    
                }
            }
            
        }
        public void SortedPiority(List<process> Lp)
        {
            for (int i = 0; i < Lp.Count; i++)
            {
                for (int j = i + 1; j < Lp.Count; j++)
                {
                    if (Lp[i].prio > Lp[j].prio)
                    {
                        process p = Lp[i];
                        Lp[i] = Lp[j];
                        Lp[j] = p;
                    }
                    
                }
            }
        }
        

        public void RemoveProcessThat_btEq0(List<process> Lp)
        {
            for (int i = 0; i < Lp.Count; i++)
            {
                if (Lp[i].bt == 0)
                {
                    Lp.RemoveAt(i);i--;
                }
            }
        }
        public void StartAndEndPoint(List<process> Lp, double s1, double s2)
        {
            double sum = s1, sum2 = s2;
            StartAndEnd S_E;
            for (int i = 0; i < Lp.Count; i++)
            {
                S_E = new StartAndEnd();
                sum2 += Lp[i].bt;
                if (i == 0)
                {
                    Lp[i].s = s1;
                    Lp[i].e = sum2;
                }
                else
                {
                    Lp[i].s = sum;
                    Lp[i].e = sum2;
                }
                sum += Lp[i].bt;
                S_E.s = Lp[i].s;
                S_E.e = Lp[i].e;
                S_E.p = Lp[i];
                List_S_And_E.Add(S_E);
            }
            //for (int z = 0; z < List_S_And_E.Count; z++)
            //{
            //    MessageBox.Show("" + List_S_And_E[z].s + "," + List_S_And_E[z].e + "," + List_S_And_E[z].p.at);
            //}
        }
        public double WaitingTime(process P,double A,double B)
        {
            double w;
            w = A - B;
            return w;
        }
        public double TurnArroundTime(process P, double A, double B)
        {
            return 0;
        }
        public void Scheduling(List<process> Lp)
        {
            //double sum = 0,sum2=0;
            int f = 0;
            StartAndEnd S_E;
            List<process>InProcess = new List<process>(); 
            List<process> Finish = new List<process>(); 
            process pnn = new process();
            for (int i = 0; i < Lp.Count; i++)
            {
                S_E = new StartAndEnd();
                if (i < 2)
                {
                    if (i == 0)
                    {
                        Lp[i].s = 0; Lp[i].wt = 0;
                        if (Lp[i].prio <= Lp[i + 1].prio)
                        {
                            Lp[i].e = Lp[i].bt; Lp[i].bt = 0;
                        }
                        else
                        {
                            Lp[i].bt -= Lp[i + 1].at;
                            Lp[i].e = Lp[i + 1].at;
                        }
                        pnn = Lp[i];
                        S_E.s = Lp[i].s;
                        S_E.e = Lp[i].e;
                        S_E.p = Lp[i];
                        S_E.f = 0;
                        List_S_And_E.Add(S_E);
                    }
                    else
                    {
                        double e = Lp[i].e;
                        if (Lp[i].prio <= Lp[i + 1].prio)
                        {
                            Lp[i].s = Lp[i - 1].e;
                            Lp[i].e = Lp[i].bt + Lp[i - 1].e; Lp[i].bt = 0;
                            Lp[i].wt += Lp[i].s - e;
                        }
                        else
                        {
                            Lp[i].s = Lp[i - 1].e;
                            Lp[i].bt -= (Lp[i + 1].at - Lp[i - 1].e);
                            Lp[i].e = Lp[i + 1].at;
                            Lp[i].wt += Lp[i].s - e;

                        }
                        pnn = Lp[i];
                        S_E.s = Lp[i].s;
                        S_E.e = Lp[i].e;
                        S_E.p = Lp[i];
                        S_E.f = 0;
                        List_S_And_E.Add(S_E);
                    }
                    if (Lp[i].bt != 0)
                    {
                        InProcess.Add(Lp[i]);
                        SortedPiority(InProcess);
                    }

                }
                else
                {
                    pnn = Lp[i];
                    if (InProcess.Count != 0 && InProcess[0].prio <= Lp[i].prio)
                    {
                        pnn = InProcess[0];
                        InProcess.RemoveAt(0);
                        if (Lp[i].bt != 0)
                        {
                            InProcess.Add(Lp[i]);
                            SortedPiority(InProcess);
                        }
                    }
                    if (i == Lp.Count - 1)
                    {
                        pnn.s = List_S_And_E[List_S_And_E.Count - 1].e;
                        pnn.e = pnn.bt + pnn.s; 
                        pnn.bt = 0;
                    }
                    else
                    {
                        if (pnn.prio <= Lp[i + 1].prio)
                        {
                            pnn.s = List_S_And_E[List_S_And_E.Count - 1].e;
                            double j = 1;
                            for (; j <= pnn.bt; j++)
                            {
                                if (j+ pnn.s == Lp[Lp.Count - 1].at)
                                {
                                    break;
                                }
                            }
                            pnn.e=j+ pnn.s;
                            pnn.bt -= j;
                            //pnn.e =??;// pnn.bt + pnn.s; 
                            //pnn.bt = 0;
                        }
                        else
                        {
                            pnn.s = List_S_And_E[List_S_And_E.Count - 1].e;
                            pnn.bt -= (Lp[i + 1].at - pnn.s);
                            pnn.e = Lp[i + 1].at;
                        }
                    }
                    if (pnn.bt != 0)
                    {
                        InProcess.Add(pnn);
                        SortedPiority(InProcess);

                    }
                    S_E.s = pnn.s;
                    S_E.e = pnn.e;
                    S_E.p = pnn;
                    S_E.f = 0;
                    List_S_And_E.Add(S_E);

                }
                if (CheckBurstTimeIsGreaterThanAllArrivalTime(Lp, pnn))
                {
                    f = 1;
                    RemoveProcessThat_btEq0(Lp);
                    SortedPiority(Lp);
                    for (int k = 0; k < Lp.Count; k++)
                    {
                        if (k!=Lp.Count-1&&Lp[k].prio == Lp[k + 1].prio && Lp[k].at > Lp[k + 1].at)
                        {
                            process p = Lp[k];
                            Lp[k] = Lp[k + 1];
                            Lp[k + 1] = p;
                        }
                    }
                    //for (int h = 0; h < Lp.Count; h++)
                    //{
                    //    MessageBox.Show("" + Lp[h].at);
                    //}
                    StartAndEndPoint(Lp, List_S_And_E[List_S_And_E.Count - 1].e, List_S_And_E[List_S_And_E.Count - 1].e);
                    
                    break;
                }
            }
            if (f == 0)
            {
                StartAndEndPoint(InProcess, List_S_And_E[List_S_And_E.Count - 1].e, List_S_And_E[List_S_And_E.Count - 1].e);
                InProcess.Clear();
            }
        }
        public void SchedulingNonPreemptive(List<process> Lp)
        {
            //double sum = 0,sum2=0;
            int f = 0;
            StartAndEnd S_E;
            List<process> InProcess = new List<process>();
            process pnn = new process();           
            for (int i = 0; i < Lp.Count; i++)
            {
                S_E = new StartAndEnd();
                if (i == 0)
                {
                    Lp[i].s = 0;
                    Lp[i].e = Lp[i].bt; Lp[i].bt = 0;
                    pnn = Lp[i];
                    S_E.s = Lp[i].s;
                    S_E.e = Lp[i].e;
                    S_E.p = Lp[i];
                    S_E.f = 0;
                    List_S_And_E.Add(S_E);
                }
                else
                {
                    pnn = Lp[i];
                    if (i != Lp.Count - 1)
                    {
                        for(int j = i; j < Lp.Count; j++)
                        {
                            if (Lp[i - 1].e >= Lp[j].at)
                            {
                                InProcess.Add(Lp[j]);
                            }
                        }
                        if(InProcess.Count > 0)
                        {
                            SortedPiority(InProcess);
                            pnn = InProcess[0];
                            InProcess.RemoveAt(0);
                        }
                        pnn.s = List_S_And_E[i - 1].e;
                        pnn.e = pnn.bt + pnn.s; pnn.bt = 0;
                    }
                    else
                    {
                        pnn.s = List_S_And_E[i - 1].e;
                        pnn.e = pnn.bt + List_S_And_E[i - 1].e; pnn.bt = 0;
                    }
                    S_E.s =pnn.s;
                    S_E.e = pnn.e;
                    S_E.p = pnn;
                    S_E.f = 0;
                    List_S_And_E.Add(S_E);
                }
                if (CheckBurstTimeIsGreaterThanAllArrivalTime(Lp, pnn))
                {
                    f = 1;
                    RemoveProcessThat_btEq0(Lp);
                    SortedPiority(Lp);
                    for (int k = 0; k < Lp.Count; k++)
                    {
                        if (k != Lp.Count - 1 && Lp[k].prio == Lp[k + 1].prio && Lp[k].at > Lp[k + 1].at)
                        {
                            process p = Lp[k];
                            Lp[k] = Lp[k + 1];
                            Lp[k + 1] = p;
                        }
                    }
                    //for (int h = 0; h < Lp.Count; h++)
                    //{
                    //    MessageBox.Show("" + Lp[h].at);
                    //}
                    StartAndEndPoint(Lp, List_S_And_E[List_S_And_E.Count - 1].e, List_S_And_E[List_S_And_E.Count - 1].e);

                    break;
                }
            }
            if (f == 0)
            {
                StartAndEndPoint(Lp, List_S_And_E[List_S_And_E.Count - 1].e, List_S_And_E[List_S_And_E.Count - 1].e);
            }
        }
        public void Priority_preemptive(List<process> Lp)
        {
            List<process> Lp2 = new List<process>();
            List_S_And_E = new List<StartAndEnd>();
            for (int i=0; i<Lp.Count; i++)
            {
                process pnn =new process();
                pnn.prio = Lp[i].prio;
                pnn.at = Lp[i].at;
                Lp2.Add(pnn);
            }
            SortedArrivaltime(Lp);
            Scheduling(Lp);
            double S = 0, E = 0; double Wt = 0, TAT = 0;
            for (int i = 0; i < Lp2.Count; i++)
            {
                for (int j = 0; j < List_S_And_E.Count; j++)
                {
                    if (Lp2[i].at == List_S_And_E[j].p.at && Lp2[i].prio == List_S_And_E[j].p.prio)
                    {
                        if (Lp2[i].f == 0)
                        {
                            Lp2[i].wt = WaitingTime(Lp2[i], List_S_And_E[j].s, Lp2[i].at);
                            Lp2[i].f = 1;
                            S = List_S_And_E[j].s;
                            E = List_S_And_E[j].e;
                        }
                        else
                        {
                            Lp2[i].wt += WaitingTime(Lp2[i], List_S_And_E[j].s, E);
                            S = List_S_And_E[j].s;
                            E = List_S_And_E[j].e;
                        }
                    }
                }
                pl[i].WT.Text = "" + Lp2[i].wt;
                pl[i].TA.Text = "" + (E - Lp2[i].at);
                Wt += Lp2[i].wt;
                TAT += E - Lp2[i].at;
            }

            Wt /= Lp2.Count;
            TAT /= Lp2.Count;
            MessageBox.Show("" + Wt + "," + TAT);
            List_S_And_E = null;
        }
        public void Non_Priority_preemptive(List<process> Lp)
        {
            List_S_And_E = new List<StartAndEnd>();
            List<process> Lp2 = new List<process>();
            for (int i = 0; i < Lp.Count; i++)
            {
                process pnn = new process();
                pnn.prio = Lp[i].prio;
                pnn.at = Lp[i].at;
                Lp2.Add(pnn);
            }

            if (isAllZeroInArrivalTime(Lp))
            {
                SortedPiority(Lp);
                StartAndEndPoint(Lp, 0, 0);
            }
            else
            {
                SortedPiority(Lp);
                SortedArrivaltime(Lp);
                SchedulingNonPreemptive(Lp);
            }
            double S = 0, E = 0; double Wt = 0, TAT = 0;
            for (int i = 0; i < Lp2.Count; i++)
            {
                for (int j = 0; j < List_S_And_E.Count; j++)
                {
                    if (Lp2[i].at == List_S_And_E[j].p.at && Lp2[i].prio == List_S_And_E[j].p.prio)
                    {
                        if (Lp2[i].f == 0)
                        {
                            Lp2[i].wt = WaitingTime(Lp2[i], List_S_And_E[j].s, Lp2[i].at);
                            Lp2[i].f = 1;
                            S = List_S_And_E[j].s;
                            E = List_S_And_E[j].e;
                        }
                        else
                        {
                            Lp2[i].wt += WaitingTime(Lp2[i], List_S_And_E[j].s, E);
                            S = List_S_And_E[j].s;
                            E = List_S_And_E[j].e;
                        }
                    }
                }
                pl[i].WT.Text = "" + Lp2[i].wt;
                pl[i].TA.Text = "" + (E - Lp2[i].at);
                Wt += Lp2[i].wt;
                TAT += E - Lp2[i].at;
            }
            Wt /= Lp2.Count;
            TAT /= Lp2.Count;
            MessageBox.Show("" + Wt + "," + TAT);
        }

        private void Calc_Click(object sender, EventArgs e)
        {
            if (Algo_box.SelectedIndex == -1)
             {
                MessageBox.Show("Please select a scheduling algorithm first.");
                return;
            }
            //pl[0].BT.Text = "3";
            //pl[1].BT.Text = "5";
            //pl[2].BT.Text = "4";
            //pl[3].BT.Text = "2";
            //pl[4].BT.Text = "9";
            //pl[5].BT.Text = "4";
            //pl[6].BT.Text = "10";

            //pl[0].AT.Text = "0";
            //pl[1].AT.Text = "2";
            //pl[2].AT.Text = "1";
            //pl[3].AT.Text = "4";
            //pl[4].AT.Text = "6";
            //pl[5].AT.Text = "5";
            //pl[6].AT.Text = "7";

            //pl[0].Prio.Text = "3";
            //pl[1].Prio.Text = "6";
            //pl[2].Prio.Text = "3";
            //pl[3].Prio.Text = "5";
            //pl[4].Prio.Text = "7";
            //pl[5].Prio.Text = "4";
            //pl[6].Prio.Text = "10";

            for (int i = 0; i < pl.Count; i++)
            {
                if (!IsValid(pl[i].BT.Text) || !IsValid(pl[i].AT.Text) || (priority.Checked && !IsValid(pl[i].Prio.Text)))
                {
                    break;
                }
                else
                {
                    pl[i].bt = Double.Parse(pl[i].BT.Text);
                    pl[i].at = Double.Parse(pl[i].AT.Text);
                    if (priority.Checked)
                    {
                        pl[i].prio = int.Parse(pl[i].Prio.Text);
                    }
                }
            }
            List<process> LpCopy = new List<process>();
            for (int i = 0; i < pl.Count; i++)
            {
                process pnn = new process();
                pnn = pl[i];
                LpCopy.Add(pnn);
            }
            if (Algo_box.SelectedIndex != -1)
            {
            switch (Algo_box.Items[Algo_box.SelectedIndex].ToString())
            {
                case "First Come First Served":
                    //always non-preemptive
                    break;

                case "Shortest Job First":
                    SJF_Algo();
                    break;

                case "Round Robin":
                    //always preemptive
                    break;
                case "Priority":
                    //take numbers
                    if (pre.Checked)
                    {
                        Priority_preemptive(LpCopy);
                        
                    }
                    if (non_pre.Checked)
                    {
                        Non_Priority_preemptive(LpCopy);
                    }
                    break;
            }
            LpCopy = null;
        }

        }

        private void SJF_Algo()
        {
            List<ProcessData> processes = new List<ProcessData>();

            for (int i = 0; i < pl.Count; i++)
            {
                int prio = priority.Checked ? int.Parse(pl[i].Prio.Text) : 0;
                processes.Add(new ProcessData(pl[i], prio));
            }

            int time = 0;
            while (processes.Count > 0)
            {
                int rj_burst = -1;
                int rj_prio = -1;
                int rj_idx = -1;
                for (int i = 0; i < processes.Count; i++)
                {
                    int arrival = processes[i].arrival;
                    int burst = processes[i].burst;
                    int prio = processes[i].priority;

                    if (arrival <= time && (rj_burst == -1 || burst < rj_burst))
                    {
                        if (rj_burst == -1 || !priority.Checked || prio < rj_prio)
                        {
                            rj_burst = burst;
                            rj_prio = prio;
                            rj_idx = i;
                        }
                    }
                }

                ProcessData running_job = processes[rj_idx];
                ProcessData nsj = null; // nsj = next shortest job
                for (int i = 0; (pre.Checked || priority.Checked) && i < processes.Count; i++)
                {
                    int arrival = processes[i].arrival;
                    int burst = processes[i].burst;
                    int prio = processes[i].priority;

                    if (arrival > time
                        && running_job.burst - (arrival - time) > burst
                        && (nsj == null || (arrival < nsj.arrival && burst < nsj.burst)))
                    {
                        if (nsj == null || !priority.Checked || prio < nsj.priority)
                        {
                            nsj = processes[i];
                        }
                    }
                }

                int time_inc = nsj != null ? nsj.arrival - time : running_job.burst;
                running_job.burst -= time_inc;
                time += time_inc;

                if (running_job.burst == 0)
                {
                    running_job.turn_around = time - running_job.arrival;
                    running_job.waiting = running_job.turn_around - running_job.original_burst;

                    running_job.WriteBack();
                    processes.RemoveAt(rj_idx);
                }
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
        public double bt, at, s, e,sold,eold,wt,ta;
        public int prio,f;
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
    public class StartAndEnd
    {
        public double s, e, sold, eold;
        public int f;
        public process p;
    }

    public class ProcessData
    {
        process process;
        public int arrival, burst, priority, original_burst;
        public int waiting, turn_around;

        public ProcessData(process process, int priority)
        {
            this.process = process;
            arrival = int.Parse(process.AT.Text);
            burst = int.Parse(process.BT.Text);
            original_burst = int.Parse(process.BT.Text);
            this.priority = priority;
            waiting = 0;
            turn_around = 0;
        }

        public void WriteBack()
        {
            process.WT.Text = waiting.ToString();
            process.TA.Text = turn_around.ToString();
        }
    }
}
