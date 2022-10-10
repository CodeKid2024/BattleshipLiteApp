using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public class GridSpotModel
    {
        //holds the letter of the current spot on the grid
        public string SpotLetter { get; set; }
        //holds the number of the current spot on the grid
        public int SpotNumber { get; set; }
        //sets the default gridspot status to empty
        public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty;
    }
}
