using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using InputSupport;

namespace KamioriInput
{
	public class MouseEvent : MonoBehaviour
	{
		private InputInfoManager<TouchInfo> infoManager;
		private List<TouchInfo> touches;
        private Vector3 beforeMousePosition;

        void Awake()
		{
            infoManager = TouchInputManager.Instance.InfoManager;
			touches = new List<TouchInfo> ();

            beforeMousePosition = Vector3.zero;
        }

		void Start()
		{
			Input.simulateMouseWithTouches = false;
		}

		void Update()
		{
            if (infoManager == null) {
                infoManager = TouchInputManager.Instance.InfoManager;
            }
            if (touches == null) {
                touches = new List<TouchInfo>();
            }

			for(var i=infoManager.InfoCount-1; i>=0; i--) {
				var currentInfo = infoManager.InputInfo [i];
				if (currentInfo.Phase == InputPhase.Ended && currentInfo.Id == -1) {
                    currentInfo.Phase = InputPhase.Missing;
                    infoManager.UpdateParam(currentInfo, null);
				}
			}

			InputForMouse (ref touches);
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
				touches.Clear ();
			}
		}

		void OnDestroy()
		{
			foreach (var touch in touches) {
                touch.Phase = InputPhase.Ended;
                infoManager.UpdateParam(touch, null);
			}
		}

        private void InputForMouse (ref List<TouchInfo> touchInfo)
		{
			TouchInfo info = new TouchInfo ();
			info.deltaTime = Time.deltaTime;
			info.Id = -1;

			#if UNITY_EDITOR
				info.currentScreenPosition = currentMousePositionInGameView;
			#else
				info.currentScreenPosition = Input.mousePosition;
			#endif

            info.deltaDistance = info.currentScreenPosition - beforeMousePosition;
            beforeMousePosition = info.currentScreenPosition;

			if (Input.GetMouseButtonDown (0)) {
				info.Phase = InputPhase.Began;
				touchInfo.Add (info);
			} else if (Input.GetMouseButtonUp (0)) {
				info.Phase = InputPhase.Ended;
				touchInfo.Add (info);
			} else if (Input.GetMouseButton (0)) {
				info.Phase = InputPhase.Stay;
				touchInfo.Add (info);
			}
		}

		#if UNITY_EDITOR
		private Vector2 currentMousePositionInGameView = Vector2.zero;
		void OnGUI()
		{
			Vector2 currentMousePosition = Event.current.mousePosition;
			var x = currentMousePosition.x;
			var y = Screen.height - currentMousePosition.y;
			currentMousePosition = new Vector2 (x, y);
			currentMousePositionInGameView = currentMousePosition;
		}
		#endif
	}
}
