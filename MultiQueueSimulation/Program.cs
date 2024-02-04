using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections;



namespace MultiQueueSimulation
{
    static class Program
    {
        public static SimulationSystem system = new SimulationSystem();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //E:/7th Semester/Modelling & Simulation/2_Labs/Lab 2_Task1/MultiQueueSimulation/MultiQueueSimulation/TestCases/TestCase1.txt
            string[] lines = File.ReadAllLines(Form1.path_in_form);

            int numberOfServers = 0;
            var interarrivalDistribution = new Dictionary<int, double>();
            int selectionMethod = 0;
            int stoppingCriteria = 0;
            int stoppingNumber = 0;
            int max_num_of_cust_inqueue = 0;
            Queue<int> cust_Queue = new Queue<int>();
            var serviceDistributions = new List<ServiceDistribution>();

            string currentSection = null;

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line == "NumberOfServers")
                    {
                        currentSection = "NumberOfServers";
                    }
                    else if (line == "InterarrivalDistribution")
                    {
                        currentSection = "InterarrivalDistribution";
                    }
                    else if (line == "SelectionMethod")
                    {
                        currentSection = "SelectionMethod";
                    }
                    else if (line == "StoppingCriteria")
                    {
                        currentSection = "StoppingCriteria";
                    }
                    else if (line == "StoppingNumber")
                    {
                        currentSection = "StoppingNumber";
                    }
                    else if (line.StartsWith("ServiceDistribution_Server"))
                    {
                        currentSection = "ServiceDistribution";
                        serviceDistributions.Add(new ServiceDistribution());
                    }
                    else
                    {
                        switch (currentSection)
                        {
                            case "NumberOfServers":
                                numberOfServers = int.Parse(line);
                                break;
                            case "InterarrivalDistribution":
                                var parts = line.Split(',');
                                int key = int.Parse(parts[0]);
                                double value = double.Parse(parts[1]);
                                interarrivalDistribution[key] = value;
                                break;
                            case "SelectionMethod":
                                selectionMethod = int.Parse(line);
                                break;
                            case "StoppingCriteria":
                                stoppingCriteria = int.Parse(line);
                                break;
                            case "StoppingNumber":
                                stoppingNumber = int.Parse(line);
                                break;
                            case "ServiceDistribution":
                                var serviceDistribution = serviceDistributions.Last();
                                var serviceParts = line.Split(',');
                                int serviceKey = int.Parse(serviceParts[0]);
                                double serviceValue = double.Parse(serviceParts[1]);
                                serviceDistribution.Values[serviceKey] = serviceValue;
                                break;
                        }
                    }
                }
            }

            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

            system.NumberOfServers = numberOfServers;
            system.StoppingNumber = stoppingNumber;
            system.StoppingCriteria = (Enums.StoppingCriteria)stoppingCriteria;
            system.SelectionMethod = (Enums.SelectionMethod)selectionMethod;

            decimal cumm_prob_inter = 0;
            for (int i = 0; i < interarrivalDistribution.Keys.Count; i++)
            {
                TimeDistribution inter_arrival_dist = new TimeDistribution();
                system.InterarrivalDistribution.Add(inter_arrival_dist);

                system.InterarrivalDistribution[i].Time = interarrivalDistribution.Keys.ElementAt(i);
                system.InterarrivalDistribution[i].Probability = (decimal)interarrivalDistribution.Values.ElementAt(i);
                cumm_prob_inter += system.InterarrivalDistribution[i].Probability;
                system.InterarrivalDistribution[i].CummProbability = cumm_prob_inter;
                system.InterarrivalDistribution[i].MinRange = (int)((system.InterarrivalDistribution[i].CummProbability - system.InterarrivalDistribution[i].Probability) * 100) + 1;
                system.InterarrivalDistribution[i].MaxRange = (int)(system.InterarrivalDistribution[i].CummProbability * 100);
            }

            decimal cumm_prob_serv = 0;
            for (int j = 0; j < numberOfServers; j++)
            {
                Server servers = new Server();
                system.Servers.Add(servers);

                system.Servers[j].ID = j + 1;
                for (int k = 0; k < serviceDistributions[0].Values.Count; k++)
                {
                    TimeDistribution service_time_dist = new TimeDistribution();
                    system.Servers[j].TimeDistribution.Add(service_time_dist);

                    system.Servers[j].TimeDistribution[k].Time = serviceDistributions[j].Values.Keys.ElementAt(k);
                    system.Servers[j].TimeDistribution[k].Probability = (decimal)serviceDistributions[j].Values.Values.ElementAt(k);
                    cumm_prob_serv += system.Servers[j].TimeDistribution[k].Probability;
                    system.Servers[j].TimeDistribution[k].CummProbability = cumm_prob_serv;
                    system.Servers[j].TimeDistribution[k].MinRange = (int)((system.Servers[j].TimeDistribution[k].CummProbability - system.Servers[j].TimeDistribution[k].Probability) * 100) + 1;
                    system.Servers[j].TimeDistribution[k].MaxRange = (int)(system.Servers[j].TimeDistribution[k].CummProbability * 100);
                }
                cumm_prob_serv = 0;
            }

            // initialize the total working time for each server with zero
            for (int i = 0; i < numberOfServers; i++)
            {
                system.Servers[i].TotalWorkingTime = 0;
                system.Servers[i].number_of_customers = 0;
            }

            //*********Simulation Table Generation*****************************************************

            Random random = new Random();
            int total_number_of_customers = 0;

            //first row in the table
            SimulationCase simulation_table = new SimulationCase();
            system.SimulationTable.Add(simulation_table);

            system.SimulationTable[0].CustomerNumber = 1;
            system.SimulationTable[0].RandomInterArrival = 1;
            system.SimulationTable[0].InterArrival = 0;
            system.SimulationTable[0].ArrivalTime = 0;
            int random_service_number = random.Next(1, 101);
            system.SimulationTable[0].RandomService = random_service_number;

            int Assigned_server_id = 0;
            switch (system.SelectionMethod)
            {
                //HighestPriority
                case (Enums.SelectionMethod)1:
                    Assigned_server_id = 1;
                    break;
                //Random
                case (Enums.SelectionMethod)2:
                    Assigned_server_id = random.Next(1, system.NumberOfServers + 1);
                    break;
                //LeastUtilization
                case (Enums.SelectionMethod)3:
                    Assigned_server_id = 1;
                    break;
            }
            system.SimulationTable[0].AssignedServer = system.Servers[Assigned_server_id - 1];

            system.SimulationTable[0].StartTime = 0;
            system.SimulationTable[0].ServiceTime = get_serv_time(system.SimulationTable[0].RandomService, Assigned_server_id - 1);
            system.SimulationTable[0].EndTime = system.SimulationTable[0].StartTime + system.SimulationTable[0].ServiceTime;
            system.SimulationTable[0].TimeInQueue = system.SimulationTable[0].StartTime - system.SimulationTable[0].ArrivalTime;
            system.Servers[Assigned_server_id - 1].FinishTime = system.SimulationTable[0].EndTime;
            system.Servers[Assigned_server_id - 1].TotalWorkingTime += system.SimulationTable[0].ServiceTime;
            system.Servers[Assigned_server_id - 1].number_of_customers = system.Servers[Assigned_server_id - 1].number_of_customers + 1;
            system.Servers[Assigned_server_id - 1].customers.Add(0);
            total_number_of_customers++;

            if (system.SimulationTable[0].TimeInQueue > 0)
            {
                cust_Queue.Enqueue(0);
                max_num_of_cust_inqueue = cust_Queue.Count;
            }

            //the rest of the rows in the table
            for (int l = 1; l < system.StoppingNumber; l++)
            {
                SimulationCase simulation_table_rest = new SimulationCase();
                system.SimulationTable.Add(simulation_table_rest);

                system.SimulationTable[l].CustomerNumber = l + 1;
                int random_digit_inter = random.Next(1, 101);
                system.SimulationTable[l].RandomInterArrival = random_digit_inter;
                system.SimulationTable[l].InterArrival = get_inter_time(system.SimulationTable[l].RandomInterArrival);
                system.SimulationTable[l].ArrivalTime = system.SimulationTable[l - 1].ArrivalTime + system.SimulationTable[l].InterArrival;
                if (system.StoppingCriteria == (Enums.StoppingCriteria)2)
                {
                    if (system.SimulationTable[l].ArrivalTime > system.StoppingNumber)
                    {
                        system.SimulationTable.RemoveAt(l);
                        break;
                    }
                }
                int random_service_number_rest = random.Next(1, 101);
                system.SimulationTable[l].RandomService = random_service_number_rest;
                system.SimulationTable[l].TimeInQueue = 0;

                int Assigned_server_id_rest = 0;
                switch (system.SelectionMethod)
                {
                    //HighestPriority
                    case (Enums.SelectionMethod)1:
                        Assigned_server_id_rest = get_highest_priority_id(l);
                        break;
                    //Random
                    case (Enums.SelectionMethod)2:
                        Assigned_server_id_rest = get_random_id(l);
                        break;
                    //LeastUtilization
                    case (Enums.SelectionMethod)3:
                        Assigned_server_id_rest = get_least_utilization_id(l);
                        break;
                }
                system.SimulationTable[l].AssignedServer = system.Servers[Assigned_server_id_rest - 1];

                system.SimulationTable[l].StartTime = system.SimulationTable[l].ArrivalTime + system.SimulationTable[l].TimeInQueue;
                system.SimulationTable[l].ServiceTime = get_serv_time(system.SimulationTable[l].RandomService, Assigned_server_id_rest - 1);
                system.SimulationTable[l].EndTime = system.SimulationTable[l].StartTime + system.SimulationTable[l].ServiceTime;
                system.Servers[Assigned_server_id_rest - 1].FinishTime = system.SimulationTable[l].EndTime;
                system.Servers[Assigned_server_id_rest - 1].TotalWorkingTime += system.SimulationTable[l].ServiceTime;
                system.Servers[Assigned_server_id_rest - 1].number_of_customers = system.Servers[Assigned_server_id_rest - 1].number_of_customers + 1;
                system.Servers[Assigned_server_id_rest - 1].customers.Add(l);
                total_number_of_customers++;

                if (system.SimulationTable[l].TimeInQueue > 0)
                {
                    for (int i = 0; i < cust_Queue.Count; i++)
                    {
                        if (system.SimulationTable[l].ArrivalTime >= system.SimulationTable[cust_Queue.Peek()].StartTime)
                        {
                            cust_Queue.Dequeue();
                        }
                    }

                    cust_Queue.Enqueue(l);
                    if (cust_Queue.Count > max_num_of_cust_inqueue)
                    {
                        max_num_of_cust_inqueue = cust_Queue.Count;
                    }
                }
            }

            int sim_end_time = 0;
            for (int i = 0; i < system.SimulationTable.Count; i++)
            {
                if (system.SimulationTable[i].EndTime > sim_end_time)
                {
                    sim_end_time = system.SimulationTable[i].EndTime;
                }
            }

            for (int i = 0; i < system.Servers.Count; i++)
            {
                system.Servers[i].IdleProbability = (decimal)((decimal)sim_end_time - (decimal)(system.Servers[i].TotalWorkingTime)) / (decimal)sim_end_time;
                if (system.Servers[i].number_of_customers == 0)
                {
                    system.Servers[i].AverageServiceTime = 0;
                }
                else
                {
                    system.Servers[i].AverageServiceTime = (decimal)system.Servers[i].TotalWorkingTime / (decimal)system.Servers[i].number_of_customers;
                }
                system.Servers[i].Utilization = (decimal)system.Servers[i].TotalWorkingTime / (decimal)sim_end_time;
            }

            decimal max_number = system.SimulationTable[0].TimeInQueue;
            decimal total_waiting_time = 0;
            decimal total_waited_customers = 0;
            for (int i = 1; i < system.SimulationTable.Count; i++)
            {
                total_waiting_time += system.SimulationTable[i].TimeInQueue;

                if (system.SimulationTable[i].TimeInQueue > max_number)
                {
                    max_number = system.SimulationTable[i].TimeInQueue;
                }

                if (system.SimulationTable[i].TimeInQueue > 0)
                {
                    total_waited_customers++;
                }
            }

            system.PerformanceMeasures.AverageWaitingTime = ((decimal)total_waiting_time / (decimal)total_number_of_customers);
            system.PerformanceMeasures.MaxQueueLength = max_num_of_cust_inqueue;
            system.PerformanceMeasures.WaitingProbability = ((decimal)total_waited_customers / (decimal)total_number_of_customers);

            string testcase = Path.GetFileName(Form1.path_in_form);
            string result = TestingManager.Test(system, testcase);
            MessageBox.Show(result);

            Application.Run(new SimulationTable());
            Application.Run(new performanceMeasures());
            Application.Run(new Graph());
        }



        class ServiceDistribution
        {
            public Dictionary<int, double> Values { get; } = new Dictionary<int, double>();
        }
        static int get_inter_time(int random)
        {
            int generated_number = 0;
            int counter = 0;
            do
            {
                if (random >= system.InterarrivalDistribution[counter].MinRange && random <= system.InterarrivalDistribution[counter].MaxRange)
                {
                    generated_number = system.InterarrivalDistribution[counter].Time;
                }
                counter++;
            } while (counter < system.InterarrivalDistribution.Count);
            return generated_number;
        }
        static int get_serv_time(int random, int server_index)
        {
            int generated_number = 0;
            int counter = 0;
            do
            {
                if (random >= system.Servers[server_index].TimeDistribution[counter].MinRange && random <= system.Servers[server_index].TimeDistribution[counter].MaxRange)
                {
                    generated_number = system.Servers[server_index].TimeDistribution[counter].Time;
                }
                counter++;
            } while (counter < system.Servers[server_index].TimeDistribution.Count);
            return generated_number;
        }
        static int get_highest_priority_id(int sim_table_index)
        {
            int Assigned_server_id_highest = 0;
            int server_counter = 0;
            do
            {
                if (system.Servers[server_counter].FinishTime <= system.SimulationTable[sim_table_index].ArrivalTime)
                {
                    Assigned_server_id_highest = system.Servers[server_counter].ID;
                }
                server_counter++;
            } while (Assigned_server_id_highest == 0 && server_counter < system.Servers.Count);

            int flag = 0;
            if (Assigned_server_id_highest == 0)
            {
                flag = 1;
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    system.Servers[i].diff_for_queue = system.Servers[i].FinishTime - system.SimulationTable[sim_table_index].ArrivalTime;
                }

                int min = system.Servers[0].diff_for_queue;
                Assigned_server_id_highest = system.Servers[0].ID;
                system.SimulationTable[sim_table_index].TimeInQueue = min;
                for (int i = 1; i < system.Servers.Count; i++)
                {
                    if (system.Servers[i].diff_for_queue < min)
                    {
                        min = system.Servers[i].diff_for_queue;
                        Assigned_server_id_highest = system.Servers[i].ID;
                        system.SimulationTable[sim_table_index].TimeInQueue = min;
                    }
                }
            }

            if (flag == 1)
            {
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    if (system.Servers[i].FinishTime == (system.SimulationTable[sim_table_index].ArrivalTime + system.SimulationTable[sim_table_index].TimeInQueue))
                    {
                        Assigned_server_id_highest = system.Servers[i].ID;
                        break;
                    }
                }
            }

            return Assigned_server_id_highest;
        }
        static int get_random_id(int sim_table_index)
        {
            int Assigned_server_id_random = 0;
            Random random = new Random();

            List<int> random_numbers = new List<int>();

            for (int i = 0; i < system.Servers.Count; i++)
            {
                random_numbers.Add(i);
            }

            // Fisher-Yates shuffle algorithm
            for (int i = random_numbers.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = random_numbers[i];
                random_numbers[i] = random_numbers[j];
                random_numbers[j] = temp;
            }

            int server_counter = 0;
            do
            {
                if (system.Servers[random_numbers[server_counter]].FinishTime <= system.SimulationTable[sim_table_index].ArrivalTime)
                {
                    Assigned_server_id_random = system.Servers[random_numbers[server_counter]].ID;
                }
                server_counter++;
            } while (Assigned_server_id_random == 0 && server_counter < system.Servers.Count);

            int flag = 0;
            if (Assigned_server_id_random == 0)
            {
                flag = 1;
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    system.Servers[i].diff_for_queue = system.Servers[i].FinishTime - system.SimulationTable[sim_table_index].ArrivalTime;
                }

                int min = system.Servers[0].diff_for_queue;
                Assigned_server_id_random = system.Servers[0].ID;
                system.SimulationTable[sim_table_index].TimeInQueue = min;
                for (int i = 1; i < system.Servers.Count; i++)
                {
                    if (system.Servers[i].diff_for_queue < min)
                    {
                        min = system.Servers[i].diff_for_queue;
                        Assigned_server_id_random = system.Servers[i].ID;
                        system.SimulationTable[sim_table_index].TimeInQueue = min;
                    }
                }
            }

            if (flag == 1)
            {
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    if (system.Servers[random_numbers[i]].FinishTime == (system.SimulationTable[sim_table_index].ArrivalTime + system.SimulationTable[sim_table_index].TimeInQueue))
                    {
                        Assigned_server_id_random = system.Servers[random_numbers[i]].ID;
                        break;
                    }
                }
            }

            return Assigned_server_id_random;
        }
        class IndexedValue
        {
            public int Value { get; }
            public int Index { get; }

            public IndexedValue(int value, int index)
            {
                Value = value;
                Index = index;
            }
        }

        static int get_least_utilization_id(int sim_table_index)
        {
            int Assigned_server_id_util = 0;

            IndexedValue[] indexedArray = new IndexedValue[system.Servers.Count];
            for (int i = 0; i < system.Servers.Count; i++)
            {
                indexedArray[i] = new IndexedValue(system.Servers[i].TotalWorkingTime, i);
            }
            indexedArray = indexedArray.OrderBy(item => item.Value).ToArray();
            //server counter
            int s_c = 0;
            do
            {
                if (system.Servers[indexedArray[s_c].Index].FinishTime <= system.SimulationTable[sim_table_index].ArrivalTime)
                {
                    Assigned_server_id_util = indexedArray[s_c].Index + 1;
                }
                s_c++;
            } while (Assigned_server_id_util == 0 && s_c < system.Servers.Count);

            int flag = 0;
            if (Assigned_server_id_util == 0)
            {
                flag = 1;
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    system.Servers[i].diff_for_queue = system.Servers[i].FinishTime - system.SimulationTable[sim_table_index].ArrivalTime;
                }

                int min = system.Servers[0].diff_for_queue;
                Assigned_server_id_util = system.Servers[0].ID;
                system.SimulationTable[sim_table_index].TimeInQueue = min;
                for (int i = 1; i < system.Servers.Count; i++)
                {
                    if (system.Servers[i].diff_for_queue < min)
                    {
                        min = system.Servers[i].diff_for_queue;
                        Assigned_server_id_util = system.Servers[i].ID;
                        system.SimulationTable[sim_table_index].TimeInQueue = min;
                    }
                }
            }

            if (flag == 1)
            {
                int customer_start_time = (system.SimulationTable[sim_table_index].ArrivalTime + system.SimulationTable[sim_table_index].TimeInQueue);
                for (int i = 0; i < system.Servers.Count; i++)
                {
                    //compare the finish time of each server with the start time of the customer
                    if (system.Servers[indexedArray[i].Index].FinishTime == customer_start_time)
                    {
                        Assigned_server_id_util = indexedArray[i].Index + 1;
                        break;
                    }
                }
            }

            return Assigned_server_id_util;
        }
    }
}