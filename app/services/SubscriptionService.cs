using Microsoft.EntityFrameworkCore;
using rut_shop.net.database;
using rut_shop.net.dto.request;
using rut_shop.net.interfaces;
using rut_shop.net.model;

namespace rut_shop.net.services;

public class SubscriptionService(CloudPlatformDbContext db, IDiscountService discountService) : ISubscriptionService
{
    public async Task<IReadOnlyList<Subscription>> GetAllAsync()
        => await db.Subscriptions
            .AsNoTracking()
            .OrderByDescending(x => x.StartDateUtc)
            .ToListAsync();

    public async Task<Subscription?> GetByIdAsync(Guid id)
        => await db.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Subscription> CreateAsync(CreateSubscriptionRequest request)
    {
        ValidateSubscriptionRequest(request);

        var company = await db.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);
        if (company is null)
        {
            throw new InvalidOperationException("Компания не найдена.");
        }

        var package = await db.ComputingPackages
            .FirstOrDefaultAsync(x => x.Id == request.PackageId);
        if (package is null)
        {
            throw new InvalidOperationException("Пакет вычислений не найден.");
        }

        if (!package.IsActive)
        {
            throw new InvalidOperationException("Выбранный пакет более не доступен.");
        }

        var startDate = request.StartDateUtc ?? DateTime.UtcNow;
        var endDate = startDate.AddMonths(request.DurationMonths);
        var baseAmount = package.PricePerMonth * request.DurationMonths;
        var discount = discountService.CalculateDiscount(request.DurationMonths, package.PricePerMonth);
        var billingAmount = baseAmount - discount;

        var earnedCredits = CalculateCreditsEarned(request.DurationMonths);
        discountService.AddCredits(company, earnedCredits);

        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            CompanyName = company.CompanyName,
            PackageId = package.Id,
            StartDateUtc = startDate,
            EndDateUtc = endDate,
            BillingAmount = billingAmount,
            DiscountApplied = discount,
            Status = "Active"
        };

        db.Subscriptions.Add(subscription);
        await db.SaveChangesAsync();

        return subscription;
    }

    public async Task<Subscription?> UpdateAsync(Guid id, UpdateSubscriptionRequest request)
    {
        var subscription = await db.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id);
        if (subscription is null)
        {
            return null;
        }

        if (request.Status != null && 
            new[] { "Active", "Expired", "Cancelled" }.Contains(request.Status))
        {
            subscription.Status = request.Status;
        }

        await db.SaveChangesAsync();
        return subscription;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var subscription = await db.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id);
        if (subscription is null)
        {
            return false;
        }

        subscription.Status = "Cancelled";
        await db.SaveChangesAsync();
        return true;
    }

    private static void ValidateSubscriptionRequest(CreateSubscriptionRequest request)
    {
        if (request.CompanyId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор компании не должен быть пустым.");
        }

        if (request.PackageId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор пакета не должен быть пустым.");
        }

        if (request.DurationMonths <= 0)
        {
            throw new ArgumentException("Длительность подписки должна быть положительной.");
        }
    }
    
    private static int CalculateCreditsEarned(int durationMonths)
    {
        return durationMonths * 10;
    }
}
