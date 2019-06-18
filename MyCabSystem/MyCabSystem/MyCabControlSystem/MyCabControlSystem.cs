using MyCabSystem.MyCabSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCabSystem.MyCabControlSystem
{

    public class MyCabControlSystem : IControlSystem
    {


        private List<Cab> cabs = new List<Cab>();
        // FIFO req queue
        private List<PickupRequest> requests = new List<PickupRequest>();

        private  int noOfcabs, noOfStops;

        void checkstopValidity(int stopVal)
        {
            if (stopVal < 0 || stopVal >= noOfStops)
                throw new ArgumentOutOfRangeException("Invalid stop value");
        }

        void checkcabValidity(int cabVal)
        {
            if (cabVal < 0 || cabVal >= noOfcabs)
                throw new ArgumentOutOfRangeException("Invalid cab ID");
        }

        public MyCabControlSystem(int noOfcabs, int noOfStops)
        {
            this.noOfcabs = noOfcabs;
            this.noOfStops = noOfStops;
            for (int i = 0; i < noOfcabs; i++)
            {
                Cab cab = new Cab(i);
                cabs.Add(cab);
            }
        }

        
public List<List<int>> status()
        {
            List<List<int>> statuses = new List<List<int>>();
            foreach (var cab in cabs)
            {
                List<int> cabStatus = new List<int>();
                cabStatus.Add(cab.getId()); cabStatus.Add(cab.getCurStop()); cabStatus.Add(cab.getDestStop());
                statuses.Add(cabStatus);
            }
            return statuses;
        }

        
       
public void setState(int id, int curstop, int deststop)
        {
            checkcabValidity(id);
            checkstopValidity(curstop);
            checkstopValidity(deststop);
            foreach (var cab in cabs)
            {
                if (cab.getId() == id)
                {
                    cab.setState(curstop, deststop);
                }
            }
        }

        // Used to set direction of an cab once the user enters
  
public void setDest(int id, int deststop)
        {
            checkcabValidity(id);
            checkstopValidity(deststop);
            foreach (var cab in cabs)
            {
                if (cab.getId() == id)
                {
                    cab.addDestStop(deststop);
                   Console.WriteLine("Setting dest cabId: " + id + " stop: " + deststop);
                }
            }
        }

    
public void pickup(int stop, int destination)
        {
            checkstopValidity(stop);
            PickupRequest pickupReq = new PickupRequest(stop, destination, noOfStops);
            Cab cab = findbubbleUpcab(pickupReq);
            if (cab != null)
            {
                cab.addDestStop(pickupReq.getStop());
                Console.WriteLine("\bubbleUp possible for stop: " + pickupReq.getStop() + " isGoingLeft: " + pickupReq.isGoingLeft() + " added to cabID: " + cab.getId());
            }
            else
            {
                requests.Add(pickupReq);
                Console.WriteLine("\nAdding to FIFO queue. bubbleUp NOT possible for stop: " + pickupReq.getStop() + " isGoingLeft: " + pickupReq.isGoingLeft());
            }
        }

        private Cab findbubbleUpcab(PickupRequest pickupReq)
        {
            Cab result = null;
            int closest = int.MaxValue;
            foreach (var cab in cabs)
            {
                if (cab.isCabGoingLeft() == pickupReq.isGoingLeft() && cab.isReqGoingLeft() == pickupReq.isGoingLeft())
                {
                    if (cab.isstopInBetween(pickupReq.getStop()))
                    {
                        if (cab.getDistTostop(pickupReq.getStop()) < closest)
                        {
                            result = cab;
                        }
                    }
                }
            }
            return result;
        }

public void step()
        {
            // Move all cabs by one step and update idle logic
            foreach (var cab in cabs)
            {
                cab.move();
            }

            List<int> processed = new List<int>();
            // Process FIFO queue
            for (int i = 0; i < requests.Count; i++)
            {
                PickupRequest req = requests[i];
                Cab closestcab = findIdlecab(req.getStop());
                if (closestcab != null)
                {
                    closestcab.addPickupReq(req);
                    Console.WriteLine("\nFIFO req: " + req.getStop() + " isGoingLeft: " + req.isGoingLeft() + " assigned to cabID: " + closestcab.getId());
                    processed.Add(i);
                }
                else
                {
                    closestcab = findbubbleUpcab(req);
                    if (closestcab != null)
                    {
                        closestcab.addDestStop(req.getStop());
                        Console.WriteLine("\nbubbleUp possible for stop: " + req.getStop() + " isGoingLeft: " + req.isGoingLeft() + " added to cabID: " + closestcab.getId());
                        processed.Add(i);
                    }
                }
            }

            // Remove all processed requests
            for (int i = processed.Count - 1; i >= 0; i--)
            {
                requests.Remove(requests[i]);
            }
        }

        private Cab findIdlecab(int deststop)
        {
            int closestDist = int.MaxValue;
            Cab closestcab = null;
            foreach (var cab in cabs)
            {
                if (cab.isIdle())
                {
                    if (cab.getDistTostop(deststop) < closestDist)
                    {
                        closestcab = cab;
                    }
                }
            }
            return closestcab;
        }

    }
}

