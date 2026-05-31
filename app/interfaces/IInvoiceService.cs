using rut_shop.net.model;

namespace rut_shop.net.interfaces;

public interface IInvoiceService
{
    Invoice BuildInvoice(Subscription subscription);
}
