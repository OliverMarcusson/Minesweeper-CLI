using System;

public class Board
{
    public int size;
    public int mines;
    int[] mineCoords;
    public string [,] board;
    Random rand = new Random();

    public Board(int size, int mines)
    {
        this.size = size;
        this.mines = mines;
        this.mineCoords = new int[mines];
        this.board = new string[size, size];

        // Creates board
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                this.board[i, j] = "#";
            }
        }
        
        int coordToMine;
        int placedMines = 0;
        while (placedMines != this.mines) {
            coordToMine = rand.Next(0, (int)(Math.Pow(size, 2))); // Gets random integer between 0 and size^2 - 1
            int[] rowIndex = GetRowIndex(coordToMine);
            if (this.board[rowIndex[0], rowIndex[1]] != Color("red", "X"))
            {
                this.board[rowIndex[0], rowIndex[1]] = Color("red", "X");
                Console.WriteLine($"Marked {rowIndex[0]}, {rowIndex[1]}.");
                this.mineCoords[placedMines] = coordToMine;
                placedMines += 1;
            }
        }
    }

    string Color(string color, string text) {
        string colorString = "";
        string reset = "\x1b[0m";

        switch (color)
        {
            case "red":
                colorString = "\x1b[91m";
                break;
        }
        
        return $"{colorString}{text}{reset}";
    }

    public int[] GetRowIndex(int coord)
    {
        int row = (int)Math.Floor((double)(coord / this.size));
        int index = coord % this.size;
        int[] rowIndex = {row, index};
        return rowIndex;
    } 

    private string Bar()
    {
        string bar = "   ";
        bar += "+";
            for (int i = 0; i < this.size; i++)
            {
                bar += (i == this.size - 1 ? "---" : "----");
            }
            bar += "+";
        
        return bar;
    }
    public void display() {
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
}
