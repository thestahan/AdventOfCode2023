string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

ulong points = 0;

foreach (var line in inputData)
{
    var substringWithNumbers = line.Split(':')[1].Trim();
    var numbersSplit = substringWithNumbers.Split('|');
    var winningNumbers = numbersSplit[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
    var elfNumbers = numbersSplit[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
    int elfWinningNumbers = elfNumbers.Intersect(winningNumbers).Count();

    if (elfWinningNumbers > 0)
    {
        points += GetWinningNumber(elfWinningNumbers);
    }
}

Console.WriteLine($"Part one: {points}");

List<Card> cards = [];

foreach (var line in inputData)
{
    var substringWithNumbers = line.Split(':')[1].Trim();
    var numbersSplit = substringWithNumbers.Split('|');
    var winningNumbers = numbersSplit[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
    var elfNumbers = numbersSplit[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

    cards.Add(new Card { ElfNumbers = elfNumbers, WinningNumbers = winningNumbers, Count = 1 });
}

for (int i = 0; i < cards.Count; i++)
{
    var currentCard = cards[i];
    int wonCopies = currentCard.WinningNumbers.Intersect(currentCard.ElfNumbers).Count();

    for (int k = 0; k < currentCard.Count; k++)
    {
        for (int j = 1; j <= wonCopies; j++)
        {
            if (i + j < cards.Count)
            {
                cards[i + j].Count++;
            }
        }
    }
}

Console.WriteLine($"Part two: {cards.Sum(x => x.Count)}");

static ulong GetWinningNumber(int n)
{
    ulong winningNumber = 1;

    for (int i = 1; i < n; i++)
    {
        winningNumber *= 2;
    }

    return winningNumber;
}

internal class Card
{
    public List<int> WinningNumbers { get; set; } = [];
    public List<int> ElfNumbers { get; set; } = [];
    public int Count { get; set; }
}