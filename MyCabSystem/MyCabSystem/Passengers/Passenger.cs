using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCabSystem.Passengers
{
    public class Passenger
    {
        private int pId;
        private int source, destination;
        private bool goingLeft;
        private bool passengerBoarded;
        public Passenger(int pId, int source, int destination)
        {
            this.pId = pId;
            this.source = source;
            this.destination = destination;
            this.goingLeft = getDirection(source, destination);
            this.passengerBoarded = false;
        }

        private bool getDirection(int stop, int destinationStop)
        {
            if (stop <= destinationStop)
                return true;
            else
                return false;
        }
        public int getID()
        {
            return pId;
        }
        public bool setPassengerBoarded(bool value)
        {
            this.passengerBoarded = value;
            return value;
        }

        public int getSource()
        {
            return source;
        }
        public int getDestination()
        {
            return destination;
        }

        public bool isGoingLeft()
        {
            return goingLeft;
        }
        public bool isPassengerBoarded()
        {
            return passengerBoarded;
        }

        public int getStop()
        {
            if (isPassengerBoarded())
                return getDestination();
            else
                return getSource();
        }

       
    }
}
