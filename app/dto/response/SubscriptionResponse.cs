namespace rut_shop.net.dto.response;

public record SubscriptionResponse
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public Guid PackageId { get; init; }
    public DateTime StartDateUtc { get; init; }
    public DateTime EndDateUtc { get; init; }
    public decimal BillingAmount { get; init; }
    public decimal DiscountApplied { get; init; }
    public string Status { get; init; } = string.Empty;
}
