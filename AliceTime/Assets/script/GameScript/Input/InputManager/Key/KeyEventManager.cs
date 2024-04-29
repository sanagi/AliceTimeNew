using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;
using Rewired;


namespace KamioriInput
{
    public class KeyEventManager : InputEventHandlerManager<IKeyEventHandler, KeyInfo>
    {
        private float border = 0.25f;
        #region implemented abstract members of InputEventHandlerManager

        public override void FireEvent(List<KeyInfo> info)
        {
            var crossKey = new KeyInfo();
            crossKey.Phase = InputPhase.Missing;
            foreach (var i in info)
            {
                if (i.Phase == InputPhase.Began)
                {
                    crossKey.Phase = InputPhase.Began;
                }

                // 十字キー
                if (i.Right > border && i.Right > crossKey.Right)
                {
                    crossKey.Right = 1;
                }
                if (i.Left > border && i.Left > crossKey.Left)
                {
                    crossKey.Left = 1;
                }
                if (i.Up > border && i.Up > crossKey.Up)
                {
                    crossKey.Up = 1;
                }
                if (i.Down > border && i.Down > crossKey.Down)
                {
                    crossKey.Down = 1;
                }

                if (crossKey.Jump == 0 && i.Jump == 1)
                {
                    crossKey.Jump = 1;
                }
            }

            foreach (var handler in registedHandlers)
            {
                if (!handler.Process)
                    continue;

                handler.OnCrossKeyEvent(crossKey);
            }

            #endregion
        }
    }
}