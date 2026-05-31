namespace rut_shop.net.dto.request;

public record UpdateCompanyRequest
{
    public string CompanyName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public int CreditPoints { get; init; }
}
