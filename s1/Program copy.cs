int n_rows = 10;
int n_cols = 30;
int score = 0;
string statusMessage = "";
char[,] map = new char[n_rows, n_cols];
var random = new Random();

int n_treasures = (int)(n_rows * n_cols * 0.01);
char treasure = '$';

int n_obstacles = (int)(n_rows * n_cols * 0.05);
char obstacle = 'o';


for (var y = 0; y < n_rows; y++)
{
  for (var x = 0; x < n_cols; x++)
  {
    map[y, x] = ' ';
  }
}

for (var i = 0; i < n_obstacles; i++)
{
  map[random.Next(n_rows), random.Next(n_cols)] = obstacle;
}

for (var i = 0; i < n_treasures; i++)
{
  map[random.Next(n_rows), random.Next(n_cols)] = treasure;
}

var player =
(
    Y: random.Next(n_rows),
    X: random.Next(n_cols)
);

if (map[player.Y, player.X] == treasure)
{
  score += 1;
  statusMessage = "Lucky you! Spawned on a treasure";
}
map[player.Y, player.X] = '@';

Console.Clear();
Console.Write("PRESS ANY KEY TO START....");

ConsoleKeyInfo cki;
var gameStopped = false;
while(!gameStopped)
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

  if (player.X < 0 || player.X >= n_cols || player.Y < 0 || player.Y >= n_rows)
  {
    statusMessage = "You crashed!";
  }

  player.X = Math.Clamp(player.X, 0, n_cols - 1);
  player.Y = Math.Clamp(player.Y, 0, n_rows - 1);

  if (map[player.Y, player.X] == treasure)
  {
    score += 1;
    statusMessage = "You found a treasure!";
    if (score == n_treasures)
    {
      statusMessage = $"YOU WIN! Total treasure: ${score}";
      gameStopped = true;
    }
  }

  if (map[player.Y, player.X] == obstacle)
  {
    player.X = oldCoords.X;
    player.Y = oldCoords.Y;
    statusMessage = "You crashed!";
  }

  map[oldCoords.Y, oldCoords.X] = ' ';
  map[player.Y, player.X] = '@';

  Console.Clear();
  Console.WriteLine($"TREASURES: {score}");
  Console.WriteLine($"PLAYER (X,Y): ({player.X}, {player.Y})");
  Console.WriteLine(statusMessage);
  for (var x = 0; x < n_rows; x++)
  {
    for (var y = 0; y < n_cols; y++)
    {
      Console.Write(map[x, y]);
    }
    Console.WriteLine();
  }
}
