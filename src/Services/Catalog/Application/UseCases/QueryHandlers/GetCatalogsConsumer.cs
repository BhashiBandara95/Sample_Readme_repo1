using Application.EventSourcing.Projections;
using ECommerce.Abstractions.Messages.Queries.Responses;
using ECommerce.Contracts.Catalogs;
using MassTransit;

namespace Application.UseCases.QueryHandlers;

public class GetCatalogsConsumer : IConsumer<Queries.GetCatalogs>
{
    private readonly ICatalogProjectionsService _projectionsService;

    public GetCatalogsConsumer(ICatalogProjectionsService projectionsService)
    {
        _projectionsService = projectionsService;
    }

    public async Task Consume(ConsumeContext<Queries.GetCatalogs> context)
    {
        var catalogs = await _projectionsService.GetCatalogsAsync(context.Message.Limit, context.Message.Offset, context.CancellationToken);

        await (catalogs is null
            ? context.RespondAsync<NotFound>(new())
            : context.RespondAsync<Responses.Catalogs>(catalogs));
    }
}