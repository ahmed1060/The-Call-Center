using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
            this.customers = new List<int>();
           this.Intervals= new List<KeyValuePair<int, int>>();
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; } 
        public decimal Utilization { get; set; }

        public List<TimeDistribution> TimeDistribution;

        //optional if needed use them
        public int FinishTime { get; set; }
        public int diff_for_queue { get; set; }
        public int number_of_customers { get; set; }
        public List<int> customers { get; set; }
       
        public List<KeyValuePair<int, int>> Intervals;
        public int TotalWorkingTime { get; set; }
    }
}
