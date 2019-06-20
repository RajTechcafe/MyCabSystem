using MyCabSystem.MyCabSystem;
using MyCabSystem.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCabSystem.MyCabControlSystem
{

    public class MyCabControlSystem : IControlSystem
    {


        private List<Cab> cabs = new List<Cab>();
        // FIFO req queue
        // private Queue<PickupRequest> requests = new Queue<PickupRequest>();
        private Queue<Passenger> pRequest = new Queue<Passenger>();
        List<Passenger> requestAllocatedToCabs = new List<Passenger>();
        private int noOfcabs, noOfStops;

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
                cabStatus.Add(cab.GetcabCapacity());
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

        public void setDest(int id, Passenger pickupRequest)
        {
            checkcabValidity(id);
            checkstopValidity(pickupRequest.getSource());
            foreach (var cab in cabs)
            {
                if (cab.getId() == id)
                {
                    cab.addToDestOfCab(pickupRequest, false);
                    Console.WriteLine("Setting dest cabId: " + id + " stop: " + pickupRequest.getSource());
                }
            }
        }



        public void RaisePickupRequest(Passenger passengerPickupRequest)
        {
            pRequest.Enqueue(passengerPickupRequest);
            SearchForCab();
        }
        private void SearchForCab()
        {
            if (pRequest.Count == 0)
                Console.Write("\n There is no request for Cab, Please raise request for cab");
            else
            {

                Passenger pickupRequest = pRequest.Dequeue();

                checkstopValidity(pickupRequest.getSource()); checkstopValidity(pickupRequest.getDestination());

                if (cabs.Where(x => x.isCabFull() == true).Count() == cabs.Count)
                {
                    Console.WriteLine("\n All cabs is running full ");
                    return;
                }
                // search for nearest cab to pick you(Source) in seats available cab and which are in ur way
                Cab cab = findRequestOverlappingCab(pickupRequest);
                if (cab != null)
                {
                    if (cab.getDestination().Count <= 2 * (cab.getMaxCapacity()))
                    {
                        requestAllocatedToCabs.Add(pickupRequest);
                        cab.addToDestOfCab(pickupRequest, false);// request for adding source of p to destqueue
                        cab.addToDestOfCab(pickupRequest, true); //request for adding dest of p to destqueue
                        Console.WriteLine("\n Overlap is possible for stop: " + pickupRequest.getSource() + " to " + pickupRequest.getDestination() + " isGoingLeft: " + pickupRequest.isGoingLeft() + " added to cabID: " + cab.getId());
                    }
                    else
                    {
                        pRequest.Enqueue(pickupRequest);
                        Console.WriteLine("\nAdding to FIFO queue. : " + pickupRequest.getSource() + " to " + pickupRequest.getDestination() + " isGoingLeft: " + pickupRequest.isGoingLeft());
                    } 
                }
                else
                {
                    pRequest.Enqueue(pickupRequest);
                    Console.WriteLine("\nAdding to FIFO queue. Overlap is NOT possible for stop: " + pickupRequest.getSource() + " to " + pickupRequest.getDestination() + " isGoingLeft: " + pickupRequest.isGoingLeft());
                }

            }
        }



        private Cab findRequestOverlappingCab(Passenger pickupReq)
        {
            Cab result = null;
            int closest = int.MaxValue;
            List<Cab> seatAvailableCab = cabs.Where(x => x.isCabFull() != true).ToList(); // filter seat available Cabs
            foreach (var cab in seatAvailableCab)
            {
                if (cab.isCabGoingLeft() == pickupReq.isGoingLeft() && cab.isReqGoingLeft() == pickupReq.isGoingLeft())
                {
                    if (cab.isstopInBetween(pickupReq.getSource()))
                    {
                        if (cab.getDistTostop(pickupReq.getSource()) < closest)
                        {
                            result = cab;
                            closest = cab.getDistTostop(pickupReq.getSource());
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

                cab.move(requestAllocatedToCabs);

            }

            Queue<int> processed = new Queue<int>();
            Queue<Passenger> unProccessedRequests = new Queue<Passenger>();
            // unProccessedRequests = requests
            // Process FIFO queue

            while (pRequest.Count != 0)
            {

                Passenger req = pRequest.Dequeue();
                unProccessedRequests.Enqueue(req);
                if (cabs.Where(x => x.isCabFull() == true).Count() == cabs.Count)
                {
                    Console.WriteLine("\n All cabs is running full ");
                    return;
                }
                Cab closestcab = findIdlecab(req.getSource());
                if (closestcab != null)
                {
                    if (closestcab.getDestination().Count <= 2*(closestcab.getMaxCapacity()))
                    {
                        closestcab.addPickupReq(req, false);
                        closestcab.addPickupReq(req, true);
                        Console.WriteLine("\n req: " + req.getSource() + " to " + req.getDestination() + " isGoingLeft: " + req.isGoingLeft() + " assigned to cabID: " + closestcab.getId());
                        unProccessedRequests.Dequeue();
                    }
                }
                else
                {
                    closestcab = findRequestOverlappingCab(req);
                    if (closestcab != null)
                    {
                        if (closestcab.getDestination().Count <= 2 * (closestcab.getMaxCapacity()))
                        {
                            requestAllocatedToCabs.Add(req);
                            closestcab.addToDestOfCab(req, false);// request for adding source of p to destqueue
                            closestcab.addToDestOfCab(req, true);
                            Console.WriteLine("\n Overlap is possible for stop: " + req.getSource() + " to " + req.getDestination() + " isGoingLeft: " + req.isGoingLeft() + " added to cabID: " + closestcab.getId());
                            unProccessedRequests.Dequeue();
                        }
                            
                    }
                }
            }
            pRequest = unProccessedRequests;

        }

        private Cab findIdlecab(int deststop)
        {
            int closestDist = int.MaxValue;
            Cab closestcab = null;
            List<Cab> seatAvailableCab = cabs.Where(x => x.isCabFull() != true).ToList(); // filter seat available Cabs
            foreach (var cab in seatAvailableCab)
            {
                if (cab.isIdle())
                {
                    if (cab.getDistTostop(deststop) < closestDist)
                    {
                        closestcab = cab;
                        closestDist = cab.getDistTostop(deststop);
                    }
                }
            }
            return closestcab;
        }

    }
}

