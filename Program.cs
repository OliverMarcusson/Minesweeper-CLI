using System;
using System.Collections;

int size;
int mines;

Console.Clear();

// Main game loop
while (true)
{
    // size input validation
    while (true)
    {
        try
        {
            Console.Write("Board Size (5-25): ");
            size = Int32.Parse(Console.ReadLine());
            if (size < 5 || size > 25) {Console.WriteLine("Invalid input!"); continue;}
            break;

        }
        catch 
        {
            Console.WriteLine("Invalid input!");
        }
    }

    // mines input validation
    while (true)
    {
        try
        {
            Console.Write($"Mines (1-{size * size}): ");
            mines = Int32.Parse(Console.ReadLine());
            if (mines < 1 || mines > size * size) {Console.WriteLine("Invalid input!"); continue;}
            break;
        }
        catch
        {
            Console.WriteLine("Invalid input!");
        }
    }

    Board board = new Board(size, mines);
    
    // Minesweeper loop
    while (true)
    {
        Console.Clear();
        board.Display(board.playerBoard);

        int? validatedInput;
        int firstCoord;
        bool mark;

        // Command input validation
        while (true)
        {
            Console.Write(">");
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            validatedInput = board.validateInput(input);
            if (validatedInput == null)
            {
                Console.WriteLine("Invalid input!");
            }
            else
            {
                mark = Char.IsLetter(input.Last());
                break;
            }
        }

        firstCoord = (int)validatedInput;

        if (mark)
        {
            board.Mark(firstCoord);
        }
        
        // Search and reveal tiles
        else
        {
            List<int>? foundCoords = board.Search(new List<int>{firstCoord});
            if (foundCoords == null) {
                Console.Clear();
                foreach (int coord in board.mineCoords)
                {
                    board.Reveal(coord);
                }
                board.Display(board.playerBoard);
                Console.WriteLine("You lose!");
                break;
            }
            foreach (int coord in foundCoords)
            {
                board.Reveal(coord);    
            }
        }

        // Win check
        if (board.Win())
        {
            Console.Clear();
            board.Display(board.playerBoard);

            Console.WriteLine("Congratulations, you won!");
            break;
        }
    }
    
    // Try again input validation
    char tryAgain;
    while (true)
    {
        try
        {
            Console.Write("Do you want to play again? [Y/N]: ");
            tryAgain = Console.ReadKey().KeyChar;
            break;
        }
        catch 
        {
            Console.WriteLine("Invalid input!");
        }
    }
    if (!(Char.ToUpper(tryAgain) == 'Y')) {break;}
    Console.Clear();
}