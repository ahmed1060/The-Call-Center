# The Call Center

This project implements a simulation of a multi-queue system using C# and Windows Forms. The simulation aims to model and analyze the performance of a system with multiple servers serving arriving customers based on various selection methods.

## Key Features:
- **Initialization**: Input parameters such as the number of servers, interarrival distributions, selection methods, and service distributions are read from a user-specified file to initialize the simulation system.
  
- **Simulation Execution**: Customers arrive according to specified interarrival distributions and are assigned to servers using selection methods like Highest Priority, Random, and Least Utilization. The simulation generates a simulation table capturing customer arrival, service, and departure times.

- **Performance Analysis**: Performance measures such as average waiting time, maximum queue length, and waiting probability are calculated throughout the simulation to evaluate the efficiency of the system.

- **Testing**: The simulation system is rigorously tested using predefined test cases to ensure correctness and reliability.

## Usage:
1. Clone the repository to your local machine.
2. Build and run the project using Visual Studio or any compatible IDE.
3. Specify input parameters in the provided file.
4. Run the simulation and analyze the results using the visualization interface.

## Implementation Details:
- The project utilizes C# for implementation, with Windows Forms used for visualization.
- Various helper methods are employed to calculate interarrival times, service times, and assign customers to servers based on different selection methods.

## Contributing:
Contributions are welcome! If you have suggestions, feature requests, or bug reports, please open an issue or submit a pull request.

## License:
This project is licensed under the [MIT License](LICENSE).
