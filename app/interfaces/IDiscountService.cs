using rut_shop.net.model;

namespace rut_shop.net.interfaces;

public interface IDiscountService
{
    decimal CalculateDiscount(int subscriptionMonths, decimal monthlyPrice);

    void AddCredits(Company company, int credits);

    void RemoveCredits(Company company, int credits);
}
