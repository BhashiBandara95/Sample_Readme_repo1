﻿using ECommerce.Abstractions.Messages;
using ECommerce.Abstractions.Messages.Commands;

namespace ECommerce.Contracts.Catalogs;

public static class Command
{
    public record CreateCatalog(Guid CatalogId, string Title, string Description) : Message(CorrelationId: CatalogId), ICommand;

    public record DeleteCatalog(Guid CatalogId) : Message(CorrelationId: CatalogId), ICommand;

    public record ChangeCatalogTitle(Guid CatalogId, string Title) : Message(CorrelationId: CatalogId), ICommand;

    public record ChangeCatalogDescription(Guid CatalogId, string Description) : Message(CorrelationId: CatalogId), ICommand;

    public record ActivateCatalog(Guid CatalogId) : Message(CorrelationId: CatalogId), ICommand;

    public record DeactivateCatalog(Guid CatalogId) : Message(CorrelationId: CatalogId), ICommand;

    public record DeleteCatalogItem(Guid CatalogId, Guid CatalogItemId) : Message(CorrelationId: CatalogId), ICommand;

    public record AddCatalogItem(Guid CatalogId, string Name, string Description, decimal Price, string PictureUri) : Message(CorrelationId: CatalogId), ICommand;

    public record UpdateCatalogItem(Guid CatalogId, Guid CatalogItemId, string Name, string Description, decimal Price, string PictureUri) : Message(CorrelationId: CatalogId), ICommand;
}