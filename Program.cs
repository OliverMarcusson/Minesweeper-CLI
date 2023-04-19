using System;
using System.Collections;

Board board = new Board(8,10);
board.Display();
Random rand = new Random();
List<int> foundCoords = board.Search(new List<int>{Int32.Parse(Console.ReadLine())});
Console.WriteLine($"Found Coords: {string.Join(", ", foundCoords)}");