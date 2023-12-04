string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

int maxRows = inputData.Length - 1;
int maxCols = inputData.First().Length - 1;

long enginePartNumbersSum = 0;

var myNums = new List<long>();

for (int i = 0; i < inputData.Length; i++)
{
    string? line = inputData[i];
    var lineNumbers = line.Split('.').Where(x => x.All(Char.IsDigit) && !string.IsNullOrEmpty(x)).ToList();
    var symbols = new List<char> { '*', '#', '+', '$', '/', '=', '&', '@', '%', '-' };

    bool isCreatingNumber = false;
    var newNumber = new List<long>();
    for (int j = 0; j < line.Length; j++)
    {
        bool isNumber = char.IsNumber(line[j]);

        if (isNumber && !isCreatingNumber)
        {
            isCreatingNumber = true;
            newNumber = [long.Parse(line[j].ToString())];
        }
        else if (isNumber && isCreatingNumber)
        {
            newNumber.Add(long.Parse(line[j].ToString()));
        }
        else if (!isNumber && isCreatingNumber)
        {
            bool isAdjacentToSymbol = IsNumberAdjacentToSymbol(j - newNumber.Count, j - 1, i, maxRows, maxCols, inputData, symbols);

            if (isAdjacentToSymbol)
            {
                enginePartNumbersSum += long.Parse(string.Join("", newNumber));
                Console.Write($" | {long.Parse(string.Join("", newNumber))} | ");
                myNums.Add(long.Parse(string.Join("", newNumber)));
            }

            isCreatingNumber = false;
            newNumber = [];
        }
    }

    if (isCreatingNumber)
    {
        bool isAdjacentToSymbol = IsNumberAdjacentToSymbol(line.Length - newNumber.Count, line.Length - 1, i, maxRows, maxCols, inputData, symbols);

        if (isAdjacentToSymbol)
        {
            enginePartNumbersSum += int.Parse(string.Join("", newNumber));

            Console.Write($" | {int.Parse(string.Join("", newNumber))} | ");
            myNums.Add(long.Parse(string.Join("", newNumber)));
        }

        isCreatingNumber = false;
        newNumber = [];
    }

    Console.WriteLine();
}

Console.WriteLine($"Part one: {enginePartNumbersSum}");

bool IsNumberAdjacentToSymbol(int startColumn, int endColumn, int row, int maxRows, int maxColumns, string[] inputData, List<char> symbols)
{
    // Check top
    if (row - 1 >= 0)
    {
        int topStartRow = row - 1;
        int topStartCol = startColumn;
        int topEndCol = endColumn;

        if (startColumn - 1 >= 0)
        {
            topStartCol = startColumn - 1;
        }
        if (endColumn + 1 <= maxColumns)
        {
            topEndCol = endColumn + 1;
        }

        for (int i = topStartCol; i <= topEndCol; i++)
        {
            if (symbols.Contains(inputData[topStartRow][i]))
            {
                return true;
            }
        }
    }

    // Check sides
    if (startColumn - 1 >= 0)
    {
        if (symbols.Contains(inputData[row][startColumn - 1]))
        {
            return true;
        }
    }
    if (endColumn + 1 <= maxColumns)
    {
        if (symbols.Contains(inputData[row][endColumn + 1]))
        {
            return true;
        }
    }

    // Check bottom
    if (row + 1 <= maxRows)
    {
        int bottomStartRow = row + 1;
        int bottomStartCol = startColumn;
        int bottomEndCol = endColumn;

        if (startColumn - 1 >= 0)
        {
            bottomStartCol = startColumn - 1;
        }
        if (endColumn + 1 <= maxColumns)
        {
            bottomEndCol = endColumn + 1;
        }

        for (int i = bottomStartCol; i <= bottomEndCol; i++)
        {
            if (symbols.Contains(inputData[bottomStartRow][i]))
            {
                return true;
            }
        }
    }

    return false;
}