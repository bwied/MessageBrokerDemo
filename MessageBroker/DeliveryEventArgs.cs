using System;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class DeliveryEventArgs : EventArgs
    {
        /// <summary>Default constructor.</summary>
        public DeliveryEventArgs()
        {
        }

        /// <summary>Constructor that fills the event's properties from
        /// its arguments.</summary>
        public DeliveryEventArgs(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            byte[] body)
        {
            this.ConsumerTag = consumerTag;
            this.DeliveryTag = deliveryTag;
            this.Redelivered = redelivered;
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
            this.BasicProperties = properties;
            this.Body = body;
        }

        /// <summary>The content header of the message.</summary>
        public IBasicProperties BasicProperties { get; set; }

        /// <summary>The message body.</summary>
        public byte[] Body { get; set; }

        /// <summary>The consumer tag of the consumer that the message
        /// was delivered to.</summary>
        public string ConsumerTag { get; set; }

        /// <summary>The delivery tag for this delivery. See
        /// IModel.BasicAck.</summary>
        public ulong DeliveryTag { get; set; }

        /// <summary>The exchange the message was originally published
        /// to.</summary>
        public string Exchange { get; set; }

        /// <summary>The AMQP "redelivered" flag.</summary>
        public bool Redelivered { get; set; }

        /// <summary>The routing key used when the message was
        /// originally published.</summary>
        public string RoutingKey { get; set; }
    }
}
