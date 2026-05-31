using rut_shop.net.dto.response;
using rut_shop.net.model;

namespace rut_shop.net.dto;

public interface IMapper
{
    ComputingPackageResponse Map(ComputingPackage package);

    CompanyResponse Map(Company company);

    CompanyCreditsResponse MapCredits(Company company);

    SubscriptionResponse Map(Subscription subscription);
}
