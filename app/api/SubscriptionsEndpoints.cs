using rut_shop.net.dto;
using rut_shop.net.dto.request;
using rut_shop.net.dto.response;
using rut_shop.net.interfaces;

namespace rut_shop.net.api;

public static class SubscriptionsEndpoints
{
    public static RouteGroupBuilder MapSubscriptionsEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/subscriptions").WithTags("Subscriptions");

        group.MapGet("/", async (ISubscriptionService subscriptions, IMapper mapper) =>
            {
                var result = await subscriptions.GetAllAsync();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список подписок")
            .WithDescription("Возвращает историю подписок: компании, пакеты, даты, скидки и статусы.")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{subscriptionId:guid}/invoice",
                async (Guid subscriptionId, ISubscriptionService subscriptions, IInvoiceService invoices) =>
            {
                var subscription = await subscriptions.GetByIdAsync(subscriptionId);
                if (subscription is null)
                {
                    return Results.NotFound(new ErrorResponse { Message = "Подписка не найдена." });
                }

                var invoice = invoices.BuildInvoice(subscription);
                return Results.File(invoice.Content, invoice.ContentType, invoice.FileName);
            })
            .WithSummary("Скачать счет по подписке")
            .WithDescription("Формирует и отдает текстовый счет в виде файла .txt с полной информацией о подписке.")
            .Produces(StatusCodes.Status200OK, contentType: "text/plain")
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{subscriptionId:guid}",
                async (Guid subscriptionId, ISubscriptionService subscriptions, IMapper mapper) =>
            {
                var subscription = await subscriptions.GetByIdAsync(subscriptionId);
                return subscription is null
                    ? Results.NotFound(new ErrorResponse { Message = "Подписка не найдена." })
                    : Results.Ok(mapper.Map(subscription));
            })
            .WithSummary("Получить подписку по идентификатору")
            .WithDescription("Возвращает одну подписку: компания, пакет, период, сумму с учетом скидки и статус.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateSubscriptionRequest request, ISubscriptionService subscriptions, IMapper mapper) =>
            {
                try
                {
                    var subscription = await subscriptions.CreateAsync(request);
                    return Results.Created($"/api/subscriptions/{subscription.Id}", mapper.Map(subscription));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Создать подписку")
            .WithDescription("Создаёт подписку компании на пакет: проверяет данные, вычисляет скидку за годовой контракт, начисляет кредиты.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{subscriptionId:guid}",
                async (Guid subscriptionId, UpdateSubscriptionRequest request, ISubscriptionService subscriptions, IMapper mapper) =>
            {
                try
                {
                    var subscription = await subscriptions.UpdateAsync(subscriptionId, request);
                    return subscription is null
                        ? Results.NotFound(new ErrorResponse { Message = "Подписка не найдена." })
                        : Results.Ok(mapper.Map(subscription));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Изменить подписку")
            .WithDescription("Обновляет статус подписки (Active, Expired, Cancelled).")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{subscriptionId:guid}",
                async (Guid subscriptionId, ISubscriptionService subscriptions) =>
            {
                var deleted = await subscriptions.DeleteAsync(subscriptionId);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound(new ErrorResponse { Message = "Подписка не найдена." });
            })
            .WithSummary("Удалить подписку")
            .WithDescription("Отменяет подписку и устанавливает статус Cancelled.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }
}
