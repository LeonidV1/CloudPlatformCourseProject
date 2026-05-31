using Microsoft.EntityFrameworkCore;
using rut_shop.net.database;
using rut_shop.net.dto.request;
using rut_shop.net.interfaces;
using rut_shop.net.model;

namespace rut_shop.net.services;

public class CompanyService(CloudPlatformDbContext db) : ICompanyService
{
    public async Task<IReadOnlyList<Company>> GetAllAsync()
        => await db.Companies
            .AsNoTracking()
            .OrderBy(x => x.CompanyName)
            .ToListAsync();

    public async Task<Company?> GetByIdAsync(Guid id)
        => await db.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Company> AddAsync(CreateCompanyRequest request)
    {
        ValidateCompanyFields(request.CompanyName, request.ContactEmail);

        var id = request.Id ?? Guid.NewGuid();
        if (await db.Companies.AnyAsync(x => x.Id == id))
        {
            throw new InvalidOperationException($"Компания с идентификатором {id} уже существует.");
        }

        var entity = new Company
        {
            Id = id,
            CompanyName = request.CompanyName.Trim(),
            ContactEmail = request.ContactEmail.Trim(),
            CreditPoints = Math.Max(0, request.CreditPoints)
        };

        db.Companies.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<Company?> UpdateAsync(Guid id, UpdateCompanyRequest request)
    {
        ValidateCompanyFields(request.CompanyName, request.ContactEmail);

        var entity = await db.Companies.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return null;
        }

        entity.CompanyName = request.CompanyName.Trim();
        entity.ContactEmail = request.ContactEmail.Trim();
        entity.CreditPoints = Math.Max(0, request.CreditPoints);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await db.Companies.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        var hasActiveSubscriptions = await db.Subscriptions
            .AnyAsync(s => s.CompanyId == id && s.Status == "Active");
        if (hasActiveSubscriptions)
        {
            throw new InvalidOperationException(
                "Нельзя удалить компанию: есть активные подписки.");
        }

        db.Companies.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    private static void ValidateCompanyFields(string companyName, string contactEmail)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentException("Название компании не должно быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(contactEmail))
        {
            throw new ArgumentException("Email контакта не должен быть пустым.");
        }
    }
}
