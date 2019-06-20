using MyCabSystem.Passengers;
using System;
using System.Collections.Generic;
using System.Text;

 namespace MyCabSystem.MyCabControlSystem
{
    public interface IControlSystem
    {
       
          List<List<int>> status();

             void setState(int id, int curStop, int destStop);

             void setDest(int id, Passenger p);
        void RaisePickupRequest(Passenger p);
           //void pickup(int stop, int direction);
       
             void step();
        
    }
}
