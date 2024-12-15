using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// 事件管理
///
namespace MM.Common
{


	[ExecuteAlways]
    public static class MMEventManager 
    {
        private static Dictionary<Type, List<IMMEventListener>> _subscribersList;


		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeStatics()
        {
            _subscribersList = new Dictionary<Type, List<IMMEventListener>> ();
        }

        static MMEventManager()
        {
            _subscribersList = new Dictionary<Type, List<IMMEventListener>> ();
        }

        public static void AddListener<MMEvent>(MMEventListener<MMEvent> listener) where MMEvent : struct
        {
            Type eventType = typeof(MMEvent);
            if (!_subscribersList.ContainsKey(eventType))
            {
                _subscribersList[eventType] = new List<IMMEventListener> ();
            } 
            if (!SubscriptionExists(eventType, listener)) 
            {
                _subscribersList[eventType].Add(listener);
            }
        }

        public static void RemoveListener<MMEvent>(MMEventListener<MMEvent> listener) where MMEvent: struct
        {
            Type eventType = typeof(MMEvent);
            if (!_subscribersList.ContainsKey(eventType)) return;
            List<IMMEventListener> list = _subscribersList[eventType];

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == listener)
                {
                    list.RemoveAt(i);
                    if (list.Count == 0)
                    {
                        _subscribersList.Remove(eventType);
                    }
                }
                return;
            }
        }

        public static void TriggerEvent<MMEvent>(MMEvent newEvent) where MMEvent : struct
        {
            if (!_subscribersList.TryGetValue(typeof(MMEvent), out var listenerList)) return;
            for (int i = listenerList.Count - 1; i > 0; i--)
            {
                (listenerList[i] as MMEventListener<MMEvent>).OnMMEvent(newEvent);
            }
        }

        public static bool SubscriptionExists( Type type, IMMEventListener listener )
        {
            if (!_subscribersList.TryGetValue(type, out List<IMMEventListener> receivers)) return false;
            bool exists = false;
            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == listener)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }

    public static class EventRegister
    {
        public static void MMEventStartListening<EventType> (this MMEventListener<EventType> listener) where EventType : struct
        {
            MMEventManager.AddListener<EventType>(listener);
        }

        public static void MMEventStopListening<EventType> (this MMEventListener<EventType> listener) where EventType : struct
        {
            MMEventManager.RemoveListener<EventType>(listener);
        }
    }

    public interface IMMEventListener { }

    public interface MMEventListener<T> : IMMEventListener
    {
        void OnMMEvent(T eventType);
    }

    public class MMEventListenerWrapper<TOwner, TTarget, TEvent> : MMEventListener<TEvent>, IDisposable
        where TEvent : struct
    {
        private Action<TTarget> _callback;
        private TOwner _owner;

        public MMEventListenerWrapper(TOwner owner, Action<TTarget> callback)
        {
            _owner = owner;
            _callback = callback;
            this.MMEventStartListening<TEvent>();
        }

        public void Dispose()
        {
            this.MMEventStopListening<TEvent>();
            _callback = null;
        }

        protected virtual TTarget OnEvent(TEvent eventType) => default;

        public void OnMMEvent(TEvent eventType)
        {
            var item = OnEvent(eventType);
            _callback?.Invoke(item);
        }
    }
}