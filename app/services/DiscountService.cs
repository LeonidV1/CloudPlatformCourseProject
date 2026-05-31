using rut_shop.net.interfaces;
using rut_shop.net.model;

namespace rut_shop.net.services;

public class DiscountService : IDiscountService
{
    private const decimal AnnualDiscountPercent = 0.10m;
    private const int AnnualDiscountThreshold = 12;

    public decimal CalculateDiscount(int subscriptionMonths, decimal monthlyPrice)
    {
        if (subscriptionMonths >= AnnualDiscountThreshold)
        {
            var totalPrice = monthlyPrice * subscriptionMonths;
            return totalPrice * AnnualDiscountPercent;
        }

        return 0m;
    }

    public void AddCredits(Company company, int credits)
    {
        if (credits <= 0)
        {
            return;
        }

        company.CreditPoints += credits;
    }

    public void RemoveCredits(Company company, int credits)
    {
        if (credits <= 0)
        {
            return;
        }

        company.CreditPoints = Math.Max(0, company.CreditPoints - credits);
    }
}
