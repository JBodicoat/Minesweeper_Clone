using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    //The tiles used for the minefield.
    class Tile
    {
        public int touching = 0; //how many mines tile is touching

        public string display = "?"; //default tile display

        public bool flagged = false;

        //Swaps the tiles display between ‘F’ and ‘?’.
        public void AlternateFlag()
        {
            flagged = !flagged;
            if (flagged == true)
            {
                display = "F";
            }
            else
            {
                display = "?";
            }
        }

        public bool IsMine = false;

        //Switches isMine to true.
        public void PlantMine()
        {
            IsMine = true;
        }

        public bool clicked = false;

        //Changes the tiles display to either a mine symbol if it’s a mine or the number of mines that it’s touching. clickCount is then incremented.
        public void Click()
        {
            display = touching.ToString();
            clicked = true;
            Program.clickCount++;
        }
    }
}
