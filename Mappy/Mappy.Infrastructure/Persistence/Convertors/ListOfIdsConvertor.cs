using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mappy.Infrastructure.Persistence.Convertors;

public class ListOfIdsConvertor: ValueConverter<List<Guid>, string>
{
    public ListOfIdsConvertor() : base(
        value => string.Join(',', value),
        value => value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList())
    {
    }
}