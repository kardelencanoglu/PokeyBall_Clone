using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 

public class UnityOneArgEvent : UnityEvent<object> { }
public class UnityTwoArgsEvent : UnityEvent<object, object> { };

namespace PokeyBallTest.Manager
{
    
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> eventDictionary;
        private Dictionary<string, UnityOneArgEvent> eventOneArgDictionary;
        private Dictionary<string, UnityTwoArgsEvent> eventTwoArgsDictionary;

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                    
                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else 
                    {
                        eventManager.Init();
                    }
                }
                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }

            if (eventOneArgDictionary == null)
            {
                eventOneArgDictionary = new Dictionary<string, UnityOneArgEvent>();
            }

            if (eventTwoArgsDictionary == null)
            {
                eventTwoArgsDictionary = new Dictionary<string, UnityTwoArgsEvent>();
            }
        }

        
        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        
        public static void StartListening(string eventName, UnityAction<object> listener)
        {
            UnityOneArgEvent thisEvent = null;

            if (instance.eventOneArgDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityOneArgEvent();
                thisEvent.AddListener(listener);
                instance.eventOneArgDictionary.Add(eventName, thisEvent);
            }
        }

        
        public static void StartListening(string eventName, UnityAction<object, object> listener)
        {
            UnityTwoArgsEvent thisEvent = null;

            if (instance.eventTwoArgsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityTwoArgsEvent();
                thisEvent.AddListener(listener);
                instance.eventTwoArgsDictionary.Add(eventName, thisEvent);
            }
        }

        
        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) return; 
            UnityEvent thisEvent = null;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void StopListening(string eventName, UnityAction<object> listener)
        {
            if (eventManager == null) return; 
            UnityOneArgEvent thisEvent = null;

            if (instance.eventOneArgDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void StopListening(string eventName, UnityAction<object, object> listener)
        {
            if (eventManager == null) return; 
            UnityTwoArgsEvent thisEvent = null;

            if (instance.eventTwoArgsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        
        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (instance.eventDictionary != null && instance.eventDictionary.Count > 0 && instance.eventDictionary.ContainsKey(eventName) && instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(); 
            }
        }

        public static void TriggerEvent(string eventName, object arg)
        {
            UnityOneArgEvent thisEvent = null;
            if (instance.eventOneArgDictionary != null && instance.eventOneArgDictionary.Count > 0 && instance.eventOneArgDictionary.ContainsKey(eventName) && instance.eventOneArgDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(arg); 
            }
        }

        public static void TriggerEvent(string eventName, object arg1, object arg2)
        {
            UnityTwoArgsEvent thisEvent = null;
            if (instance.eventTwoArgsDictionary != null && instance.eventTwoArgsDictionary.Count > 0 && instance.eventTwoArgsDictionary.ContainsKey(eventName) && instance.eventTwoArgsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(arg1, arg2); 
            }
        }
    }
}
