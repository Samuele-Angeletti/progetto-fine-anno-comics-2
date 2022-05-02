using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PubSub
{
    public static class PubSub
    {
        private static Dictionary<Type, List<ISubscriber>> _allSubscribers = new Dictionary<Type, List<ISubscriber>>();

        public static void Subscribe(ISubscriber subscriber, Type messageType)
        {
            if (_allSubscribers.ContainsKey(messageType))
            {
                _allSubscribers[messageType].Add(subscriber);
            }
            else
            {
                List<ISubscriber> subscribers = new List<ISubscriber> { subscriber };

                _allSubscribers.Add(messageType, subscribers);
            }
        }

        public static void Publish(IMessage message)
        {
            Type messageType = message.GetType();

            if (!_allSubscribers.ContainsKey(messageType)) return;

            foreach (ISubscriber subscriber in _allSubscribers[messageType])
            {
                subscriber.OnPublish(message);
            }
        }

        public static void Unsubscribe(ISubscriber subscriber, Type messageType)
        {
            if (_allSubscribers.ContainsKey(messageType))
            {
                _allSubscribers[messageType].Remove(subscriber);
            }
        }
    }
}
