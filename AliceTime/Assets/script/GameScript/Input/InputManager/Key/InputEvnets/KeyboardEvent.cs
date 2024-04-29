using UnityEngine;
using InputSupport;

namespace KamioriInput
{
	public class KeyboardEvent : MonoBehaviour 
	{
		private InputInfoManager<KeyInfo> infoManager;
		private KeyInfo crossKey;
		private KeyInfo jumpKey;

		void Awake()
		{
            infoManager = KeyInputManager.Instance.InfoManager;
			
			crossKey = new KeyInfo ();
            crossKey.Id = int.MinValue;
		}

		void Update()
		{	
			// CrossKey
			if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D) || Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				crossKey.Right = 1;
			} else {
				crossKey.Right = 0;
			}
				
			if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				crossKey.Left = 1;
			} else {
				crossKey.Left = 0;
			}

			if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				crossKey.Up = 1;
			} else {
				crossKey.Up = 0;
			}
			if (Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.S) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
				crossKey.Down = 1;
			} else {
				crossKey.Down = 0;
			}


			if(Input.GetKeyDown(KeyCode.Space)) {
				crossKey.Jump = 1;
			} else {
				crossKey.Jump = 0;
			}


            if (infoManager == null) {
                infoManager = KeyInputManager.Instance.InfoManager;
            }

			if(crossKey.Phase == InputPhase.Ended) {
				crossKey.Phase = InputPhase.Missing;
				crossKey.Up = 0;
				crossKey.Down = 0;
				crossKey.Right = 0;
				crossKey.Left = 0;
				infoManager.UpdateParam(crossKey, null);
			} else if(crossKey.Right == 0 && crossKey.Left == 0 && crossKey.Up == 0 && crossKey.Down == 0) {
				crossKey.Phase = InputPhase.Ended;
				infoManager.UpdateParam(crossKey, null);               
			} else {
				if(crossKey.Phase == InputPhase.Missing) {
					crossKey.Phase = InputPhase.Began;
					infoManager.UpdateParam(crossKey, null);
				} else if(crossKey.Phase != InputPhase.Stay) {
					crossKey.Phase = InputPhase.Stay;
					infoManager.UpdateParam(crossKey, null);
				} else {
					infoManager.UpdateParam(crossKey, null);
				}
            }
		}
	}
}