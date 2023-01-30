using Application.Abstractions.Gateways;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Infrastructure.EventStore.DependencyInjection.Options;
using Infrastructure.EventStore.Exceptions;
using Microsoft.Extensions.Options;

namespace Infrastructure.EventStore.Abstractions;

public class EventStoreGateway : IEventStoreGateway
{
    private readonly EventStoreOptions _options;
    private readonly IEventStoreRepository _repository;

    public EventStoreGateway(IEventStoreRepository repository, IOptionsSnapshot<EventStoreOptions> options)
    {
        _options = options.Value;
        _repository = repository;
    }

    public async Task AppendEventsAsync(IAggregateRoot aggregate, CancellationToken cancellationToken)
    {
        foreach (var @event in aggregate.UncommittedEvents.Select(@event => new StoreEvent(aggregate, @event)))
        {
            await _repository.AppendEventAsync(@event, cancellationToken);
            await AppendSnapshotAsync(aggregate, @event.Version, cancellationToken);
        }
    }

    public async Task<TAggregate> LoadAggregateAsync<TAggregate>(Guid aggregateId, CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot, new()
    {
        var snapshot = await _repository.GetSnapshotAsync(aggregateId, cancellationToken);
        var events = await _repository.GetStreamAsync(aggregateId, snapshot?.AggregateVersion, cancellationToken);

        if (snapshot is null && events is not { Count: > 0 })
            throw new AggregateNotFoundException(aggregateId, typeof(TAggregate));

        var aggregate = snapshot?.Aggregate ?? new TAggregate();

        return (TAggregate)aggregate.Load(events);
    }

    private async Task AppendSnapshotAsync(IAggregateRoot aggregate, long version, CancellationToken cancellationToken)
    {
        if (version % _options.SnapshotInterval is not 0) return;
        Snapshot snapshot = new(version, aggregate);
        await _repository.AppendSnapshotAsync(snapshot, cancellationToken);
    }

    public IAsyncEnumerable<Guid> StreamAggregatesId(CancellationToken cancellationToken)
        => _repository.GetAggregateIdsAsync(cancellationToken);
}