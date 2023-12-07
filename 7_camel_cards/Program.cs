string path = Path.Combine(Environment.CurrentDirectory, "input.txt");
var inputData = File.ReadAllLines(path);

var hands = new List<Hand>();

foreach (var line in inputData)
{
    var data = line.Split(' ');
    var cards = data[0].Trim().ToList();
    var bid = long.Parse(data[1].Trim());

    hands.Add(new Hand { CardsOnHand = cards, Bid = bid });
}

long partOne = 0;

var orderedHands = hands.Order().ToList();

for (int i = 1; i <= orderedHands.Count; i++)
{
    partOne += orderedHands[i - 1].Bid * i;
}

Console.WriteLine($"Part two: {partOne}");

internal class Hand : IComparable<Hand>
{
    private readonly List<char> _order = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

    public List<char> CardsOnHand { get; set; } = [];
    public long Bid { get; set; }

    public int CompareTo(Hand? other)
    {
        if (CardsKind < other.CardsKind)
        {
            return 1;
        }
        else if (CardsKind > other.CardsKind)
        {
            return -1;
        }

        for (int i = 0; i < CardsOnHand.Count; i++)
        {
            if (_order.IndexOf(CardsOnHand[i]) < _order.IndexOf(other.CardsOnHand[i]))
            {
                return 1;
            }
            else if (_order.IndexOf(CardsOnHand[i]) > _order.IndexOf(other.CardsOnHand[i]))
            {
                return -1;
            }
        }

        return 0;
    }

    public CardsKind CardsKind => GetCardsKind();

    private CardsKind GetCardsKind()
    {
        var groupedCards = CardsOnHand.GroupBy(x => x);

        if (CardsOnHand.All(x => x.Equals(CardsOnHand[0])) || CardsOnHand.Contains('J') && CardsOnHand.Distinct().Count() == 2)
        {
            return CardsKind.FiveOfKind;
        }
        else if (groupedCards.Any(group => group.Count() == 4) ||
                 groupedCards.Any(group => group.Count() == 3 && group.Key != 'J' && CardsOnHand.Contains('J')) || // 2333J
                 groupedCards.Any(group => group.Count() == 2 && group.Key != 'J' && CardsOnHand.Count(x => x == 'J') == 2) || // 233JJ
                 groupedCards.Any(group => group.Count() == 1 && group.Key != 'J' && CardsOnHand.Count(x => x == 'J') == 3)) // 23JJJ
        {
            return CardsKind.FourOfKind;
        }
        else if (groupedCards.Any(group => group.Count() == 3) && groupedCards.Any(group => group.Count() == 2) ||
                 (groupedCards.Where(group => group.Count() == 2).Count() == 2) && CardsOnHand.Contains('J')) // two pairs and joker
        {
            return CardsKind.FullHouse;
        }
        else if (groupedCards.Any(group => group.Count() == 3) ||
                 groupedCards.Any(group => group.Count() == 2) && CardsOnHand.Contains('J')) // one pair and joker
        {
            return CardsKind.ThreeOfKind;
        }
        else if (groupedCards.Where(group => group.Count() == 2).Count() == 2) // one joker here means three kind is always better than two pair
        {
            return CardsKind.TwoPair;
        }
        else if (groupedCards.Any(group => group.Count() == 2) || CardsOnHand.Contains('J')) // one pair or one joker
        {
            return CardsKind.OnePair;
        }
        else
        {
            return CardsKind.HighCard;
        }
    }
}

internal enum CardsKind
{
    FiveOfKind,
    FourOfKind,
    FullHouse,
    ThreeOfKind,
    TwoPair,
    OnePair,
    HighCard
}