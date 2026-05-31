using rut_shop.net.dto.response;
using rut_shop.net.model;

namespace rut_shop.net.dto;

public class Mapper : IMapper
{
    public ComputingPackageResponse Map(ComputingPackage package) => new()
    {
        Id = package.Id,
        Name = package.Name,
        CpuCores = package.CpuCores,
        RamGb = package.RamGb,
        StorageGb = package.StorageGb,
        PricePerMonth = package.PricePerMonth,
        IsActive = package.IsActive
    };

    public CompanyResponse Map(Company company) => new()
    {
        Id = company.Id,
        CompanyName = company.CompanyName,
        ContactEmail = company.ContactEmail,
        CreditPoints = company.CreditPoints
    };

    public CompanyCreditsResponse MapCredits(Company company) => new()
    {
        Id = company.Id,
        CompanyName = company.CompanyName,
        CreditPoints = company.CreditPoints
    };

    public SubscriptionResponse Map(Subscription subscription) => new()
    {
        Id = subscription.Id,
        CompanyId = subscription.CompanyId,
        CompanyName = subscription.CompanyName,
        PackageId = subscription.PackageId,
        StartDateUtc = subscription.StartDateUtc,
        EndDateUtc = subscription.EndDateUtc,
        BillingAmount = subscription.BillingAmount,
        DiscountApplied = subscription.DiscountApplied,
        Status = subscription.Status
    };
}
