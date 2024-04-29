using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace KamioriInput
{
	public class TouchInputManager : InputPluginBase<ITouchEventHandler, TouchEventManager, TouchInfo>
	{
		void Start()
		{
			Setup ();	
		}

		private void Setup ()
		{
			gameObject.AddComponent<TouchEvent> ();
			gameObject.AddComponent<MouseEvent> ();
		}
	}
}