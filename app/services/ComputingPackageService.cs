using Microsoft.EntityFrameworkCore;
using rut_shop.net.database;
using rut_shop.net.dto.request;
using rut_shop.net.interfaces;
using rut_shop.net.model;

namespace rut_shop.net.services;

public class ComputingPackageService(CloudPlatformDbContext db) : IComputingPackageService
{
    public async Task<IReadOnlyList<ComputingPackage>> GetAllAsync()
        => await db.ComputingPackages
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<ComputingPackage?> GetByIdAsync(Guid id)
        => await db.ComputingPackages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ComputingPackage> AddAsync(CreateComputingPackageRequest request)
    {
        ValidatePackageFields(request.Name, request.CpuCores, request.RamGb, request.StorageGb, request.PricePerMonth);

        var id = request.Id ?? Guid.NewGuid();
        if (await db.ComputingPackages.AnyAsync(x => x.Id == id))
        {
            throw new InvalidOperationException($"Пакет с идентификатором {id} уже существует.");
        }

        var entity = new ComputingPackage
        {
            Id = id,
            Name = request.Name.Trim(),
            CpuCores = request.CpuCores,
            RamGb = request.RamGb,
            StorageGb = request.StorageGb,
            PricePerMonth = request.PricePerMonth,
            IsActive = true
        };

        db.ComputingPackages.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<ComputingPackage?> UpdateAsync(Guid id, UpdateComputingPackageRequest request)
    {
        ValidatePackageFields(request.Name, request.CpuCores, request.RamGb, request.StorageGb, request.PricePerMonth);

        var entity = await db.ComputingPackages.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return null;
        }

        entity.Name = request.Name.Trim();
        entity.CpuCores = request.CpuCores;
        entity.RamGb = request.RamGb;
        entity.StorageGb = request.StorageGb;
        entity.PricePerMonth = request.PricePerMonth;
        entity.IsActive = request.IsActive;
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var entity = await db.ComputingPackages.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        var usedInSubscription = await db.Subscriptions
            .AnyAsync(s => s.PackageId == id && s.Status == "Active");
        if (usedInSubscription)
        {
            throw new InvalidOperationException(
                "Нельзя деактивировать пакет: он используется в активных подписках.");
        }

        entity.IsActive = false;
        await db.SaveChangesAsync();
        return true;
    }

    private static void ValidatePackageFields(string name, int cpuCores, int ramGb, int storageGb, decimal pricePerMonth)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название пакета не должно быть пустым.");
        }

        if (cpuCores <= 0)
        {
            throw new ArgumentException("Количество CPU ядер должно быть положительным.");
        }

        if (ramGb <= 0)
        {
            throw new ArgumentException("Объем ОЖП должен быть положительным.");
        }

        if (storageGb <= 0)
        {
            throw new ArgumentException("Объем хранилища должен быть положительным.");
        }

        if (pricePerMonth < 0)
        {
            throw new ArgumentException("Цена не может быть отрицательной.");
        }
    }
}
    

