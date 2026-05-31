using rut_shop.net.dto;
using rut_shop.net.dto.request;
using rut_shop.net.dto.response;
using rut_shop.net.interfaces;

namespace rut_shop.net.api;

public static class CompaniesEndpoints
{
    public static RouteGroupBuilder MapCompaniesEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/companies").WithTags("Companies");

        group.MapGet("/", async (ICompanyService companies, IMapper mapper) =>
            {
                var result = await companies.GetAllAsync();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список компаний")
            .WithDescription("Возвращает все зарегистрированные компании-клиенты: названия, контакты и текущий баланс кредитов.")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{companyId:guid}",
                async (Guid companyId, ICompanyService companies, IMapper mapper) =>
            {
                var company = await companies.GetByIdAsync(companyId);
                return company is null
                    ? Results.NotFound(new ErrorResponse { Message = "Компания не найдена." })
                    : Results.Ok(mapper.Map(company));
            })
            .WithSummary("Получить компанию по идентификатору")
            .WithDescription("Возвращает одну компанию: название, контактный email и баланс кредитов.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{companyId:guid}/credits",
                async (Guid companyId, ICompanyService companies, IMapper mapper) =>
            {
                var company = await companies.GetByIdAsync(companyId);
                return company is null
                    ? Results.NotFound(new ErrorResponse { Message = "Компания не найдена." })
                    : Results.Ok(mapper.MapCredits(company));
            })
            .WithSummary("Получить баланс кредитов компании")
            .WithDescription("Возвращает только информацию о кредитах: id, название компании и текущий баланс.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateCompanyRequest request, ICompanyService companies, IMapper mapper) =>
            {
                try
                {
                    var company = await companies.AddAsync(request);
                    return Results.Created($"/api/companies/{company.Id}", mapper.Map(company));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Создать компанию")
            .WithDescription("Регистрирует новую компанию-клиента с названием и контактным email.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{companyId:guid}",
                async (Guid companyId, UpdateCompanyRequest request, ICompanyService companies, IMapper mapper) =>
            {
                try
                {
                    var company = await companies.UpdateAsync(companyId, request);
                    return company is null
                        ? Results.NotFound(new ErrorResponse { Message = "Компания не найдена." })
                        : Results.Ok(mapper.Map(company));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Изменить данные компании")
            .WithDescription("Обновляет название и контактный email компании.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{companyId:guid}",
                async (Guid companyId, ICompanyService companies) =>
            {
                try
                {
                    var deleted = await companies.DeleteAsync(companyId);
                    return deleted
                        ? Results.NoContent()
                        : Results.NotFound(new ErrorResponse { Message = "Компания не найдена." });
                }
                catch (Exception ex) when (ex is InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Удалить компанию")
            .WithDescription("Удаляет компанию. Нельзя удалить компанию, у которой есть активные подписки.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }
}
