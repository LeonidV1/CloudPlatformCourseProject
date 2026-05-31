namespace rut_shop.net.model;

public class Subscription
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public Guid PackageId { get; set; }

    public DateTime StartDateUtc { get; set; }

    public DateTime EndDateUtc { get; set; }

    public decimal BillingAmount { get; set; }

    public decimal DiscountApplied { get; set; }

    public string Status { get; set; } = "Active";
}
