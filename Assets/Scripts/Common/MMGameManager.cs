using System.Collections;
using UnityEngine;

namespace MM.Common
{
    public enum GameEventType
    {
        GameStart,
        Pause,
        Resume,
        GameOver,
    }
    public struct MMGameEvent
    {
        public GameEventType EventType;

        public MMGameEvent(GameEventType eventType)
        {
            EventType = eventType;
        }

        static MMGameEvent e;
        public static void Trigger(GameEventType eventType)
        {
            e.EventType = eventType;
            MMEventManager.TriggerEvent(e);
        }
    }
    public class MMGameManager : MMSingleton<MMGameManager>, MMEventListener<MMGameEvent>
    {
        private void OnEnable()
        {
            this.MMEventStartListening<MMGameEvent>();   
        }

        private void OnDisable()
        {
            this.MMEventStopListening<MMGameEvent>();
        }
        public void OnMMEvent(MMGameEvent gameEvent)
        {
            switch (gameEvent.EventType)
            {
                case GameEventType.GameStart:
                    //TODO:
                    break;
                case GameEventType.GameOver:
                    //TODO:
                    break;
            }
        }
    }
}