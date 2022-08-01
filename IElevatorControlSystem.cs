using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator_Control
{
    public interface IElevatorControlSystem
    {
        //Elevator GetStatus(int elevatorId);
        //void Update(int elevatorId, int floorNumber, int goalFloorNumber);
        //void Pickup(int pickupFloor, int destinationFloor);
        //void Step();
        void MoveElevatorUp(bool skip);
        void MoveElevatorDown(bool skip);
        void MoveElevatorRightDirection(bool skip);
        List<Traveller> UnloadElevator();
        bool AnyOutstandingPickups();
    }
}
