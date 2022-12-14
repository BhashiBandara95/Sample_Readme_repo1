﻿using Application.Abstractions;
using Application.Services;
using Contracts.Services.Catalog;
using Domain.Aggregates;

namespace Application.UseCases.Commands;

public interface IChangeCatalogDescriptionInteractor : IInteractor<Command.ChangeCatalogDescription> { }

public class ChangeCatalogDescriptionInteractor : IChangeCatalogDescriptionInteractor
{
    private readonly IApplicationService _applicationService;

    public ChangeCatalogDescriptionInteractor(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task InteractAsync(Command.ChangeCatalogDescription command, CancellationToken cancellationToken)
    {
        var catalog = await _applicationService.LoadAggregateAsync<Catalog>(command.CatalogId, cancellationToken);
        catalog.Handle(command);
        await _applicationService.AppendEventsAsync(catalog, cancellationToken);
    }
}