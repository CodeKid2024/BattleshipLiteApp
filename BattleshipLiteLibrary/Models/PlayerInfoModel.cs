using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public class PlayerInfoModel
    {
        //stores the user name
        public string UsersName { get; set; }
        //hjolds tdhe ship positions
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
        //this one holds the location of the shots on the grid
        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();

    }
}
