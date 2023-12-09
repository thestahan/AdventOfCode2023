string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var numbersList = inputData.Select(x => x.Split(" ").Select(y => long.Parse(y)).ToList()).ToList();

//long partOne = 0;
long partTwo = 0;

foreach (var list in numbersList)
{
    var subsequencesLists = new List<List<long>>
    {
        list
    };

    var currentSubsequence = list;

    while (currentSubsequence.Count == 0 || !currentSubsequence.All(x => x == 0))
    {
        currentSubsequence = GetDiffSubsequence(currentSubsequence);
        subsequencesLists.Add(currentSubsequence);
    }

    for (int i = subsequencesLists.Count - 2; i >= 0; i--)
    {
        var subsequence = subsequencesLists[i];
        var nextSubsequence = subsequencesLists[i + 1];

        //subsequence.Add(subsequence.Last() + nextSubsequence.Last());
        subsequencesLists[i] = subsequence.Prepend(subsequence.First() - nextSubsequence.First()).ToList();
    }

    //partOne += subsequencesLists.First().Last();
    partTwo += subsequencesLists.First().First();
}

Console.WriteLine($"Part one: {partTwo}");

List<long> GetDiffSubsequence(List<long> numbers)
{
    var subsequence = new List<long>();

    for (int i = 0; i < numbers.Count - 1; i++)
    {
        long diff = numbers[i + 1] - numbers[i];

        subsequence.Add(diff);
    }

    return subsequence;
}