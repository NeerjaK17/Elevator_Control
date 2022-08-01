using System;
using System.Collections.Generic;
using System.Text;


namespace Elevator_Control
{
    public class Elevator
    {

        public int Id { get; private set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public int Capacity { get; set; }
        // SelectedFloor is highest or lowest floor the elevator will go to.
        public int SelectedFloor { get; set; }
        public int TotalTimeTaken { get; set; }
        public int ElevatorStuck { get; set; }
        public List<Traveller> Travellers { get; set; }

        public Elevator()
        {
            //Id = id;
            Travellers = new List<Traveller>();
            CurrentFloor = 0;
            Capacity = 10;
            TotalTimeTaken = 0;
            ElevatorStuck = 0;
        }


        //Decides what direction elevator should be going. If CurrentFloor is smaller than SelectedFloor elevator should go up, if not it should go down.
        public Direction Direction
        {
            get
            {
                //return CurrentFloor == 1
                //    ? Direction.UP
                //    : DestinationFloor > CurrentFloor ? Direction.UP : Direction.DOWN;

                if (CurrentFloor == 0 || CurrentFloor < SelectedFloor)
                {
                    return Direction.UP;
                }
                else if (CurrentFloor == 9 || CurrentFloor > SelectedFloor)
                {
                    return Direction.DOWN;
                }
                else
                {
                    return Direction.IDLE;
                }
            }
        }



    }
}
