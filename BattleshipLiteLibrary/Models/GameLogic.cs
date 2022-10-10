using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            //adds to the grid the number and letter on every row.
            List<string> letters = new List<string> { "A", "B", "C", "D", "E" };

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }
        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            // adds the number and letter to the current spot.
            GridSpotModel spot = new GridSpotModel 
            { 
            SpotLetter = letter, 
            SpotNumber = number, 
            Status = GridSpotStatus.Empty 
            };

            model.ShotGrid.Add(spot);
        }

        public static (string row, int column) SplitRow_and_Columns(string shot)
        {
            string row = "";
            int column = 0;

            if (shot.Length != 2)
            {
                throw new ArgumentException("This shot input had two many or two little letters.", "shot");
            }

            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }

        public static bool playerStillActive(PlayerInfoModel opponent)
        {
            bool isActive = false;

            //insead of checking all five ships, just check if one isn't sunk
            foreach (var ship in opponent.ShipLocations) 
            {
                isActive = true;
            }

            return isActive;
        }

        public static bool placeShip(PlayerInfoModel model, string location)
        {
            //Hold the position for the current ship your placing and set that spot's status to 'ship'
            (string row, int column) = SplitRow_and_Columns(location);

            bool output = false;
            bool isValidLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if ( isValidLocation && isSpotOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                });

                output = true;
            }

            return output;

        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            //check if the position your placing it on isn't another ship.
            bool isvalidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column) 
                {
                    isvalidLocation = false;
                }
            }

            return isvalidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isvalidLocation = false;

            foreach (var ship in model.ShotGrid)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isvalidLocation = true;
                }
            }

            return isvalidLocation;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    shotCount++;
                }
            }

            return shotCount;
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isvalidShot = false;

            foreach (var gridSpot in player.ShotGrid)
            {
                //finds if the spot is on the grid
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    //if it is on the grid, then check to make sure you haven't shot there before.
                    if (gridSpot.Status == GridSpotStatus.Empty)
                    {
                        isvalidShot = true;
                    }
                }
            }

            return isvalidShot;
        }

        public static bool IdentifyShotResults(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var ship in opponent.ShipLocations) //problem?
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isAHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }

            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            foreach (var gridspot in player.ShotGrid)
            {
                if (gridspot.SpotLetter == row.ToUpper() && gridspot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridspot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridspot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
