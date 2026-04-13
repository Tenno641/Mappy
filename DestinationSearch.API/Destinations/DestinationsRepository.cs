namespace DestinationSearch.API.Destinations;

public class DestinationsRepository
{
    private readonly List<Destination> _destinations = [
        new Destination("Antwerp, Belgium", "", "antwerp.jpg"),
        new Destination("San Francisco, USA", "", "sanfranciso.jpg"),
        new Destination("Sydney, Australia", "", "sydney.jpg"),
        new Destination("Paris, France", "", "paris.jpg"),
        new Destination("New Delhi", "", "newdelhi.jpg"),
        new Destination("Tokyo, Japan", "", "tokyo.jpg"),
        new Destination("Barcelona, Spain", "", "barcelona.jpg"),
        new Destination("Toronto, Canada", "", "toronto.jpg")
    ];

    public List<Destination> Search(string? input) => _destinations.Where(destination => destination.DoesMatch(input)).ToList();
}