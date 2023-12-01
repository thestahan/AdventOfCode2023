string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var calibrationValues = new List<int>();

foreach (var inputLine in inputData)
{
    var digits = inputLine.Where(char.IsDigit).ToArray();
    calibrationValues.Add(int.Parse($"{digits.First()}{digits.Last()}"));
}

Console.WriteLine($"Part one: {calibrationValues.Sum()}");

var digitsText = new List<(string text, int value)>
{
    ("one", 1),
    ("two", 2),
    ("three", 3),
    ("four", 4),
    ("five", 5),
    ("six", 6),
    ("seven", 7),
    ("eight", 8),
    ("nine", 9)
};

calibrationValues = [];

// Overcomplicated for sure
foreach (var inputLine in inputData)
{
    int? firstDigitFromText = null;
    int firstDigitFromTextIndex = int.MaxValue;

    foreach (var (text, value) in digitsText)
    {
        int index = inputLine.IndexOf(text);
        if (index != -1 && inputLine.IndexOf(text) < firstDigitFromTextIndex)
        {
            firstDigitFromTextIndex = index;
            firstDigitFromText = value;
        }
    }

    int? lastDigitFromText = null;
    int lastDigitFromTextIndex = -1;

    foreach (var (text, value) in digitsText)
    {
        int index = inputLine.LastIndexOf(text);
        if (index > lastDigitFromTextIndex)
        {
            lastDigitFromTextIndex = index;
            lastDigitFromText = value;
        }
    }

    var digits = inputLine.Where(x => char.IsDigit(x)).ToArray();

    int firstDigit = 0;
    var firstDigitIndex = inputLine.IndexOf(digits.FirstOrDefault());
    if (firstDigitIndex == -1 || firstDigitIndex > firstDigitFromTextIndex)
    {
        firstDigit = firstDigitFromText!.Value;
    }
    else
    {
        firstDigit = int.Parse(digits.FirstOrDefault().ToString());
    }

    var lastDigit = 0;
    var lastDigitIndex = inputLine.LastIndexOf(digits.LastOrDefault());
    if (lastDigitIndex > lastDigitFromTextIndex)
    {
        lastDigit = int.Parse(digits.LastOrDefault().ToString());
    }
    else
    {
        lastDigit = lastDigitFromText!.Value;
    }

    calibrationValues.Add(int.Parse($"{firstDigit}{lastDigit}"));
}

Console.WriteLine($"Part two: {calibrationValues.Sum()}");