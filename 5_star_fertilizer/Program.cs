string path = Path.Combine(Environment.CurrentDirectory, "input.txt");

var inputData = File.ReadAllLines(path);

List<long> seeds = [];
List<SingleMapData> seedToSoilMap = [];
List<SingleMapData> soilToFertilizerMap = [];
List<SingleMapData> fertilizerToWaterMap = [];
List<SingleMapData> waterToLightMap = [];
List<SingleMapData> lightToTemperatureMap = [];
List<SingleMapData> temperatureToHumidityMap = [];
List<SingleMapData> humidityToLocationMap = [];

long minLocation = long.MaxValue;

for (int i = 0; i < inputData.Length; i++)
{
    string? line = inputData[i];

    if (line.StartsWith("seed-"))
    {
        seedToSoilMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("soil"))
    {
        soilToFertilizerMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("fertilizer"))
    {
        fertilizerToWaterMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("water"))
    {
        waterToLightMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("light"))
    {
        lightToTemperatureMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("temperature"))
    {
        temperatureToHumidityMap = GetMapData(inputData, ref i);
    }

    if (line.StartsWith("humidity"))
    {
        humidityToLocationMap = GetMapData(inputData, ref i);
    }
}

for (int i = 0; i < inputData.Length; i++)
{
    string? line = inputData[i];
    if (line.StartsWith("seeds"))
    {
        var seedsRanges = line[7..].Split(" ").Select(x => long.Parse(x)).ToList();

        for (int j = 0; j < seedsRanges.Count; j += 2)
        {
            for (long k = seedsRanges[j]; k < seedsRanges[j] + seedsRanges[j + 1]; k++)
            {
                var map = new Map();
                map.Data.Add(k);

                MapSourceToDestination(seedToSoilMap, map);
                MapSourceToDestination(soilToFertilizerMap, map);
                MapSourceToDestination(fertilizerToWaterMap, map);
                MapSourceToDestination(waterToLightMap, map);
                MapSourceToDestination(lightToTemperatureMap, map);
                MapSourceToDestination(temperatureToHumidityMap, map);
                MapSourceToDestination(humidityToLocationMap, map);

                minLocation = Math.Min(map.Data.Last(), minLocation);
            }
        }
    }
}

Console.WriteLine($"Part two: {minLocation}");

static List<SingleMapData> GetMapData(string[] inputData, ref int i)
{
    var maps = new List<SingleMapData>();
    string line;
    do
    {
        i++;
        line = inputData[i];

        if (string.IsNullOrWhiteSpace(line))
        {
            break;
        }

        var data = line.Split(" ").Select(long.Parse).ToList();
        maps.Add(new SingleMapData { Destination = data[0], Source = data[1], Range = data[2] });
    } while (i < inputData.Length - 1 && !string.IsNullOrWhiteSpace(line));

    return maps;
}

static void MapSourceToDestination(List<SingleMapData> seedToSoilMap, Map map)
{
    var lastElement = map.Data.Last();
    var matchingSoilData = seedToSoilMap.Where(x => x.Source <= lastElement && x.Source + x.Range >= lastElement).FirstOrDefault();
    if (matchingSoilData is not null)
    {
        long difference = lastElement - matchingSoilData.Source;
        map.Data.Add(matchingSoilData.Destination + difference);
    }
    else
    {
        map.Data.Add(lastElement);
    }
}

internal class SingleMapData
{
    public long Destination { get; set; }
    public long Source { get; set; }
    public long Range { get; set; }
}

internal class Map
{
    public List<long> Data { get; set; } = [];
}