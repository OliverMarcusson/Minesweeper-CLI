using System;
using System.Collections;

Board board = new Board(12,10);
board.Display(board.board);
Console.WriteLine();
board.Display(board.playerBoard);
int input = board.inputToCoord(Console.ReadLine());
List<int> foundCoords = board.Search(new List<int>{input});
foundCoords.Sort();
foreach (int coord in foundCoords)
{
    board.Reveal(coord);    
}
board.Display(board.board);
Console.WriteLine();
board.Display(board.playerBoard);