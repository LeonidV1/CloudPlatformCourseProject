namespace rut_shop.net.model;

public class Company
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public int CreditPoints { get; set; }
}
