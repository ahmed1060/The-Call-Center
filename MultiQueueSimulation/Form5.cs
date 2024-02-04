using MultiQueueModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiQueueSimulation
{
    public partial class Graph : Form
    {

        int Currchart = 0;
        DataTable ServerMeasures = new DataTable();
        SimulationSystem SSystem = Program.system;

        public Graph()
        {
            InitializeComponent();
        }
   
        private void Form5_Load(object sender, EventArgs e)
        {
           
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = SSystem.StoppingNumber;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Interval = SSystem.StoppingNumber / 10;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.Series[0]["PointWidth"] = "0.8";

            ///################

            ServerMeasures.Columns.Add("ServerID ");
            ServerMeasures.Columns.Add("AverageServiceTime");

            ServerMeasures.Columns.Add("Utilization");
            ServerMeasures.Columns.Add("IdleProbability");


            int id = 1;
            foreach (Server SERVER in SSystem.Servers)
            {
                DataRow row = ServerMeasures.NewRow();
                row["ServerID "] = id;
                row["AverageServiceTime"] = SERVER.AverageServiceTime;
                row["Utilization"] = SERVER.Utilization;
                row["IdleProbability"] = SERVER.Utilization;

                ServerMeasures.Rows.Add(row);
                id++;
            }


            updateGraph();

        }
        void updateGraph()
        {
            chart1.Series[0].Points.Clear();
            // label7.Text = Currchart.ToString();
            label1.Text = (Currchart ).ToString();
            for (int i = 0; i < SSystem.SimulationTable.Count; i++)
            {
                if (SSystem.SimulationTable[i].AssignedServer.ID == Currchart)
                {
                    for (int x = SSystem.SimulationTable[i].StartTime; x <= SSystem.SimulationTable[i].EndTime; x++)
                    {
                        chart1.Series[0].Points.AddXY(x, 1);
                    }

                }
            }



            


            dataGridView1.DataSource = ServerMeasures;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Currchart++;
            if (Currchart > SSystem.Servers.Count)
            {
                Currchart = 1;
            }
            updateGraph();


        }
     
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          


        }
    }
}
