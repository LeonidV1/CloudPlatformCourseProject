using System.Text;
using rut_shop.net.interfaces;
using rut_shop.net.model;

namespace rut_shop.net.services;

public class InvoiceService : IInvoiceService
{
    public Invoice BuildInvoice(Subscription subscription)
    {
        var builder = new StringBuilder();
        builder.AppendLine("CLOUD COMPUTING PLATFORM");
        builder.AppendLine("СЧЕТ");
        builder.AppendLine(new string('-', 50));
        builder.AppendLine($"Номер подписки: {subscription.Id}");
        builder.AppendLine($"Дата начала (UTC): {subscription.StartDateUtc:yyyy-MM-dd HH:mm:ss}");
        builder.AppendLine($"Дата окончания (UTC): {subscription.EndDateUtc:yyyy-MM-dd HH:mm:ss}");
        builder.AppendLine($"Компания: {subscription.CompanyName}");
        builder.AppendLine(new string('-', 50));
        builder.AppendLine($"Статус: {subscription.Status}");
        builder.AppendLine(new string('-', 50));
        builder.AppendLine($"Сумма до скидки: ${subscription.BillingAmount + subscription.DiscountApplied:F2}");
        
        if (subscription.DiscountApplied > 0)
        {
            builder.AppendLine($"Скидка: -${subscription.DiscountApplied:F2}");
        }
        
        builder.AppendLine($"ИТОГО: ${subscription.BillingAmount:F2}");
        builder.AppendLine(new string('-', 50));
        builder.AppendLine("Спасибо за использование нашей платформы!");
        builder.AppendLine("Для поддержки: support@cloudcompute.example");

        return new Invoice
        {
            FileName = $"invoice-{subscription.Id}.txt",
            ContentType = "text/plain; charset=utf-8",
            Content = Encoding.UTF8.GetBytes(builder.ToString())
        };
    }
}
