using System;
using System.Collections;

Board board = new Board(10,1);
while (true)
{
    board.Display(board.board);
    Console.WriteLine();
    board.Display(board.playerBoard);

    int? validatedInput;
    int firstCoord;
    bool mark;

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
    else
    {
        List<int>? foundCoords = board.Search(new List<int>{firstCoord});
        if (foundCoords == null) {
            Console.WriteLine("You lose!");
            break;
        }
        foreach (int coord in foundCoords)
        {
            board.Reveal(coord);    
        }
    }

    if (board.Win())
    {
        board.Display(board.board);
        Console.WriteLine();
        board.Display(board.playerBoard);

        Console.WriteLine("Congratulations, you won!");
        break;
    }
}
