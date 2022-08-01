using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Elevator_Control
{
    public class Program
    {
        public static bool skip = false;
        public static void Main(string[] args)
        {
            const int numberOfFloors = 10;
            const int numberOfElevators = 10;
            const int numberOfRequests = 10 * 100;

            // Constant
            var pickupCount = 0;
            var stepCount = 0;
            var random = new Random();
            IElevatorControlSystem system = new ElevatorControl(numberOfElevators);


            Console.WriteLine("Press any key to start elevator.");
            Console.WriteLine("Press 'P' to pause/play");
            Console.WriteLine("Press 'S' to skip");
            Console.WriteLine("--------------------------------------------------------------------");
            string input = Console.ReadLine();
            if (input == "s" || input == "S")
            {
                skip = true;
            }
            Console.WriteLine();

            int row = 0;
            ElevatorControl controlpanel = new ElevatorControl(numberOfElevators);
            TravellerInfo(controlpanel.WaitingRiders, row);

            while (system.AnyOutstandingPickups())
            {
                system.MoveElevatorRightDirection(skip);
                row += 10;
                TravellerInfo(controlpanel.WaitingRiders, row);
            }





            int TotalTime = controlpanel.elevator.TotalTimeTaken;
            float AverageWaitingTime = 0;
            float AverageCompletionTime = 0;
            int LeastTime = 10;
            int MostTime = 0;
            int NumberOfPersonsWithMostTime = 0;
            int NumberOfPersonsWithLeastTime = 0;
            foreach (Traveller person in controlpanel.FinishedRiders)
            {
                AverageWaitingTime += person.WaitingTime;
                AverageCompletionTime += person.CompletionTime;
                LeastTime = Math.Min(LeastTime, person.CompletionTime);
                MostTime = Math.Max(MostTime, person.CompletionTime);
            }
            AverageWaitingTime = AverageWaitingTime / controlpanel.FinishedRiders.Count;
            AverageCompletionTime = AverageCompletionTime / controlpanel.FinishedRiders.Count;
            foreach (Traveller traveller in controlpanel.FinishedRiders)
            {
                if (traveller.CompletionTime == MostTime)
                {
                    NumberOfPersonsWithMostTime++;
                }
                if (traveller.CompletionTime == LeastTime)
                {
                    NumberOfPersonsWithLeastTime++;
                }
            }
            if (MostTime == 0)
            {
                LeastTime = 0;
                AverageCompletionTime = 0;
                AverageWaitingTime = 0;
            }

            int finishedpersons = controlpanel.FinishedRiders.Count - 1;

            //while (pickupCount<numberOfRequests)
            //{
            //    var originatingFloor = random.Next(1, numberOfFloors + 1);
            //    var destinationFloor = random.Next(1, numberOfFloors + 1);
            //    if (originatingFloor != destinationFloor)
            //    {
            //        system.Pickup(originatingFloor, destinationFloor);
            //        pickupCount++;
            //    }
            //}

            while (system.AnyOutstandingPickups())
            {
                //system.Step();
                stepCount++;
            }

            Console.WriteLine("Transported {0} elevator riders to their requested destinations in {1} steps.", pickupCount, stepCount);
            Console.WriteLine();
            Console.WriteLine("Number of passengers: " + finishedpersons);
            Console.WriteLine("Total time for elevator: " + TotalTime * 10 + " ms.");
            Console.WriteLine("Average waiting time: " + AverageWaitingTime * 10 + " ms.");
            Console.WriteLine("Average completion time: " + AverageCompletionTime * 10 + " ms.");
            Console.WriteLine("Least total time taken: " + LeastTime * 10 + " ms. by " + NumberOfPersonsWithLeastTime + " persons.");
            Console.WriteLine("Most total time taken: " + MostTime * 10 + " ms. by " + NumberOfPersonsWithMostTime + " persons.");
            Console.ReadLine();

        }

        //Method takes a list of Persons and will fill it with people from the .txt file.
        public static void TravellerInfo(List<Traveller> listofTravellers, int row)
        {
            int id = 1;
            int floor = 0;
            int CompletionTime = 0;
            int WaitingTime = 0;
            string line;
            int count = 0;
            string fileName = "C:\\Users\\neerj\\OneDrive\\Desktop\\test\\TestDataAlgoDS.txt";

            StreamReader file = new StreamReader(fileName.Replace(@"\\", @"\"));
            for (int i = 0; i < row; i++)
            {
                file.ReadLine();
            }

            if (floor == 10)
            {
                floor = 0;
            }
            while ((line = file.ReadLine()) != null && floor < 10)
            {
                try
                {
                    string[] words = line.Split(',');
                    int[] possiblefloors = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    if (words[0] == "-1")
                    {

                    }

                    else
                    {
                        foreach (string s in words)
                        {

                            Int32.TryParse(s, out int x);
                            if (x != -1 && x != floor && possiblefloors.Contains(x))
                            {
                                foreach (Traveller p in listofTravellers)
                                {
                                    if (p.OriginatingFloor == floor)
                                    {
                                        count++;
                                    }
                                }
                                if (count < 50)
                                {
                                    listofTravellers.Add(new Traveller(id, floor, x));
                                    id++;
                                }
                                count = 0;
                            }

                        }
                    }
                    floor++;

                }

                catch
                {
                    Console.WriteLine("Something went wrong with reading input text file");
                    Environment.Exit(0);
                }
            }
            file.Close();
        }
    }
}
