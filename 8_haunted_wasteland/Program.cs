string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var instructions = inputData.First();

var nodes = new List<Node>();

foreach (var line in inputData.Skip(2))
{
    nodes.Add(new Node { Name = line[0..3], LeftElement = line[7..10], RightElement = line[12..15] });
}

long steps = 0;

//var currentNode = nodes.First(x => x.Name == "AAA");

//while (currentNode.Name != "ZZZ")
//{
//    for (int i = 0; i < instructions.Length; i++)
//    {
//        bool isLeft = instructions[i] == 'L';

//        if (isLeft)
//        {
//            currentNode = nodes.First(x => x.Name == currentNode.LeftElement);
//        }
//        else
//        {
//            currentNode = nodes.First(x => x.Name == currentNode.RightElement);
//        }

//        steps++;

//        if (currentNode.Name == "ZZZ")
//        {
//            break;
//        }
//    }
//}

//Console.WriteLine($"Part one: {steps}");

steps = 0;

var currentNodes = nodes.Where(x => x.Name.EndsWith('A')).ToList();

var minSteps = new List<long>();

for (int k = 0; k < currentNodes.Count; k++)
{
    while (!currentNodes[k].Name.EndsWith('Z'))
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            bool isLeft = instructions[i] == 'L';

            if (isLeft)
            {
                currentNodes[k] = nodes.First(x => x.Name == currentNodes[k].LeftElement);
            }
            else
            {
                currentNodes[k] = nodes.First(x => x.Name == currentNodes[k].RightElement);
            }

            steps++;

            if (currentNodes[k].Name.EndsWith('Z'))
            {
                break;
            }
        }
    }

    minSteps.Add(steps);

    steps = 0;
}

Console.WriteLine($"Part two: {CalculateLCM(minSteps)}");

static long CalculateLCM(List<long> numbers)
{
    if (numbers == null || numbers.Count == 0)
    {
        throw new ArgumentException("List cannot be empty or null.");
    }

    long lcm = numbers[0];
    for (int i = 1; i < numbers.Count; i++)
    {
        lcm = GetLCM(lcm, numbers[i]);
    }
    return lcm;
}

static long GetLCM(long a, long b)
{
    return (a / GetGCD(a, b)) * b;
}

static long GetGCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

internal class Node
{
    public string Name { get; set; }
    public string LeftElement { get; set; }
    public string RightElement { get; set; }
}