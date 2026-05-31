using rut_shop.net.dto.request;
using rut_shop.net.model;

namespace rut_shop.net.interfaces;

public interface IComputingPackageService
{
    Task<IReadOnlyList<ComputingPackage>> GetAllAsync();

    Task<ComputingPackage?> GetByIdAsync(Guid id);

    Task<ComputingPackage> AddAsync(CreateComputingPackageRequest request);

    Task<ComputingPackage?> UpdateAsync(Guid id, UpdateComputingPackageRequest request);

    Task<bool> DeactivateAsync(Guid id);
}
