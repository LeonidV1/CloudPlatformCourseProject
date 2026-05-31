namespace rut_shop.net.dto.request;

public record CreateCompanyRequest
{
    public Guid? Id { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public int CreditPoints { get; init; }
}
