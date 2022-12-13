using Application.Abstractions;
using Contracts.Services.Catalog;
using Infrastructure.EventBus.Abstractions;

namespace Infrastructure.EventBus.Consumers;

public class CatalogItemRemovedConsumer : Consumer<DomainEvent.CatalogItemRemoved>
{
    public CatalogItemRemovedConsumer(IInteractor<DomainEvent.CatalogItemRemoved> interactor)
        : base(interactor) { }
}