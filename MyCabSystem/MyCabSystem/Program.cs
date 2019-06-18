using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCabSystem.MyCabControlSystem;
namespace MyCabSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            MyCabControlSystem.MyCabControlSystem myCabControlSystem = new MyCabControlSystem.MyCabControlSystem(2, 20);
            printStatus(myCabControlSystem.status());
            myCabControlSystem.pickup(2, 4);
            printStatus(myCabControlSystem.status());
            myCabControlSystem.pickup(3, 9);
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            myCabControlSystem.step();
            myCabControlSystem.step();
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.setDest(0,4);// Once user boarded setting up destination for cab1
            myCabControlSystem.setDest(1, 9);//Once user boarded setting up destination for cab2
          
            printStatus(myCabControlSystem.status());
            
            myCabControlSystem.pickup(5, 1);
            myCabControlSystem.pickup(2, 1);
            printStatus(myCabControlSystem.status());
             myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
             myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.step();
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
                        + " destStop: " + state[2]);
            }
            Console.WriteLine("\n");
        }
    }
}
