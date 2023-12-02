string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var games = new List<Game>();

for (int i = 0; i < inputData.Length; i++)
{
    string? line = inputData[i];

    var roundsCombined = line[8..];
    var roundsSplit = roundsCombined.Split(';');

    var game = new Game { Number = i + 1 };
    foreach (var round in roundsSplit)
    {
        var cubes = round.Split(',').ToList();
        cubes.ForEach(x => x = x.Trim());

        var gameRound = new Game.Round();

        foreach (var cube in cubes)
        {
            int cubesNumber = int.Parse(cube.Where(char.IsDigit).ToArray());

            if (cube.Contains("red"))
            {
                gameRound.RedCubes = cubesNumber;
            }
            else if (cube.Contains("blue"))
            {
                gameRound.BlueCubes = cubesNumber;
            }
            else if (cube.Contains("green"))
            {
                gameRound.GreenCubes = cubesNumber;
            }
        }

        game.Rounds.Add(gameRound);
    }

    games.Add(game);
}

int possibleGamesCount = games.Where(x => x.IsPossible()).Sum(x => x.Number);

Console.WriteLine($"Part one: {possibleGamesCount}");

int cubesPower = games.Sum(x => x.GetGamePower());

Console.WriteLine($"Part two: {cubesPower}");

public class Game
{
    public int Number { get; set; }

    public List<Round> Rounds { get; set; } = [];

    public bool IsPossible() => Rounds.All(x => x.IsPossible());

    public int GetGamePower()
    {
        int maxRed = Rounds.Max(x => x.RedCubes);
        int greenRed = Rounds.Max(x => x.GreenCubes);
        int blueRed = Rounds.Max(x => x.BlueCubes);

        return maxRed * greenRed * blueRed;
    }

    public class Round
    {
        private readonly int _maxRed = 12;
        private readonly int _maxGreen = 13;
        private readonly int _maxBlue = 14;

        public int BlueCubes { get; set; }
        public int RedCubes { get; set; }
        public int GreenCubes { get; set; }

        public bool IsPossible() =>
            RedCubes <= _maxRed && GreenCubes <= _maxGreen && BlueCubes <= _maxBlue;
    }
}