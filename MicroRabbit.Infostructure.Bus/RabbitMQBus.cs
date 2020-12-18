﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infostructure.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator = default;
        private readonly List<Type> _eventTypes = default;
        private readonly Dictionary<string, List<Type>> _handlers = default;

        public RabbitMQBus(IMediator mediator)
        {
            _mediator = mediator;
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<Type>>();
        }

        public void Publish<T>(T eventArg) where T : Event
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            var eventName = eventArg.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            var message = JsonConvert.SerializeObject(eventArg);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", eventName, null, body);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if(!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>() { handlerType });
            }
            else if(_handlers[eventName].Any(o => o.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler type {handlerType.Name} has been already registered");
            }


            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            ConnectionFactory factory = new ConnectionFactory()
                { HostName = "localhost", DispatchConsumersAsync=true };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(typeof(T).Name, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += SubscribeReceivedAsync;


            channel.BasicConsume(typeof(T).Name, true, consumer);
        }

        private async Task SubscribeReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());
            try
            {
                await ProcessEventAsync(eventName, message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        private async Task ProcessEventAsync(string eventName, string message)
        {
            if(_handlers.ContainsKey(eventName))
            {
                var subscriptions = _handlers[eventName];
                foreach(var item in subscriptions)
                {
                    var handler = Activator.CreateInstance(item);
                    if (handler == null) continue;

                    var eventType = _eventTypes.FirstOrDefault(o => o.GetType().Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { @event });
                }
            }
        }
    }
}