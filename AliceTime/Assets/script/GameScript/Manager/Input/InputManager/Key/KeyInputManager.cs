using UnityEngine;
using System.Collections.Generic;
using InputSupport;

namespace KamioriInput
{
	public class KeyInputManager : InputPluginBase<IKeyEventHandler, KeyEventManager, KeyInfo> 
	{
		void Start() {
			Setup ();
		}

		private void Setup()
		{
			gameObject.AddComponent<VirtualControllerEvent> ();
			gameObject.AddComponent<KeyboardEvent> ();
		}
	}
}