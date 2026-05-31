using rut_shop.net.dto.request;
using rut_shop.net.model;

namespace rut_shop.net.interfaces;

public interface ICompanyService
{
    Task<IReadOnlyList<Company>> GetAllAsync();

    Task<Company?> GetByIdAsync(Guid id);

    Task<Company> AddAsync(CreateCompanyRequest request);

    Task<Company?> UpdateAsync(Guid id, UpdateCompanyRequest request);

    Task<bool> DeleteAsync(Guid id);
}
