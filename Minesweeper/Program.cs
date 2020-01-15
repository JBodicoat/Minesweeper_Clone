using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Minesweeper
{
    //Minesweeper uses a grid, the minefield, of the users chosen dimensions. In the minefield a number of mines, chosen by the user, are placed.
    //Tiles display as a "?" on the screen unless the user selects that tile.
    //When a tile is selected it reveals either that it is a mine or, if it's not a mine, the number of mines that it is touching (diagonals included).
    //Revealing a tile touching 0 mines will then reveal the tiles around it.
    //The user loses by selecting a mine or wins by revealing all tiles that are not mines.
    class Program
    {
        public static int clickCount = 0;
        public static int mineFieldColumns = -1;
        public static int mineFieldRows = -1;

        static void Main(string[] args)
        {
            //Initialises minefield array.
            mineFieldColumns = GetIntFromUser("Enter the width of the field:");
            mineFieldRows = GetIntFromUser("Enter the height of the field:");
            Tile[,] mineField = new Tile[mineFieldColumns, mineFieldRows];
            for(int i = 0; i < mineFieldColumns; i++)
            {
                for(int j = 0; j < mineFieldRows; j++)
                {
                    mineField[i, j] = new Tile();
                }
            }

            //Initalies randomCoordinate array.
            int numberOfMines = -1;
            do
            {
                numberOfMines = GetIntFromUser("Enter the number of mines you want:");
            } while (numberOfMines >= mineFieldColumns * mineFieldRows);
            Random randomNumber = new Random();
            int[,] randomCoordinate = new int[numberOfMines , 2];
            for(int i = 0; i < numberOfMines; i++)
            {
                randomCoordinate[i, 0] = -1;
                randomCoordinate[i, 1] = -1;
            }

            //Randomly places the mines in the mine field.
            int loop = 0;
            for (int i = 0; i < numberOfMines; i++)
            {
                do
                {
                    randomCoordinate[i, 0] = randomNumber.Next(0, mineFieldColumns);
                    randomCoordinate[i, 1] = randomNumber.Next(0, mineFieldRows);

                    loop = 0;
                    for (int j = 0; j < i; j++)
                    { 
                        if (randomCoordinate[j, 0] == randomCoordinate[i, 0] && randomCoordinate[j, 1] == randomCoordinate[i, 1])
                        {
                            loop++;
                        }
                    }
                } while (loop > 0);
                mineField[randomCoordinate[i, 0], randomCoordinate[i, 1]].IsMine = true;
            }

            //Sets touching values for tiles.
            for (int i = 0; i < mineFieldColumns; i++)
            {
                for (int j = 0; j < mineFieldRows; j++)
                {
                    if(mineField[i, j].IsMine == true)
                    {
                        if (i == 0 && j == 0)
                        {
                            mineField[i + 1, j].touching++;
                            mineField[i + 1, j + 1].touching++;
                            mineField[i, j + 1].touching++;
                        }
                        else if (i == mineFieldColumns - 1 && j == mineFieldRows - 1)
                        {
                            mineField[i - 1, j].touching++;
                            mineField[i - 1, j - 1].touching++;
                            mineField[i, j - 1].touching++;
                        }
                        else if (i == 0 && j < mineFieldRows - 1)
                        {
                            mineField[i, j - 1].touching++;
                            mineField[i, j + 1].touching++;
                            mineField[i + 1, j - 1].touching++;
                            mineField[i + 1, j].touching++;
                            mineField[i + 1, j + 1].touching++;
                        }
                        else if (j == 0 && i < mineFieldColumns - 1)
                        {
                            mineField[i - 1, j].touching++;
                            mineField[i - 1, j + 1].touching++;
                            mineField[i, j + 1].touching++;
                            mineField[i + 1, j].touching++;
                            mineField[i + 1, j + 1].touching++;
                        }
                        else if (i == mineFieldColumns - 1 && j > 0)
                        {
                            mineField[i - 1, j - 1].touching++;
                            mineField[i - 1, j].touching++;
                            mineField[i - 1, j + 1].touching++;
                            mineField[i, j - 1].touching++;
                            mineField[i, j + 1].touching++;
                        }
                        else if (j == mineFieldRows - 1 && i > 0)
                        {
                            mineField[i - 1, j - 1].touching++;
                            mineField[i - 1, j].touching++;
                            mineField[i, j - 1].touching++;
                            mineField[i + 1, j - 1].touching++;
                            mineField[i + 1, j].touching++;
                        }
                        else if (i == 0)
                        {
                            mineField[i, j - 1].touching++;
                            mineField[i + 1, j - 1].touching++;
                            mineField[i + 1, j].touching++;
                        }
                        else if (j == 0)
                        {;
                            mineField[i - 1, j].touching++;
                            mineField[i - 1, j + 1].touching++;
                            mineField[i, j + 1].touching++;
                        }
                        else
                        {
                            mineField[i - 1, j - 1].touching++;
                            mineField[i - 1, j].touching++;
                            mineField[i - 1, j + 1].touching++;
                            mineField[i, j - 1].touching++;
                            mineField[i, j + 1].touching++;
                            mineField[i + 1, j - 1].touching++;
                            mineField[i + 1, j].touching++;
                            mineField[i + 1, j + 1].touching++;
                        }
                    }
                }
            }

            bool gameRunning = true;
            bool mineHit = false;
            int[] userCoordinateChoice = { -1, -1};
            string userInput = "default";

            DisplayGrid(mineField, mineFieldColumns);

            do
            {
                userInput = "default";
                do
                {
                    Console.WriteLine("\n\nDo you want to\nA. Click a tile\nB. Flag a tile");
                    userInput = Console.ReadLine();
                    userInput = userInput.ToUpper();
                    if(userInput != "A" && userInput != "B")
                    {
                        Console.WriteLine("\nInvalid input\n");
                    }
                } while (userInput != "A" && userInput != "B");

                //Gets coordinates from the user of the tile they want to either click or flag.
                do
                {
                    userCoordinateChoice[0] = GetIntFromUser("Enter the x coordinate:");
                } while (userCoordinateChoice[0] >= mineFieldColumns);
                do
                {
                    userCoordinateChoice[1] = GetIntFromUser("Enter the y coordinate:");
                } while (userCoordinateChoice[1] >= mineFieldRows);

                //Clicks users selected tile.
                if (userInput == "A")
                {
                    if (mineField[userCoordinateChoice[0], userCoordinateChoice[1]].IsMine == true)
                    {
                        gameRunning = false;
                        mineHit = true;
                        mineField[userCoordinateChoice[0], userCoordinateChoice[1]].display = "*";
                        for (int i = 0; i < mineFieldColumns; i++)
                        {
                            for (int j = 0; j < mineFieldRows; j++)
                            {
                                if (mineField[i, j].IsMine == true)
                                {
                                    mineField[i, j].display = "*";
                                }
                            }
                        }
                    }
                    else
                    {
                        mineField = Reveal(mineField, userCoordinateChoice);
                    }
                }
                //Flags the selected tile.
                else
                {
                    if(mineField[userCoordinateChoice[0], userCoordinateChoice[1]].clicked == true)
                    {
                        Console.WriteLine("\nTile already revealed\n");
                    }
                    else
                    {
                        mineField[userCoordinateChoice[0], userCoordinateChoice[1]].AlternateFlag();
                    }
                }
                DisplayGrid(mineField, mineFieldColumns);
                if (clickCount == mineFieldColumns * mineFieldRows - numberOfMines)
                {
                    gameRunning = false;
                }
            } while (gameRunning == true) ;

            //Prints end game messages.
            if(mineHit == true)
            {
                Console.WriteLine("\nYou hit a mine :(");
            }
            else
            {
                Console.WriteLine("\nYou successfully avoided all the mines!");
            }
            Console.WriteLine("\nPress Enter to exit.");
            Console.ReadLine();
        }

        //Prints the minefield to the console.
        static void DisplayGrid(Tile[,] mineField, int mineFieldx)
        {
            Console.Clear();

            int spaces = -1; //for the number of units

            //Writes the top row.
            Console.Write("    ");
            if (mineFieldRows <= 10)
            {
                Console.Write(" ");
            }
            spaces = (mineFieldRows - 1).ToString().Count() - 1;
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }

            for (int i = 0; i < mineFieldx; i++)
            {

                Console.Write(i + " ");
            }
            Console.WriteLine();

            //Writes subsequent rows.    
            for (int i = 0; i < mineFieldRows; i++)
            {
                Console.WriteLine("");
                spaces = (i.ToString().Count() - mineFieldRows.ToString().Count()) * -1;
                for (int j = 0; j < spaces; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(i + "   ");
                for(int j = 0; j < mineFieldx; j++)
                {
                    spaces = j.ToString().Count();
                    Console.Write(mineField[j, i].display);
                    for(int k = 0; k < spaces; k++)
                    {
                        Console.Write(" ");
                    }
                }
            }
        }

        //Prints message to console. Returns an integer input by the user.
        static int GetIntFromUser(string message)
        {
            int integer = 0;
            while (true)
            {
                Console.WriteLine(message);
                if (Int32.TryParse(Console.ReadLine(), out integer) && integer > -1)
                {
                    return integer;
                }
                else
                {
                    Console.WriteLine("\nInvalid input\n");
                }
            }
        }

        //Clicks the selected tile at the coordinates passed in.
        //If the tile revealed is touching 0 mines the surrounding tiles are also revealed.
        //Revealing a mine will end the game. Returns the mineField.
        static Tile[,] Reveal(Tile[,] mineField, int[] coordinateOriginal)
        {
            if(mineField[coordinateOriginal[0], coordinateOriginal[1]].IsMine == false) {               
                if (mineField[coordinateOriginal[0], coordinateOriginal[1]].touching == 0 && mineField[coordinateOriginal[0], coordinateOriginal[1]].clicked == false)
                {
                    int[] coordinateNew = { -99, -99 };
                    mineField[coordinateOriginal[0], coordinateOriginal[1]].Click();

                    //if & else if statements to avoid IndexOutOfRangeException.
                    if (coordinateOriginal[0] == 0 && coordinateOriginal[1] == 0)
                    {
                        mineField = RevealRight(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                        mineField = RevealLowerRight(mineField, coordinateOriginal);

                    }
                    else if (coordinateOriginal[0] == mineFieldColumns - 1 && coordinateOriginal[1] == mineFieldRows - 1)
                    {
                        mineField = RevealUpperLeft(mineField, coordinateOriginal);
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealLeft(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] == 0 && coordinateOriginal[1] < mineFieldRows - 1)
                    {
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealUpperRight(mineField, coordinateOriginal);
                        mineField = RevealRight(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                        mineField = RevealLowerRight(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] < mineFieldColumns - 1 && coordinateOriginal[1] == 0)
                    {
                        mineField = RevealLeft(mineField, coordinateOriginal);
                        mineField = RevealRight(mineField, coordinateOriginal);
                        mineField = RevealLowerLeft(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                        mineField = RevealLowerRight(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] == mineFieldColumns - 1 && coordinateOriginal[1] > 0)
                    {
                        mineField = RevealUpperLeft(mineField, coordinateOriginal);
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealLeft(mineField, coordinateOriginal);
                        mineField = RevealLowerLeft(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] > 0 && coordinateOriginal[1] == mineFieldRows - 1)
                    {
                        mineField = RevealUpperLeft(mineField, coordinateOriginal);
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealUpperRight(mineField, coordinateOriginal);
                        mineField = RevealLeft(mineField, coordinateOriginal);
                        mineField = RevealRight(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] == 0 && coordinateOriginal[1] == mineFieldRows - 1)
                    {
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealUpperRight(mineField, coordinateOriginal);
                        mineField = RevealRight(mineField, coordinateOriginal);
                    }
                    else if (coordinateOriginal[0] == mineFieldColumns - 1 && coordinateOriginal[1] == 0)
                    {
                        mineField = RevealLeft(mineField, coordinateOriginal);
                        mineField = RevealLowerLeft(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                    }
                    else
                    {
                        mineField = RevealUpperLeft(mineField, coordinateOriginal);
                        mineField = RevealUpper(mineField, coordinateOriginal);
                        mineField = RevealUpperRight(mineField, coordinateOriginal);
                        mineField = RevealLeft(mineField, coordinateOriginal);
                        mineField = RevealRight(mineField, coordinateOriginal);
                        mineField = RevealLowerLeft(mineField, coordinateOriginal);
                        mineField = RevealLower(mineField, coordinateOriginal);
                        mineField = RevealLowerRight(mineField, coordinateOriginal);
                    }
                }
                else if(mineField[coordinateOriginal[0], coordinateOriginal[1]].clicked == false)
                {
                    mineField[coordinateOriginal[0], coordinateOriginal[1]].Click();
                }
            }
            return mineField;
        }

        //Reveals the tile 1 up and 1 to the left of the current.
        static Tile[,] RevealUpperLeft(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] - 1;
            coordinateNew[1] = coordinateOriginal[1] - 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 up of the current.
        static Tile[,] RevealUpper(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0];
            coordinateNew[1] = coordinateOriginal[1] - 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 up and 1 to the right of the current.
        static Tile[,] RevealUpperRight(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] + 1;
            coordinateNew[1] = coordinateOriginal[1] - 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 left of the current.
        static Tile[,] RevealLeft(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] - 1;
            coordinateNew[1] = coordinateOriginal[1];
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 right of the current.
        static Tile[,] RevealRight(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] + 1;
            coordinateNew[1] = coordinateOriginal[1];
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 down and 1 left of the current.
        static Tile[,] RevealLowerLeft(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] - 1;
            coordinateNew[1] = coordinateOriginal[1] + 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 down of the current.
        static Tile[,] RevealLower(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0];
            coordinateNew[1] = coordinateOriginal[1] + 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }

        //Reveals the tile 1 down and 1 to the right of the current.
        static Tile[,] RevealLowerRight(Tile[,] mineField, int[] coordinateOriginal)
        {
            int[] coordinateNew = { -999, -999 };
            coordinateNew[0] = coordinateOriginal[0] + 1;
            coordinateNew[1] = coordinateOriginal[1] + 1;
            mineField = Reveal(mineField, coordinateNew);
            return mineField;
        }
    }   
}