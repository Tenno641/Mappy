namespace DestinationSearch.API.Destinations;

public class Destination
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImageRootUri { get; private set; }
    public string? ImageName { get; private set; }
    public Uri? ImageUri => ImageRootUri is null || ImageName is null
        ? null
        : new Uri(ImageRootUri + ImageName);

    public Destination(string name, string? description = null, string? imageRootUri = null, string? imageName = null)
    {
        Name = name;
        Description = description;
        ImageRootUri = imageRootUri;
        ImageName = imageName;
    }

    public bool DoesMatch(string? input) => string.IsNullOrEmpty(input) || Name.Contains(input, StringComparison.InvariantCultureIgnoreCase) || (Description is not null && Description.Contains(input, StringComparison.InvariantCultureIgnoreCase));
}