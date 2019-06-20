using MyCabSystem.Helper;
using MyCabSystem.MyCabControlSystem;
using System;
using System.Collections.Generic;
using System.Text;
using MyCabSystem.Passengers;
using System.Linq;

namespace MyCabSystem.MyCabSystem
{
    class MaxHeapComparator : IComparer<int>
    {

        public int Compare(int x, int y)
        {
            return y - x;
        }
    }

    public class Cab
    {

        private int id;
        private int curStop, destStop;
       private PriorityQueue<int> destinations;
        //private PriorityQueue<int> destinationsQueue;
       // private PriorityQueue<int> pickupQueue;
        private bool cabGoingLeft, reqGoingLeft, idle;
        private int passengerCount;
        private int maxCabCapacity;
        private List<Passenger> passengerBoarderd = new List<Passenger>();
        public Cab(int id)
        {
            this.id = id;
            this.curStop = 0;
            this.destStop = 0;
            this.idle = true;
            this.cabGoingLeft = true;
            this.reqGoingLeft = true;
            this.passengerCount = 0;
            this.maxCabCapacity = 3;
            this.destinations = new PriorityQueue<int>();
           // this.destinationsQueue = new PriorityQueue<int>();
           // this.pickupQueue = new PriorityQueue<int>();
        }

        public PriorityQueue<int> getDestination()
        {
            return this.destinations;
        }
        public int GetcabCapacity()
        {
            return this.passengerCount;
        }
        public int getMaxCapacity()
        {
            return this.maxCabCapacity;
        }
        public bool isCabFull()
        {
            if (GetcabCapacity() ==this.maxCabCapacity)
                return true;
            else
                return false;
        }

        public string OnBoardPassenger()
        {
            if (GetcabCapacity() == this.maxCabCapacity)
                return "Cab is full no empty space";
            else
            {
                this.passengerCount++;
                return GetcabCapacity().ToString();

            }
                

        }

        public string DeBoardPassenger()
        {
            if (GetcabCapacity() == 0)
                return "Cab is empty";
            else
            {
                this.passengerCount--;
                return GetcabCapacity().ToString();

            }


        }
        public int getCurStop()
        {
            return curStop;
        }
        public int getDestStop()
        {
            return destStop;
        }

        public bool isCabGoingLeft()
        {
            return cabGoingLeft;
        }

        public bool isReqGoingLeft()
        {
            return reqGoingLeft;
        }

        public bool isIdle()
        {
            return idle;
        }

        public int getId()
        {
            return id;
        }

        /**
         * Resets the state of the cab
         * @param curStop
         * @param destStop
         */
        public void setState(int curStop, int destStop)
        {
            this.curStop = curStop;
            this.destStop = destStop;
            //destinations.Clear();
            destinations.Clear();
           // pickupQueue.Clear();
            passengerBoarderd.Clear();
            idle = curStop == destStop ? true : false;
            cabGoingLeft = curStop > destStop ? false : true;
            reqGoingLeft = cabGoingLeft;
        }

        //public void addPickupReq(PickupRequest req)
        //{
        //    if (curStop == req.getStop())
        //    {
        //        if (destinations.Count == 0) idle = true;
        //        return;
        //    }
        //    idle = false;
        //    reqGoingLeft = req.isGoingLeft();
        //    cabGoingLeft = (curStop > req.getStop()) ? false : true;
        //    destinations = !(reqGoingLeft) ? new PriorityQueue<int>(true) : new PriorityQueue<int>();
        //    destinations.Enqueue(req.getStop());
        //    destStop = destinations.Peek();
        //}

       
        public void addPickupReq(Passenger pReq,bool dest)
        {
            int stop = dest == true ? pReq.getDestination() : pReq.getSource();
            if (curStop == stop)
            {
                if (destinations.Count == 0) idle = true;
                return;
            }
            if (!dest)
            {
                idle = false;
                reqGoingLeft = pReq.isGoingLeft();
                cabGoingLeft = (curStop > stop) ? false : true;
            }
         
           
            if (destinations.Count != 0)
            {
                if (reqGoingLeft)
                    destinations.Sort(false);// get min-Heap
                else
                    destinations.Sort(true); // get max heap

            }
            else
            {
                destinations = !(reqGoingLeft) ? new PriorityQueue<int>(true) : new PriorityQueue<int>();
            }
            destinations.Enqueue(stop);
            destStop = destinations.Peek();
        }
       
        public void addToDestOfCab(Passenger passengerPickupRequest,bool dest)
        {
            int stop = dest == true ? passengerPickupRequest.getDestination() : passengerPickupRequest.getSource();
            if (isCabFull()) // Just safety check
            {
                Console.WriteLine("\n cannot board passenger");
                return;
            }
            if (curStop == stop || passengerPickupRequest.isPassengerBoarded())
                return; // passenger is already boarded in some other car or same car
            if ((isReqGoingLeft() && stop < curStop) || (!isReqGoingLeft() && stop > curStop))
            {
                throw new ArgumentOutOfRangeException("cab was summoned to serve a request which was in other direction");
            }
            if (isReqGoingLeft() != isCabGoingLeft() && curStop != destStop)
            {
                throw new ArgumentOutOfRangeException("Cab hasn't reached the destination");
            }
           
                 //Console.WriteLine("Passenger" + passengerPickupRequest.getID() + "is baorded to cab "+id + " new dest: " + passengerPickupRequest.getSource() + " old dest: " + destStop);
              //  passengerBoarderd.Add(passengerPickupRequest);
               // cabCapacity++;
                Console.WriteLine("New destination added for CabId: " + id + " new dest: " + stop + " old dest: " + destStop);
            if (destinations.Count != 0)
            {
               
                if (destinations.Contains(stop)) return;
                if (curStop > stop)
                    destinations.Sort(true);// get min-Heap
                else
                    destinations.Sort(false); // get max heap
            }
            else
            {
                destinations = (curStop > stop) ? new PriorityQueue<int>(true) : new PriorityQueue<int>();
            }
            if (!dest) // set the direction only for pickup request
            {
                idle = false;

                cabGoingLeft = (curStop > destStop) ? false : true;
            }
            destinations.Enqueue(stop);
            destStop = destinations.Peek();


        }
     



        // Tells if a stop in between curStop and destStop
        public bool isstopInBetween(int stop)
        {
            return (curStop <= stop && stop <= destStop) || (destStop <= stop && stop <= curStop);
        }

        // Gives absolute distance between cab current stop and the given stop
        public int getDistTostop(int destStop)
        {
            return Math.Abs(this.curStop - destStop);
        }

        public void move(List<Passenger> requestAllocatedToCabs)
        {
            if (curStop == destStop) {
                idle = true;
            }
            if (curStop < destStop)
            {
                curStop = curStop + 1;
            }
            else if (curStop > destStop)
            {
                curStop = curStop - 1;
            }
            if (curStop == destStop)
            {
                
                if (destinations.Count != 0)
                {
                    int dest = destinations.Dequeue();  // remove top element from queue
                    if (destinations.Count != 0)
                    {
                        destStop = destinations.Peek();
                    }
                    var boardingLst = requestAllocatedToCabs.FindAll(x => x.getSource() == dest && x.isPassengerBoarded() ==false);
                    var deboardingLst = requestAllocatedToCabs.FindAll(y => y.getDestination() == dest && y.isPassengerBoarded() == true);
                    if (boardingLst.Count != 0)
                    {
                        foreach( var item in boardingLst) // Board Passenger
                        {
                            if(this.passengerCount != this.maxCabCapacity)
                            {
                                string str = OnBoardPassenger();// change capacity 
                                Console.WriteLine("Passenger "+item.getID()+" is boarderd to cab"+this.id+" where p is travelling "+item.getSource()+" to "+ item.getDestination()+ "with "+this.passengerCount+" passenger");
                                item.setPassengerBoarded(true);
                                passengerBoarderd.Add(item);
                            }
                            else
                            {
                                Console.WriteLine("Cab " + this.id + "Is running full");
                            }
                           
                        }
                    }
                    if(deboardingLst.Count!=0) // Deboard Passenger
                    {
                        foreach (var item in deboardingLst)
                        {
                            if (this.passengerCount > 0){
                                string str = DeBoardPassenger(); // change capacity 
                                Console.WriteLine("Passenger " + item.getID() + " Deboarderd from cab " + this.id + "where p was travelling " + item.getSource() + " to " + item.getDestination() + "with " + this.passengerCount + "passengers");
                                item.setPassengerBoarded(false);
                                passengerBoarderd.Remove(item);
                                requestAllocatedToCabs.Remove(item); //remove from global as well
                            }
                            else
                            {
                                Console.WriteLine("Cab " + this.id + "Is empty");
                            }
                        }
                          
                    }
                }
                  
                
                if (destinations.Count != 0)
                    {
                       
                        destStop = destinations.Peek();
                }
            }
        }

        
    }
}

