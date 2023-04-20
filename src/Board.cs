using System;
using System.Collections;
using System.Linq;
using System.Threading;

public class Board
{
    static string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public int size;
    public int mines;
    public int[] mineCoords;
    public string [,] board;
    public string [,] playerBoard;
    Random rand = new Random();

    public Board(int size, int mines)
    {
        this.size = size;
        this.mines = mines;
        this.mineCoords = new int[mines];
        this.board = new string[size, size];
        this.playerBoard = new string[size, size];

        // Generates boards
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                this.board[i, j] = "#";
                this.playerBoard[i, j] = "#";
            }
        }
        
        // Places mines at random locations
        int coordToMine;
        int placedMines = 0;
        while (placedMines != this.mines) {
            coordToMine = rand.Next(0, (int)(Math.Pow(size, 2))); // Gets random integer between 0 and size^2 - 1
            int[] rowIndex = GetRowIndex(coordToMine);
            if (this.board[rowIndex[0], rowIndex[1]] != Util.Colored(Util.Color.Red, "X"))
            {
                this.board[rowIndex[0], rowIndex[1]] = Util.Colored(Util.Color.Red, "X");
                this.mineCoords[placedMines] = coordToMine;
                placedMines += 1;
            }
        }

        // Marking tiles on this.board with correct numbers.
        int coord;
        int[] newRowIndex;
        int nearbyMines;
        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < this.size; j++)
            {
                newRowIndex = new int[]{i, j};
                coord = GetCoord(newRowIndex);
                if (!(this.board[i, j] == Util.Colored(Util.Color.Red, "X")))
                {
                    nearbyMines = GetNearbyMines(coord);
                    if (nearbyMines == 0)
                    {
                        this.board[i, j] = " ";
                    }
                    else
                    {
                        this.board[i, j] = nearbyMines.ToString();
                    }
                }
            }
        }
    }

    public void Display(string[,] board) {
        Console.WriteLine(this.Bar());
        for (int i = 0; i < this.size; i++)
        {
            Console.Write((i + 1).ToString().Length == 2 ? $"{i + 1} " : $"{i + 1}  ");
            for (int j = 0; j < this.size; j++)
            {
                Console.Write("| ");
                Console.Write($"{board[i, j]} ");
            }
            Console.Write("|\n");
            Console.WriteLine(this.Bar());
        }

        string characters = "     ";
        for (int i = 0; i < this.size; i++)
        {
            characters += i != size ? $"{CHARS[i]}   " : $"{CHARS[i]}";
        }
        Console.WriteLine(characters);
    }

    private string Bar()
    {
        string bar = "   ";
        bar += "+";
            for (int i = 0; i < this.size; i++)
            {
                bar += (i == this.size - 1 ? "---" : "---+");
            }
            bar += "+";
        
        return bar;
    }

    public List<int>? Search(List<int> coords)
    {
        // Console.WriteLine($"Searching coords: {string.Join(", ", coords)}");
        if (coords.Count == 1) 
        {
            int[] rowIndex = GetRowIndex(coords[0]);
            // Console.WriteLine($"First coord: {coords[0]}, Row Index: {string.Join(", ", rowIndex)}");
            if (this.board[rowIndex[0], rowIndex[1]] == Util.Colored(Util.Color.Red, "X"))
            {
                return null;
            }

            if (!(this.board[rowIndex[0], rowIndex[1]] == " "))
            {
                return coords;
            }
        }

        List<int> foundCoords = new List<int>();
        foreach (int coord in coords)
        {
            List<int> nearbyCoords = GetNearbyCoords(coord);
            foreach (int nearbyCoord in nearbyCoords)
            {
                int[] rowIndex = GetRowIndex(nearbyCoord);
                if (this.board[rowIndex[0], rowIndex[1]] == " " && !(coords.Contains(nearbyCoord)) && !(foundCoords.Contains(nearbyCoord)))
                {
                    foundCoords.Add(nearbyCoord);
                }
            }
        }

        List<int> totalCoords = new List<int>();
        if (foundCoords.Count > 0)
        {
            coords = coords.Concat(foundCoords).ToList<int>();
            coords = Search(coords);
            
        }
        else 
        {
            // Console.WriteLine($"Zeroes: {string.Join(", ", coords)}");
            foreach (int coord in coords)
            {
                totalCoords.Add(coord);
                List<int> nearbyCoords = GetNearbyCoords(coord);
                foreach (int nearbyCoord in nearbyCoords)
                {
                    // 
                    if (!(totalCoords.Contains(nearbyCoord)) && !(coords.Contains(nearbyCoord)))
                    {
                        totalCoords.Add(nearbyCoord);
                    }
                }
            }
            return totalCoords;
        }
        
        return coords;
    }

    public void Reveal(int coord)
    {
        int[] rowIndex = GetRowIndex(coord);
        this.playerBoard[rowIndex[0], rowIndex[1]] = this.board[rowIndex[0], rowIndex[1]];
    }

    int[] GetRowIndex(int coord)
    {
        int row = (int)Math.Floor((double)(coord / this.size));
        int index = coord % this.size;
        int[] rowIndex = {row, index};
        return rowIndex;
    }

    int GetCoord(int[] rowIndex)
    {
        int coord = rowIndex[0] * this.size + rowIndex[1];
        return coord;
    }

    List<int> GetNearbyCoords(int coord)
    {
        List<int> nearbyCoords = new List<int>{coord - this.size - 1, coord - this.size, coord - this.size + 1, 
                                               coord - 1, coord + 1, coord + this.size - 1, 
                                               coord + this.size, coord + this.size + 1};
        List<int> toReturn = new List<int>();
        foreach (int nearbyCoord in nearbyCoords)
        {
            bool notOutOfRange = !(nearbyCoord < 0) && !(nearbyCoord > (int)Math.Pow(this.size, 2) - 1);
            if (notOutOfRange)
            {
                bool coordOnLeftEdge = coord % this.size == 0;
                bool coordOnRightEdge = coord % this.size == this.size - 1;
                if (!(coordOnLeftEdge && nearbyCoord % this.size == this.size - 1) && !(coordOnRightEdge && nearbyCoord % this.size == 0))
                {
                    toReturn.Add(nearbyCoord);
                }
            }
        }
        // Console.WriteLine($"Coords to return: {string.Join(", ", toReturn)}\n");
        return toReturn;
    }
    int GetNearbyMines(int coord)
    {
        List<int> nearbyCoords = GetNearbyCoords(coord);
        int nearbyMines = 0;

        foreach (int nearbyCoord in nearbyCoords)
        {
            int[] rowIndex = GetRowIndex(nearbyCoord);
            if (this.board[rowIndex[0], rowIndex[1]] == Util.Colored(Util.Color.Red, "X"))
            {
                nearbyMines++;
            }
        }
        return nearbyMines;
    }

    public int? validateInput(string input)
    {
        bool mark = Char.IsLetter(input.Last());
        int num = 0;
        
        if (mark)
        {
            if (!(Char.ToUpper(input.Last()) == 'M')) {return null;}
            num = Int32.Parse(input.Substring(1, input.Length - 2));
        }
        else
        {
            num = Int32.Parse(input.Substring(1, input.Length - 1));
        }
        
        bool goodChar = false;
        try {goodChar = Board.CHARS.Contains(char.ToUpper(input[0]));}
        catch {return null;}
        
        bool goodNum = num > 0 && num <= this.size;

        if (goodChar && goodNum) 
        {
            int coord = 0;
            // Changes number substring depending if input is a mark command or not
            if (Char.IsLetter(input[^1]))
            {
                coord += (Int32.Parse(input.Substring(1, input.Length - 2)) - 1) * (this.size);
                coord += Board.CHARS.IndexOf(input[0].ToString().ToUpper());
            }
            else
            {
                coord += (Int32.Parse(input.Substring(1, input.Length - 1)) - 1) * (this.size);
                coord += Board.CHARS.IndexOf(input[0].ToString().ToUpper());
            }
            return coord;
        } 
        else {return null;}
    }

    public void Mark(int coord)
    {
        int[] rowIndex = GetRowIndex(coord);
        bool alreadyMarked = this.playerBoard[rowIndex[0], rowIndex[1]] == Util.Colored(Util.Color.Red, "M");
        bool alreadyRevealed = this.playerBoard[rowIndex[0], rowIndex[1]] != "#";
        if (alreadyMarked)
        {
            this.playerBoard[rowIndex[0], rowIndex[1]] = "#";
            return;
        }
        
        if (!(alreadyRevealed))
        {
            this.playerBoard[rowIndex[0], rowIndex[1]] = Util.Colored(Util.Color.Red, "M");
        }
    }

    public bool Win()
    {
        string[,] winBoard = this.playerBoard.Clone() as string[,];
        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < this.size; j++)
            {
                if (winBoard[i, j] == Util.Colored(Util.Color.Red, "M"))
                {
                    winBoard[i, j] = Util.Colored(Util.Color.Red, "X");
                }
            }
        }
        string winBoardString = "";
        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < this.size; j++)
            {
                winBoardString += winBoard[i, j];
            }
        }

        string boardString = "";
        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < this.size; j++)
            {
                boardString += this.board[i, j];
            }
        }

        if (winBoardString == boardString) {return true;} else {return false;}
    } 
}
