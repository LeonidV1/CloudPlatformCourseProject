namespace rut_shop.net.dto.request;

public record CreateSubscriptionRequest
{
    public Guid CompanyId { get; init; }
    public Guid PackageId { get; init; }
    public int DurationMonths { get; init; } = 1;
    public DateTime? StartDateUtc { get; init; }
}
