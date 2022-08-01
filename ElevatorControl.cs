using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Elevator_Control
{
    public class ElevatorControl : IElevatorControlSystem
    {
        public int count = 0;
        public static bool IsPaused = false;
        public Elevator elevator { get; set; }
        public List<Elevator> Elevators { get; set; }
        public List<Traveller> WaitingRiders { get; set; }
        public List<Traveller> FinishedRiders { get; set; }
        public int row = 0;

        public ElevatorControl(int numberOfElevators)
        {
            elevator = new Elevator();
            Elevators = Enumerable.Range(0, numberOfElevators).Select(eid => elevator).ToList();
            WaitingRiders = new List<Traveller>();
            FinishedRiders = new List<Traveller>();
        }


        //First updates elevator.SelectedFloor by getting the highest value out of all persons currently in elevator using FindHighestDestinationFloor
        //Then unloads and loads elevator using respective methods
        //Writes the current floor number, what direction elevator is moving in, and how many people are currently in elevator. Pauses the program for .5 seconds to make console readable
        //Increments current floor of elevator
        //Increments total time taken for elevator as well as calling method for incrementing time for passengers
        public void MoveElevatorUp(bool skip)
        {
            FindHighestDestinationFloor();
            UnloadElevator();
            LoadElevator();
            if (skip == false)
            {
                Console.WriteLine("Current floor is: " + elevator.CurrentFloor + ". Elevator is moving: " + elevator.Direction + ". There are currently " + elevator.Travellers.Count + " people in elevator.");
                for (int i = 0; i < 10; i++)
                {
                    foreach (Traveller p in WaitingRiders)
                    {
                        if (p.OriginatingFloor == i)
                        {
                            count++;
                        }
                    }
                    Console.WriteLine("There are currently waiting: " + count + " people in line on this floor: " + i);
                    count = 0;
                }
                System.Threading.Thread.Sleep(150);
            }
            elevator.CurrentFloor++;
            elevator.TotalTimeTaken++;
            IncrementTimeOfPassengers();
        }

        //First updates elevator.SelectedFloor by getting the lowest value out of all persons currently in elevator using FindLowestDestinationFloor
        //Then unloads and loads elevator using respective methods
        //Writes the current floor number, what direction elevator is moving in, and how many people are currently in elevator. Pauses the program for .5 seconds to make console readable
        //Decrements current floor of elevator
        //Increments total time taken for elevator as well as calling method for incrementing time for passengers
        public void MoveElevatorDown(bool skip)
        {
            FindLowestDestinationFloor();
            UnloadElevator();
            LoadElevator();
            if (skip == false)
            {
                Console.WriteLine("Current floor is: " + elevator.CurrentFloor + ". Elevator is moving: " + elevator.Direction + ". There are currently: " + elevator.Travellers.Count + " people in elevator.");
                for (int i = 0; i < 10; i++)
                {
                    foreach (Traveller p in WaitingRiders)
                    {
                        if (p.OriginatingFloor == i)
                        {
                            count++;
                        }
                    }
                    Console.WriteLine("There are currently waiting: " + count + " people in line on this floor: " + i);
                    count = 0;
                }

                System.Threading.Thread.Sleep(150);
            }
            elevator.CurrentFloor--;
            elevator.TotalTimeTaken++;
            IncrementTimeOfPassengers();
        }
        //Loops through all persons that are currently in WaitingPersons, then adds person to elevator if following conditions are met: elevator isn't full,
        //persons direction is the same as elevators direction, persons original floor is the same as elevators current floor.
        //Method also removes that person from WaitingPersons list. Finally the method returns list of people currently in elevator.
        public List<Traveller> LoadElevator()
        {
            List<Traveller> templist = new List<Traveller>(WaitingRiders);
            foreach (Traveller rider in templist)
            {
                if (elevator.Travellers.Count < elevator.Capacity && rider.Direction == elevator.Direction && rider.OriginatingFloor == elevator.CurrentFloor)
                {
                    elevator.Travellers.Add(rider);
                    WaitingRiders.Remove(rider);
                }
            }
            return elevator.Travellers;
        }

        //Loops through all persons currently in elevator and with an if statement checks if their destinationfloor is the same as current floor of elevator. 
        //if true, person is removed from the elevator passenger list.
        //Traveller is also added to list of persons FinishedPersons. Finally method returns Passengers.
        public List<Traveller> UnloadElevator()
        {
            List<Traveller> templist = new List<Traveller>(elevator.Travellers);
            foreach (Traveller person in templist)
            {
                if (person.DestinationFloor == elevator.CurrentFloor)
                {
                    elevator.Travellers.Remove(person);
                    FinishedRiders.Add(person);
                }
            }
            return elevator.Travellers;
        }

        //public Elevator GetStatus(int elevatorId)
        //{
        //    return Elevators.First(e => e.Id == elevatorId);
        //}

        //public void Update(int elevatorId, int floorNumber, int goalFloorNumber)
        //{
        //    UpdateElevator(elevatorId, e =>
        //    {
        //        e.CurrentFloor = floorNumber;
        //        e.DestinationFloor = goalFloorNumber;
        //    });
        //}

        //public void Pickup(int pickupFloor, int destinationFloor)
        //{
        //    WaitingRiders.Add(new Traveller(pickupFloor, destinationFloor));
        //}

        //private void UpdateElevator(int elevatorId, Action<Elevator> update)
        //{
        //    Elevators = Elevators.Select(e =>
        //    {
        //        if (e.Id == elevatorId) update(e);
        //        return e;
        //    }).ToList();
        //}

        //public void Step()
        //{
        //    var busyElevatorIds = new List<int>();
        //    // unload elevators
        //    Elevators = Elevators.Select(e =>
        //    {
        //        var disembarkingRiders = e.Travellers.Where(r => r.DestinationFloor == e.CurrentFloor).ToList();
        //        if (disembarkingRiders.Any())
        //        {
        //            busyElevatorIds.Add(e.Id);
        //            e.Travellers = e.Travellers.Where(r => r.DestinationFloor != e.CurrentFloor).ToList();
        //        }

        //        return e;
        //    }).ToList();

        //    // Embark passengers to available elevators
        //    WaitingRiders.GroupBy(r => new { r.OriginatingFloor, r.Direction }).ToList().ForEach(waitingFloor =>
        //    {
        //        var availableElevator =
        //            Elevators.FirstOrDefault(
        //                e =>
        //                    e.CurrentFloor == waitingFloor.Key.OriginatingFloor &&
        //                    (e.Direction == waitingFloor.Key.Direction || !e.Travellers.Any()));
        //        if (availableElevator != null)
        //        {
        //            busyElevatorIds.Add(availableElevator.Id);
        //            var embarkingPassengers = waitingFloor.ToList();
        //            UpdateElevator(availableElevator.Id, e => e.Travellers.AddRange(embarkingPassengers));
        //            WaitingRiders = WaitingRiders.Where(r => embarkingPassengers.All(er => er.Id != r.Id)).ToList();
        //        }
        //    });


        //    Elevators.ForEach(e =>
        //    {
        //        var isBusy = busyElevatorIds.Contains(e.Id);
        //        int destinationFloor;
        //        if (e.Travellers.Any())
        //        {
        //            var closestDestinationFloor =
        //                e.Travellers.OrderBy(r => Math.Abs(r.DestinationFloor - e.CurrentFloor))
        //                    .First()
        //                    .DestinationFloor;
        //            destinationFloor = closestDestinationFloor;
        //        }
        //        else if (e.DestinationFloor == e.CurrentFloor && WaitingRiders.Any())
        //        {
        //            destinationFloor = WaitingRiders.GroupBy(r => new { r.OriginatingFloor }).OrderBy(g => g.Count()).First().Key.OriginatingFloor;
        //        }
        //        else
        //        {
        //            destinationFloor = e.DestinationFloor;
        //        }

        //        var floorNumber = isBusy
        //            ? e.CurrentFloor
        //            : e.CurrentFloor + (destinationFloor > e.CurrentFloor ? 1 : -1);

        //        Update(e.Id, floorNumber, destinationFloor);
        //    });
        //}

        //checks if there are any persons currently waiting for elevator or if there are any persons currently in elevator.
        public bool AnyOutstandingPickups()
        {
            return (WaitingRiders.Any() || elevator.Travellers.Any());
        }

        //RunProgram() method makes it possible to see wether the program is paused or wether it is running again.
        public static bool RunProgram()
        {
            if (!Console.KeyAvailable)
            {

            }
            else if (Console.ReadKey(true).Key == ConsoleKey.P)
            {


                if (IsPaused == false)
                {
                    Console.WriteLine("The program is paused.");
                    Console.WriteLine("Press 'P again to resume'.");
                    IsPaused = true;
                }
                else
                {
                    Console.WriteLine("The program is now running");
                    IsPaused = false;
                }
            }
            return IsPaused;
        }


        public void MoveElevatorRightDirection(bool skip)
        {
            elevator.ElevatorStuck++;
            UnloadElevator();
            LoadElevator();
            FindLowestDestinationFloor();

            do
            {

                MoveElevatorUp(skip);
                elevator.ElevatorStuck = 0;
                if (skip == false)
                {
                    Thread.Sleep(500);
                }
                Console.Clear();


                RunProgram();

            } while (IsPaused == false && AnyOutstandingPickups() == true && elevator.CurrentFloor < elevator.SelectedFloor);


            while (RunProgram() == true && AnyOutstandingPickups())
            {
                RunProgram();
            }



            if (elevator.CurrentFloor == 9)
            {
                FindLowestDestinationFloor();
            }

            if (elevator.Travellers.Count == 0)
            {
                foreach (Traveller traveller in WaitingRiders)
                {
                    elevator.SelectedFloor = Math.Min(elevator.SelectedFloor, traveller.OriginatingFloor);
                }
            }

            do
            {

                MoveElevatorDown(skip);
                elevator.ElevatorStuck = 0;
                if (skip == false)
                {
                    Thread.Sleep(500);
                }
                Console.Clear();


                RunProgram();
            } while (IsPaused == false && AnyOutstandingPickups() == true && elevator.CurrentFloor > elevator.SelectedFloor);

            while (RunProgram() == true && AnyOutstandingPickups())
            {
                RunProgram();
            }


            UnloadElevator();
            LoadElevator();
            FindHighestDestinationFloor();

            if (elevator.CurrentFloor == 0)
            {
                FindHighestDestinationFloor();
            }



            if (elevator.Travellers.Count == 0)
            {
                foreach (Traveller traveller in WaitingRiders)
                {
                    elevator.SelectedFloor = Math.Max(elevator.SelectedFloor, traveller.OriginatingFloor);
                }
            }

            if (elevator.ElevatorStuck == 2)
            {
                if (elevator.Direction.ToString() == "UP")
                {
                    elevator.SelectedFloor--;
                }
                else
                {
                    elevator.SelectedFloor++;
                }
            }

        }


        //Method for incrementing time of both people currently in elevator and people that are currently waiting on elevator
        public void IncrementTimeOfPassengers()
        {
            foreach (Traveller traveller in elevator.Travellers)
            {
                traveller.CompletionTime++;
            }
            foreach (Traveller person in WaitingRiders)
            {
                person.CompletionTime++;
                person.WaitingTime++;
            }
        }

        //Method for identifying lowest destinationfloor in elevator
        public void FindLowestDestinationFloor()
        {
            foreach (Traveller traveller in elevator.Travellers)
            {
                elevator.SelectedFloor = Math.Min(elevator.SelectedFloor, traveller.DestinationFloor);
            }
        }

        //Method for identifying highest destinationfloor in elevator
        public void FindHighestDestinationFloor()
        {
            foreach (Traveller traveller in elevator.Travellers)
            {
                elevator.SelectedFloor = Math.Max(elevator.SelectedFloor, traveller.DestinationFloor);
            }
        }

    }
}
