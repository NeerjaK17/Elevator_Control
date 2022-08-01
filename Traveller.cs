using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator_Control
{
    public class Traveller
    {
        public int PersonId { get; private set; }
        public int OriginatingFloor { get; private set; }
        public int DestinationFloor { get; private set; }
        public int CompletionTime { get; set; }
        public int WaitingTime { get; set; }


        public Traveller(int ID, int originatingFloor, int destinationFloor)
        {
            OriginatingFloor = originatingFloor;
            DestinationFloor = destinationFloor;
            PersonId = ID ; 
            CompletionTime = 0;
            WaitingTime = 0;
        }



        public Direction Direction
        {
            get { return OriginatingFloor < DestinationFloor ? Direction.UP : Direction.DOWN; }
        }
    }
}
