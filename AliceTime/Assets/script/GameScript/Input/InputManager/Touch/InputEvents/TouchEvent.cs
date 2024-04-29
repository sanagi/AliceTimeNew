using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using InputSupport;

namespace KamioriInput
{
    public class TouchEvent : MonoBehaviour
    {
        private InputInfoManager<TouchInfo> infoManager;
        private List<TouchInfo> touches;

        void Awake() {
            infoManager = TouchInputManager.Instance.InfoManager;
            touches = new List<TouchInfo>();
        }

        void Update() {
            if (infoManager == null) {
                infoManager = TouchInputManager.Instance.InfoManager;
            }
            if (touches == null) {
                touches = new List<TouchInfo>();
            }

            for (var i = infoManager.InfoCount - 1; i >= 0; i--) {
                var currentInfo = infoManager.InputInfo[i];
                if (currentInfo.Phase == InputPhase.Ended && currentInfo.Id != -1) {
                    currentInfo.Phase = InputPhase.Missing;
                    infoManager.UpdateParam(currentInfo, null);
                }
            }

            InputForTouch(ref touches);
            if (touches.Count > 0) {
                foreach (var touch in touches) {
                    switch (touch.Phase) {
                        case InputPhase.Canceled:
                            touch.Phase = InputPhase.Ended;
                            infoManager.UpdateParam(touch, null);
                            break;
                        default:
                            infoManager.UpdateParam(touch, null);
                            break;
                    }
                }
                touches.Clear();
            }
        }

        void OnDestroy() {
            foreach (var touch in touches) {
                touch.Phase = InputPhase.Ended;
                infoManager.UpdateParam(touch, null);
            }
        }


        private void InputForTouch(ref List<TouchInfo> touchInfo) {
            if (Input.touchCount == 0)
                return;

            foreach (var touch in Input.touches) {
                TouchInfo info = new TouchInfo();
                info.Id = touch.fingerId;
                info.currentScreenPosition = touch.position;
                info.deltaTime = Time.deltaTime;
                switch (touch.phase) {
                    case TouchPhase.Began:
                        info.Phase = InputPhase.Began;
                        touchInfo.Add(info);
                        break;
                    case TouchPhase.Moved:
                        info.Phase = InputPhase.Stay;
                        touchInfo.Add(info);
                        break;
                    case TouchPhase.Ended:
                        info.Phase = InputPhase.Ended;
                        touchInfo.Add(info);
                        break;
                    case TouchPhase.Canceled:
                        info.Phase = InputPhase.Canceled;
                        touchInfo.Add(info);
                        break;
                    case TouchPhase.Stationary:
                        info.Phase = InputPhase.Stay;
                        touchInfo.Add(info);
                        break;
                }
            }
        }
    }
}
