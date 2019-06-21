using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCabSystem.MyCabControlSystem;
using MyCabSystem.Passengers;

namespace CabSimulatorTest
{
    [TestClass]
    public class Simulator
    {
        int MaxnoOfStops = 20;
        int noOfCars = 2;
        MyCabControlSystem myCabControlSystem;//= new MyCabControlSystem.MyCabControlSystem(2, MaxnoOfStops);

        private static void printStatus(List<List<int>> statuses)
        {
            Console.WriteLine("\n");
            foreach (List<int> state in statuses)
            {
                Console.WriteLine("status: id: " + state[0]
                        + " curStop: " + state[1]
                        + " destStop: " + state[2]
                        + " passenger count: " + state[3]);
            }
            Console.WriteLine("\n");
        }

        public Simulator()
        {
            myCabControlSystem = new MyCabControlSystem(noOfCars, MaxnoOfStops);
        }
        [TestMethod]
        public void PlaceOverlappingRequestInLeftDirection()
        {
            myCabControlSystem.SetState(0, 1, 10);// set state of cabs
            myCabControlSystem.SetState(1, 3, 8);
            printStatus(myCabControlSystem.status()); // get status
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 4, 6));
            printStatus(myCabControlSystem.status()); // check assignment cab1 should assigned
            myCabControlSystem.Step();
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
        }

        [TestMethod]
        public void PlaceOverlappingInRightDirection()
        {
            myCabControlSystem.SetState(0, 10, 2);// set state of cabs
            myCabControlSystem.SetState(1, 7, 4);
            printStatus(myCabControlSystem.status()); // get status
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 6, 5));
            printStatus(myCabControlSystem.status()); // check assignment cab1 should assigned
            myCabControlSystem.Step();
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
        }

        /// <summary>
        /// should assign request which is nearest and onboard multiple passenger
        /// and deboard multiple passenger 
        /// </summary>
        [TestMethod]
        public void OnBoardMultiplePassengerOfSameDirection()
        {
            myCabControlSystem.SetState(0, 1, 10);// set state of cabs
            myCabControlSystem.SetState(1, 3, 8);
            printStatus(myCabControlSystem.status()); // get status
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 2, 6));
            myCabControlSystem.RaisePickupRequest(new Passenger(2, 4, 7));
            myCabControlSystem.RaisePickupRequest(new Passenger(3, 4, 7));
            printStatus(myCabControlSystem.status()); // check assignment cab0 ->P1 and cab1 -> p2 with count of 3 passenger
            myCabControlSystem.Step();
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
        }

        /// <summary>
        /// should assign request which is nearest and onboard multiple passenger
        /// and onbaord other passenger if cab get request on d way 
        /// </summary>
        [TestMethod]
        public void OnBoardRequestOnDwayInSameDirection()
        {
            myCabControlSystem.SetState(0, 1, 10);// set state of cabs
            myCabControlSystem.SetState(1, 3, 8);
            printStatus(myCabControlSystem.status()); // get status
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 2, 6));
            myCabControlSystem.RaisePickupRequest(new Passenger(2, 4, 7));
            myCabControlSystem.RaisePickupRequest(new Passenger(3, 4, 7));
            printStatus(myCabControlSystem.status()); // check assignment cab0 ->P1 and cab1 -> p2 ,p3 with count of 2 passenger
            myCabControlSystem.Step();
            myCabControlSystem.Step();
            myCabControlSystem.RaisePickupRequest(new Passenger(4, 6, 9)); // request should be added in queue of cab1->p4
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
           
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
        }

        /// <summary>
        /// should assign request which is nearest and onboard multiple passenger
        /// and onbaord other passenger if cab get request on d way 
        /// </summary>
        [TestMethod]
        public void OnBoardAfterDeboardingfromSamePosition()
        {
            myCabControlSystem.SetState(0, 1, 10);// set state of cabs
            myCabControlSystem.SetState(1, 3, 8);
            printStatus(myCabControlSystem.status()); // get status
            myCabControlSystem.RaisePickupRequest(new Passenger(1, 2, 6));
            myCabControlSystem.RaisePickupRequest(new Passenger(2, 4, 7));
            myCabControlSystem.RaisePickupRequest(new Passenger(3, 4, 7));
            printStatus(myCabControlSystem.status()); // check assignment cab0 ->P1 and cab1 -> p2 ,p3 with count of 2 passenger
            myCabControlSystem.Step();
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            myCabControlSystem.RaisePickupRequest(new Passenger(4, 7, 10)); // request should be added in queue of cab1->p4
            printStatus(myCabControlSystem.status());
           
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            myCabControlSystem.Step();
            printStatus(myCabControlSystem.status());
            
        }
    }
}
