using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCabSystem.MyCabControlSystem;
using MyCabSystem.Passengers;

namespace MyCabSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            int MaxnoOfStops = 20; 
            MyCabControlSystem.MyCabControlSystem myCabControlSystem = new MyCabControlSystem.MyCabControlSystem(2, MaxnoOfStops);
          
            printStatus(myCabControlSystem.status());

            // Intialization Block Bringing both the car in motion from Idle State

            myCabControlSystem.setState(0, 1, 10);// set state of cabs
            myCabControlSystem.setState(1, 3, 8);
            printStatus(myCabControlSystem.status());
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 2, 9));
            myCabControlSystem.RaisePickupRequest(new Passenger(2, 4, 6));
            myCabControlSystem.RaisePickupRequest(new Passenger(3, 5, 7));// Left Direction
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();// move all car by one unit
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            myCabControlSystem.RaisePickupRequest(new Passenger(4, 7, 4));// Right Direction
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
           
            Console.ReadKey();
        }

        private static void printStatus(List<List<int>> statuses)
        {
            Console.WriteLine("\n");
            foreach (List<int> state in statuses)
            {
                Console.WriteLine("status: id: " + state[0]
                        + " curStop: " + state[1]
                        + " destStop: " + state[2]
                        + " passenger count" +state[3]);
            }
            Console.WriteLine("\n");
        }
    }
}
