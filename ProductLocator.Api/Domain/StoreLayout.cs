namespace ProductLocator.Api.Domain;

public class StoreLayout
{
    public int StoreId { get; set; }
    public Store Store { get; set; } = null!;

    public int Width { get; set; }
    public int Height { get; set; }

    public string ElementsJson { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }
}
