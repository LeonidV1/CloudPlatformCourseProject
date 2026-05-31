using rut_shop.net.dto.request;
using rut_shop.net.model;

namespace rut_shop.net.interfaces;

public interface ISubscriptionService
{
    Task<IReadOnlyList<Subscription>> GetAllAsync();

    Task<Subscription?> GetByIdAsync(Guid id);

    Task<Subscription> CreateAsync(CreateSubscriptionRequest request);

    Task<Subscription?> UpdateAsync(Guid id, UpdateSubscriptionRequest request);

    Task<bool> DeleteAsync(Guid id);
}
