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
    public partial class performanceMeasures : Form
    {
        public performanceMeasures()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SimulationSystem SSystem = Program.system;
            DataTable performanceMeasures = new DataTable();
            performanceMeasures.Columns.Add("AverageWaitingTim");
            performanceMeasures.Columns.Add("MaxQueueLength");
            performanceMeasures.Columns.Add("WaitingProbability");

            DataRow row = performanceMeasures.NewRow();
            row["AverageWaitingTim"] = SSystem.PerformanceMeasures.AverageWaitingTime;
            row["MaxQueueLength"] = SSystem.PerformanceMeasures.MaxQueueLength;
            row["WaitingProbability"] = SSystem.PerformanceMeasures.WaitingProbability;
            performanceMeasures.Rows.Add(row);


            dataGridView1.DataSource = performanceMeasures;


        }
    }
}
