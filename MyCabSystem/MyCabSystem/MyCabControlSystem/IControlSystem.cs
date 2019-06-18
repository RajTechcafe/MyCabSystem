using System;
using System.Collections.Generic;
using System.Text;

 namespace MyCabSystem.MyCabControlSystem
{
    public interface IControlSystem
    {
       
          List<List<int>> status();

             void setState(int id, int curStop, int destStop);

             void setDest(int id, int destStop);

             void pickup(int stop, int direction);

             void step();
        
    }
}
