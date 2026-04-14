using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mappy.Infrastructure.Persistence.Convertors;

public class UriConvertor: ValueConverter<Uri, string>
{
    public UriConvertor() : base(
        value => value.ToString(),
        value => new Uri(value))
    {
    }
}