**Problem Statement** : There is a lane with finite number of CABS and STOPS, Where passenger can book CAB from SOURCE to DESTINATION on that lane.Implement a system where passenger waiting time should be least with constraint that CAB can go either in Left direction or direction on that lane.

**Constraints** : 

1. CAB can go in either left or right in same lane.
2. CAB has limited capacity.

 **Assumption** : 
 
    * if Cab request is from (S<D) i.e Left direction or (S>D) i.e. Right direction.
    
 **Actions** :  
 
 1. Passenger can place request for cab.
 2. MyCabController should assign the nearest cab for placed request.[Min waiting time for passenger]
 3. For simulation cab should move.
 4. Passenger boarding.
 5. Passenger deboarding.
 6. Capcity of cab should be check while assigning cab.
 7.Status of cab with passenger information.
 
 **Simulation Steps** :
 
 1. Intialize the number of cabs and number of stops for "MyCabSYstem".
 2. Place request by sending SOURCE, DESTINATION and PID.
 3. Continous check status of cab to track.
 4. Move cab by one unit. (For simplicity moving all cab by one unit).
 
 **Interface** :
 ```C#
 List<List<int>> status();
        void SetState(int id, int curStop, int destStop); // To set state of cab
        void setDest(int id, Passenger p); // to resest destination of passenger
        void RaisePickupRequest(Passenger p); // to place pickup request
        void Step(); // move all cab by one unit
   ``` 
   **Approach** :
   
    Cab Assignment approach :

   | Cab Direction| Request Direction |  Conclusion                                  |
   | ----------------|:------------------:|--------------------------------------------:|
   | <--(left)       | <---(left)         | Pickuprequest in range of moving cab and moving in requested direction, Insert                                                       pickuprequest to destination (MIN-HEAP) queue to maintain destination and find nearest cab                                                from all cab overlapping to pickup request.
   |  --->(right)    |  --->(right)       | Pickuprequest in range of moving cab and moving in requested direction, Insert                                                       pickuprequest to destination (MAX-HEAP) queue to maintain destination and find nearest cab                                               from all cab overlapping to pickup request.
   |  <---(left)    |  --->(right)        | Overlapping of request and cab is not possible,Insert pickuprequest to destination (MAX-                                                 HEAP) queue
   | --->(right)    |  <---(left)         | Overlapping of request and cab is not possible, Insert pickuprequest to destination (MIN-                                                 HEAP) queue.
    
       
