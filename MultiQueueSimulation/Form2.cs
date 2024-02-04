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
    public partial class SimulationTable : Form
    {
        
        public SimulationTable()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SimulationSystem System = Program.system;
            // SimulationSystem system = new SimulationSystem();


            DataTable simulationTable = new DataTable();
            simulationTable.Columns.Add("Customer Number");
            simulationTable.Columns.Add("Random Inter-Arrival");
            simulationTable.Columns.Add("Inter-Arrival");
            simulationTable.Columns.Add("Arrival Time");
            simulationTable.Columns.Add("Random Service");
            simulationTable.Columns.Add("Assigned Server");
            simulationTable.Columns.Add("Start Time");
            simulationTable.Columns.Add("Service Time");
            simulationTable.Columns.Add("End Time");
            simulationTable.Columns.Add("Time in Queue");

            foreach (SimulationCase simulationCase in System.SimulationTable)
            {
                DataRow row = simulationTable.NewRow();
                row["Customer Number"] = simulationCase.CustomerNumber;
                row["Random Inter-Arrival"] = simulationCase.RandomInterArrival;
                row["Inter-Arrival"] = simulationCase.InterArrival;
                row["Arrival Time"] = simulationCase.ArrivalTime;
                row["Random Service"] = simulationCase.RandomService;
                row["Assigned Server"] = simulationCase.AssignedServer.ID;
                row["Start Time"] = simulationCase.StartTime;
                row["Service Time"] = simulationCase.ServiceTime;
                row["End Time"] = simulationCase.EndTime;
                row["Time in Queue"] = simulationCase.TimeInQueue;

                simulationTable.Rows.Add(row);
            }

           
            dataGridView1.DataSource = simulationTable;


        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
