using System;
using System.Collections.Generic;
using System.Text;

namespace MyCabSystem.MyCabControlSystem
{
    public class PickupRequest
    {

        private  bool goingLeft;
            private  int stop, maxStop;
        public PickupRequest(int stop, int destinationStop, int maxStop)
        {
            this.stop = stop;
            this.goingLeft = getDirection(stop, destinationStop); //(direction >= 0) ? true : false;
            this.maxStop = maxStop;

            checkValidity();
        }
        private bool getDirection(int stop, int destinationStop)
        {
            if (stop <= destinationStop)
                return true;
            else
                return false;
        }
        void checkValidity()
        {
            if ((stop == maxStop - 1 && isGoingLeft()) || (stop == 0 && !isGoingLeft()))
            {
                throw new ArgumentOutOfRangeException("Invalid pick up request");
            }
        }

        

        public int getStop()
        {
            return stop;
        }

       
	
	public bool isGoingLeft()
        {
            return goingLeft;
        }
    }
}
