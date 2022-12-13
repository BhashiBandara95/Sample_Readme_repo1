using Application.Abstractions;
using Contracts.Services.Catalog;
using Infrastructure.EventBus.Abstractions;

namespace Infrastructure.EventBus.Consumers;

public class CatalogDeletedConsumer : Consumer<DomainEvent.CatalogDeleted>
{
    public CatalogDeletedConsumer(IInteractor<DomainEvent.CatalogDeleted> interactor)
        : base(interactor) { }
}