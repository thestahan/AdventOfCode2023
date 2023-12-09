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
            myNums.Add(long.Parse(string.Join("", newNumber)));
        }

        isCreatingNumber = false;
        newNumber = [];
    }
}

Console.WriteLine($"Part one: {enginePartNumbersSum}");

ulong gearRatios = 0;

for (int i = 0; i < inputData.Length; i++)
{
    var line = inputData[i];

    for (int j = 0; j < line.Length; j++)
    {
        if (inputData[i][j] == '*')
        {
            try
            {
                var nums = GetNumbersAdjecentToSymbol(j, i, inputData);
                Console.WriteLine($"Adding pair: {nums.number1}, {nums.number2}");
                gearRatios += (ulong)(nums.number1 * nums.number2);
            }
            catch (Exception)
            {
            }
        }
    }
}

Console.WriteLine($"Part two: {gearRatios}");

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

(ulong number1, ulong number2) GetNumbersAdjecentToSymbol(int column, int row, string[] inputData)
{
    int maxCol = inputData.First().Length;
    int maxRow = inputData.Length;

    var numbers = new List<ulong>();

    // Top
    if (row - 1 >= 0)
    {
        numbers.AddRange(GetNumbersFromRow(column, row - 1, inputData, maxCol));
    }

    // Sides
    if (column - 1 >= 0)
    {
        List<char> number = [];

        for (int i = column - 1; i >= 0; i--)
        {
            if (char.IsDigit(inputData[row][i]))
            {
                number.Add(inputData[row][i]);
            }
            else
            {
                break;
            }
        }

        if (number.Count > 0)
        {
            number.Reverse();
            numbers.Add(ulong.Parse(string.Join("", number)));
        }
    }
    if (column + 1 <= maxCol)
    {
        List<char> number = [];

        for (int i = column + 1; i < maxCol; i++)
        {
            if (char.IsDigit(inputData[row][i]))
            {
                number.Add(inputData[row][i]);
            }
            else
            {
                break;
            }
        }

        if (number.Count > 0)
        {
            numbers.Add(ulong.Parse(string.Join("", number)));
        }
    }

    // Bottom
    if (row + 1 < maxRow)
    {
        numbers.AddRange(GetNumbersFromRow(column, row + 1, inputData, maxCol));
    }

    if (numbers.Count != 2)
    {
        throw new Exception();
    }

    return (numbers[0], numbers[1]);
}

static List<ulong> GetNumbersFromRow(int column, int row, string[] inputData, int maxCol)
{
    var numbers = new List<ulong>();
    int topStartRow = row;
    int topStartCol = column;

    if (column - 1 >= 0)
    {
        topStartCol -= 1;
    }

    while (topStartCol >= 0)
    {
        if (!char.IsDigit(inputData[topStartRow][topStartCol]))
        {
            topStartCol++;

            break;
        }

        if (topStartCol - 1 < 0)
        {
            break;
        }

        topStartCol--;
    }

    List<char> number = [];

    while (char.IsDigit(inputData[topStartRow][topStartCol]))
    {
        number.Add(inputData[topStartRow][topStartCol]);
        topStartCol++;
    }

    if (number.Count > 0)
    {
        numbers.Add(ulong.Parse(string.Join("", number)));
    }

    topStartCol += 1;

    if (topStartCol < maxCol && !char.IsDigit(inputData[row][column]))
    {
        number = [];

        while (char.IsDigit(inputData[topStartRow][topStartCol]))
        {
            number.Add(inputData[topStartRow][topStartCol]);

            if (topStartCol + 1 == inputData.First().Length)
            {
                break;
            }

            topStartCol++;
        }

        if (number.Count > 0)
        {
            numbers.Add(ulong.Parse(string.Join("", number)));
        }
    }

    return numbers;
}