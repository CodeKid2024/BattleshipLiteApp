using BattleshipLiteLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("player1");
            PlayerInfoModel opponent = CreatePlayer("Player2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(activePlayer);

                recordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.playerStillActive(opponent);

                if (doesGameContinue)
                {
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }
            } while (winner == null);

            SayWinner(winner);

            Console.ReadLine();
        }

        private static void SayWinner(PlayerInfoModel winner)
        {
            Console.Clear();
            Console.WriteLine($"Congratulations! {winner.UsersName} is the Winner!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void recordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot();
                try
                {
                    (row, column) = GameLogic.SplitRow_and_Columns(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    isValidShot = false;
                }

                if (!isValidShot)
                {
                    Console.WriteLine("This is outside the range of the perimeters, please try again.");
                }
            } while (!isValidShot);

            bool aHit = GameLogic.IdentifyShotResults(activePlayer, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, aHit);

            displayShotResults(aHit);

            Console.Clear();
        }

        private static void displayShotResults(bool aHit)
        {
            string resultMessage = "";

            //not as long as a normal if statment but less readable
            resultMessage = aHit ? "Good Job! You sunk a ship!" : "You missed...";

            Console.WriteLine();
            Console.WriteLine("   " + resultMessage);
            Console.ReadLine();
        }

        private static string AskForShot()
        {
            Console.WriteLine();
            Console.Write("   Please input your shot location: ");
            string shotLocation = Console.ReadLine();

            return shotLocation;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            Console.WriteLine("   Current Player: " + activePlayer.UsersName.ToUpper());

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                //If at the end of a row, then go down.
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                //If there's not a letter there, then write it.
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"   {gridSpot.SpotLetter}{gridSpot.SpotNumber}   ");
                } 

                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write("    X   ");

                } 

                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("    O   ");
                }

                else
                {
                    Console.Write("    ?   ");
                }
            }

            Console.WriteLine();
            Console.WriteLine();

        }

        private static void WelcomeMessage()
        {
            //Welcome message
            Console.WriteLine("Welcome to BattleshipLite!");
            Console.WriteLine("Made by Tim Corey.");
            Console.WriteLine();
        }

        //getting player information
        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            output.UsersName = AskForUsersName();

            GameLogic.InitializeGrid(output);

            PlaceShips(output);

            Console.Clear();

            return output;
        }

        private static string AskForUsersName()
        {
            //asks, stores, and returns the name.
            Console.Write("What would you like to be called: ");
            string Name = Console.ReadLine();

            return Name;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");

                string location = Console.ReadLine();

                bool isValidLocation = false;
                try
                {
                    isValidLocation = GameLogic.placeShip(model, location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
