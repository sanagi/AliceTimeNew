using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace KamioriInput
{
    public class TouchEventManager : InputEventHandlerManager<ITouchEventHandler, TouchInfo>
    {
        #region implemented abstract members of InputEventHandlerManager

        public override void FireEvent(List<TouchInfo> touches)
        {
            FireEndedEvent(touches.FindAll(i => (i.Phase == InputPhase.Ended || i.Phase == InputPhase.Canceled)));
            FireBeganEvent(touches.FindAll(i => i.Phase == InputPhase.Began));
            FireMovedEvent(touches.FindAll(i => i.Phase == InputPhase.Stay));
        }

        #endregion

        private void FireBeganEvent(List<TouchInfo> touches)
        {
            if (touches.Count == 0)
                return;

            foreach (var handler in registedHandlers)
            {
                if (!handler.Process)
                    continue;

                if (handler.OnTouchEventBegan(touches.ToArray()))
                {
                    return;
                }
            }
        }

        private void FireEndedEvent(List<TouchInfo> touches)
        {
            if (touches.Count == 0)
                return;

            foreach (var handler in registedHandlers)
            {
                if (!handler.Process)
                    continue;

                if (handler.OnTouchEventEnded(touches.ToArray()))
                {
                    return;
                }
            }
        }

        private void FireMovedEvent(List<TouchInfo> touches)
        {
            if (touches.Count == 0)
                return;

            foreach (var handler in registedHandlers)
            {
                if (!handler.Process)
                    continue;

                if (handler.OnTouchEventMoved(touches.ToArray()))
                {
                    return;
                }
            }
        }
    }
}