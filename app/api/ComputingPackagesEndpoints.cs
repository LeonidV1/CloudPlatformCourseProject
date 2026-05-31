using rut_shop.net.dto;
using rut_shop.net.dto.request;
using rut_shop.net.dto.response;
using rut_shop.net.interfaces;

namespace rut_shop.net.api;

public static class ComputingPackagesEndpoints
{
    public static RouteGroupBuilder MapComputingPackagesEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/packages").WithTags("Computing Packages");

        group.MapGet("/", async (IComputingPackageService packages, IMapper mapper) =>
            {
                var result = await packages.GetAllAsync();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список пакетов")
            .WithDescription("Возвращает все доступные вычислительные пакеты с их характеристиками: CPU, RAM, Storage и ежемесячную цену.")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{packageId:guid}",
                async (Guid packageId, IComputingPackageService packages, IMapper mapper) =>
            {
                var package = await packages.GetByIdAsync(packageId);
                return package is null
                    ? Results.NotFound(new ErrorResponse { Message = "Пакет не найден." })
                    : Results.Ok(mapper.Map(package));
            })
            .WithSummary("Получить пакет по идентификатору")
            .WithDescription("Возвращает один пакет: название, характеристики (CPU, RAM, Storage) и цену.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateComputingPackageRequest request, IComputingPackageService packages, IMapper mapper) =>
            {
                try
                {
                    var package = await packages.AddAsync(request);
                    return Results.Created($"/api/packages/{package.Id}", mapper.Map(package));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Создать вычислительный пакет")
            .WithDescription("Создаёт новый пакет с указанными параметрами: CPU, RAM, Storage и месячной ценой.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{packageId:guid}",
                async (Guid packageId, UpdateComputingPackageRequest request, IComputingPackageService packages, IMapper mapper) =>
            {
                try
                {
                    var package = await packages.UpdateAsync(packageId, request);
                    return package is null
                        ? Results.NotFound(new ErrorResponse { Message = "Пакет не найден." })
                        : Results.Ok(mapper.Map(package));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Изменить пакет")
            .WithDescription("Обновляет параметры пакета: характеристики и цену. Проверяет валидность данных.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{packageId:guid}",
                async (Guid packageId, IComputingPackageService packages) =>
            {
                try
                {
                    var deleted = await packages.DeactivateAsync(packageId);
                    return deleted
                        ? Results.NoContent()
                        : Results.NotFound(new ErrorResponse { Message = "Пакет не найден." });
                }
                catch (Exception ex) when (ex is InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Деактивировать пакет")
            .WithDescription("Деактивирует пакет. Нельзя деактивировать пакет, который используется в активных подписках.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }
}
