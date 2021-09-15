using Application.EventSourcing.EventStore.Events;
using Domain.Abstractions.Events;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Infrastructure.EventSourcing.Catalogs.EventStore.Configurations
{
    public class CatalogStoreEventConfiguration : IEntityTypeConfiguration<CatalogStoreEvent>
    {
        public void Configure(EntityTypeBuilder<CatalogStoreEvent> builder)
        {
            builder.HasKey(storeEvent => storeEvent.Version);

            builder
                .Property(storeEvent => storeEvent.AggregateId)
                .IsRequired();

            builder
                .Property(storeEvent => storeEvent.AggregateName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(storeEvent => storeEvent.DomainEventName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(storeEvent => storeEvent.DomainEvent)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasConversion(
                    domainEvent => JsonConvert.SerializeObject(domainEvent,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
                    jsonString => JsonConvert.DeserializeObject<IDomainEvent>(jsonString,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ContractResolver = new PrivateSetterContractResolver() }))
                .IsRequired();
        }
    }
}