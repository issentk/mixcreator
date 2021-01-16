using System;
using System.Collections.Generic;

namespace MixCreator.Event
{
    public static class EventBus
    {
        private static Dictionary<Type, List<Action<object>>> handlers
            = new Dictionary<Type, List<Action<object>>>();

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            if (!handlers.ContainsKey(typeof(T)))
                handlers.Add(typeof(T), new List<Action<object>>());

            handlers[typeof(T)].Add(e => handler((T)e));
        }

        public static void Publish<T>(T e) where T : IEvent
        {
            var eventType = typeof(T);

            foreach (Type handlerType in handlers.Keys)
                TryPublishForType(handlerType, eventType, e);
        }

        private static void TryPublishForType(Type handlerType, Type eventType, object e)
        {
            if (handlerType.IsAssignableFrom(eventType))
                handlers[handlerType].ForEach(handler => handler(e));
        }
    }
}
