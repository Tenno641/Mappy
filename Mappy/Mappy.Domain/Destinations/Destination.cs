namespace Mappy.Domain.Destinations;

// Not used - Destination Response return from the message queue is.
public class Destination
{
    private string Name { get; }
    private string? Description { get; }
    private string? ImageRootUri { get; }
    private string? ImageName { get; }
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

    public bool DoesMatch(string? input) => string.IsNullOrEmpty(input) || Name.Contains(input) || (Description?.Contains(input) ?? false);
}