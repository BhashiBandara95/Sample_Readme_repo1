﻿using Contracts.Abstractions.Messages;
using Infrastructure.MessageBus.Consumers.Events;
using MassTransit;
using Identity = Contracts.Services.Identity;
using Communication = Contracts.Services.Communication;

namespace Infrastructure.MessageBus.DependencyInjection.Extensions;

internal static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        ConfigureEventReceiveEndpoint<UserRegisteredConsumer, Identity.DomainEvent.UserRegistered>(cfg, context);
        ConfigureEventReceiveEndpoint<NotificationRequestedConsumer, Communication.DomainEvent.NotificationRequested>(cfg, context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
        where TConsumer : class, IConsumer
        where TEvent : class, IEvent
        => bus.ReceiveEndpoint(
            queueName: $"communication.command.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
            configureEndpoint: endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind<TEvent>();
                endpoint.ConfigureConsumer<TConsumer>(context);
            });
}