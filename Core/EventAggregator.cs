using System;
using System.Collections.Generic;

namespace FitTrack.Core
{
    /// <summary>
    /// Manages the publishing and subscription of events in a singleton pattern.
    /// </summary>
    /// <remarks>
    /// This class allows different components to subscribe to specific event types, associating each subscription with an <see cref="Action"/> to be executed. 
    /// When an event of a subscribed type is published, all registered actions for that type are invoked.
    /// </remarks>
    public class EventAggregator
    {
        private static readonly Dictionary<Type, List<Action<object>>> _subscribers = new Dictionary<Type, List<Action<object>>>();

        /// <summary>
        /// Subscribes to an event of type <typeparamref name="T"/> with the provided action.
        /// </summary>
        /// <typeparam name="T">The type of the event message.</typeparam>
        /// <param name="action">The action to be invoked when the event is published.</param>
        public static void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Action<object>>();
            }
            _subscribers[type].Add(o => action((T)o));
        }

        /// <summary>
        /// Publishes an event of type <typeparamref name="T"/> to all subscribed actions.
        /// </summary>
        /// <typeparam name="T">The type of the event message.</typeparam>
        /// <param name="message">The event message to be published.</param>
        public static void Publish<T>(T message)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                foreach (var action in _subscribers[type])
                {
                    action(message);
                }
            }
        }
    }
}
