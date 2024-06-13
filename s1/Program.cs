using System.Runtime.CompilerServices;

const int rowCount = 10;
const int colCount = 30;
const int treasureCount = (int)(rowCount * colCount * 0.01);
const int obstacleCount = (int)(rowCount * colCount * 0.05);

var score = 0;
var statusMessage = "";
var map = new char[rowCount, colCount];

var treasureSymbol = '$';
var obstacleSymbol = 'o';
var playerSymbol = '@';
var blankSymbol = ' ';

void clearGrid(char symbol)
{
  for (var y = 0; y < rowCount; y++)
  {
    for (var x = 0; x < colCount; x++)
    {
      map[y, x] = symbol;
    }
  }
}

void placeSymbols(char symbol, int symbolCount)
{
  for (int i = 0; i < symbolCount; i++)
  {
    var coordinates = getFreeSpace();
    map[coordinates.Item1, coordinates.Item2] = symbol;
  }
}

(int, int) getFreeSpace()
{
  var coordinates = (X: Random.Shared.Next(rowCount), Y: Random.Shared.Next(colCount));
  while (map[coordinates.X, coordinates.Y] != blankSymbol)
  {
    coordinates = (X: Random.Shared.Next(rowCount), Y: Random.Shared.Next(colCount));
  }
  return coordinates;
}



clearGrid(blankSymbol);

var player =
(
    Y: Random.Shared.Next(rowCount),
    X: Random.Shared.Next(colCount)
);
map[player.Y, player.X] = playerSymbol;

placeSymbols(obstacleSymbol, obstacleCount);
placeSymbols(treasureSymbol, treasureCount);

Console.Clear();
Console.Write("PRESS ANY KEY TO START....");

ConsoleKeyInfo cki;
var gameStopped = false;
while (!gameStopped)
{
  cki = Console.ReadKey();

  var oldCoords = player;
  switch (cki.Key)
  {
    case ConsoleKey.UpArrow:
      player.Y -= 1;
      break;
    case ConsoleKey.DownArrow:
      player.Y += 1;
      break;
    case ConsoleKey.LeftArrow:
      player.X -= 1;
      break;
    case ConsoleKey.RightArrow:
      player.X += 1;
      break;
    case ConsoleKey.Z:
    case ConsoleKey.Escape:
      gameStopped = true;
      break;
  }

  if (player.X < 0 || player.X >= colCount || player.Y < 0 || player.Y >= rowCount)
  {
    statusMessage = "You crashed!";
  }

  player.X = Math.Clamp(player.X, 0, colCount - 1);
  player.Y = Math.Clamp(player.Y, 0, rowCount - 1);

  if (map[player.Y, player.X] == treasureSymbol)
  {
    score += 1;
    statusMessage = "You found a treasure!";
    if (score == treasureCount)
    {
      statusMessage = $"YOU WIN! Total treasure: ${score}";
      gameStopped = true;
    }
  }

  if (map[player.Y, player.X] == obstacleSymbol)
  {
    player.X = oldCoords.X;
    player.Y = oldCoords.Y;
    statusMessage = "You crashed!";
  }

  map[oldCoords.Y, oldCoords.X] = blankSymbol;
  map[player.Y, player.X] = playerSymbol;

  Console.Clear();
  Console.WriteLine($"TREASURES: {score}");
  Console.WriteLine($"PLAYER (X,Y): ({player.X}, {player.Y})");
  Console.WriteLine(statusMessage);
  for (var y = 0; y < rowCount; y++)
  {
    for (var x = 0; x < colCount; x++)
    {
      Console.Write(map[y, x]);
    }
    Console.WriteLine();
  }
}
