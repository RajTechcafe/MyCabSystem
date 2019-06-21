using MyCabSystem.Passengers;
using System;
using System.Collections.Generic;
using System.Text;

 namespace MyCabSystem.MyCabControlSystem
{
    public interface IControlSystem
    {
        List<List<int>> status();
        void SetState(int id, int curStop, int destStop);
        void SetDest(int id, Passenger p);
        void RaisePickupRequest(Passenger p);
        void Step();

    }
}
