namespace rut_shop.net.dto.response;

public record CompanyCreditsResponse
{
    public Guid Id { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public int CreditPoints { get; init; }
}
