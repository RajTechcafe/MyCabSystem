using MyCabSystem.Helper;
using MyCabSystem.MyCabControlSystem;
using System;
using System.Collections.Generic;
using System.Text;

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
        private bool cabGoingLeft, reqGoingLeft, idle;

        public Cab(int id)
        {
            this.id = id;
            this.curStop = 0;
            this.destStop = 0;
            this.idle = true;
            this.cabGoingLeft = true;
            this.reqGoingLeft = true;
            this.destinations = new PriorityQueue<int>();
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
            destinations.Clear();
            idle = curStop == destStop ? true : false;
            cabGoingLeft = curStop > destStop ? false : true;
            reqGoingLeft = cabGoingLeft;
        }

        public void addPickupReq(PickupRequest req)
        {
            if (curStop == req.getStop())
            {
                if (destinations.Count == 0) idle = true;
                return;
            }
            idle = false;
            reqGoingLeft = req.isGoingLeft();
            cabGoingLeft = (curStop > req.getStop()) ? false : true;
            destinations = !(reqGoingLeft) ? new PriorityQueue<int>(true) : new PriorityQueue<int>();
            destinations.Enqueue(req.getStop());
            destStop = destinations.Peek();
        }

        // Cab driver sets dest after entering
        // bubbleUp requests
        public void addDestStop(int stop)
        {
            if (curStop == stop)
            {
                return;
            }
            if ((isReqGoingLeft() && stop < curStop) || (!isReqGoingLeft() && stop > curStop))
            {
                throw new ArgumentOutOfRangeException("cab was summoned to serve a request which was in other direction");
            }
            if (isReqGoingLeft() != isCabGoingLeft() && curStop != destStop)
            {
                throw new ArgumentOutOfRangeException("Cab hasn't reached the destination");
            }
            Console.WriteLine("New destination added for CabId: " + id + " new dest: " + stop + " old dest: " + destStop);
            if (destinations.Count != 0)
            {
                destStop = destinations.Peek();

                if (destinations.Contains(stop)) return;
            }
            else
            {
                destinations = (curStop > stop) ? new PriorityQueue<int>(true) : new PriorityQueue<int>();
            }
            idle = false;
            destinations.Enqueue(stop);
            destStop = destinations.Peek();
            cabGoingLeft = (curStop > destStop) ? false : true;
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

        public void move()
        {
            if (curStop == destStop) idle = true;
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
                if(destinations.Count!=0)
                    // remove top element from queue
                    destinations.Dequeue();
                if (destinations.Count != 0)
                    {
                       
                        destStop = destinations.Peek();
                }
            }
        }
    }
}

