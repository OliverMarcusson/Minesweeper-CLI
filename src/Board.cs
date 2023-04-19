using System;

public class Board
{
    public int size;
    public int mines;
    int[] mineCoords;
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

        // Creates board
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                this.board[i, j] = "#";
                this.playerBoard[i, j] = "#";
            }
        }
        
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
    }

    public void Display() {
        Console.WriteLine(this.Bar());
        for (int i = 0; i < this.size; i++)
        {
            Console.Write((i + 1).ToString().Length == 2 ? $"{i + 1} " : $"{i + 1}  ");
            for (int j = 0; j < this.size; j++)
            {
                Console.Write("| ");
                Console.Write($"{this.board[i, j]} ");
            }
            Console.Write("|\n");
            Console.WriteLine(this.Bar());
        }
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

    public int[] GetRowIndex(int coord)
    {
        int row = (int)Math.Floor((double)(coord / this.size));
        int index = coord % this.size;
        int[] rowIndex = {row, index};
        return rowIndex;
    }

    int[] GetNearbyCoords(int coord)
    {
        int[] nearbyCoords = {coord - this.size - 1, coord - this.size, coord - this.size + 1, coord - 1, coord + 1, coord + this.size - 1, coord + this.size, coord + this.size + 1};
        foreach (int nearbyCoord in nearbyCoords)
        {
            if ( nearbyCoord !< 0 || nearbyCoord !> (int)Math.Pow(this.size, 2) - 1)
            {
                
            }
        }
        
        return nearbyCoords;
    }
    int GetNearbyMines(int coord)
    {
        return 0;
    } 
}
