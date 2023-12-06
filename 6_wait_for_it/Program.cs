string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var times = inputData[0].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();
var distances = inputData[1].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();

List<long> waysToBeat = [];

for (int i = 0; i < times.Count; i++)
{
    var time = times[i];
    var distance = distances[i];

    long options = 0;
    for (int j = 0; j < time; j++)
    {
        long timeLeft = time - j;
        long distanceMade = j * timeLeft;
        if (distanceMade > distance)
        {
            options++;
        }
    }

    waysToBeat.Add(options);
}

// Part two stays the same
Console.WriteLine($"Part one: {waysToBeat.Aggregate((a, b) => a * b)}");