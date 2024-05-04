using UnityEngine;
using System.Collections;
using InputSupport;

namespace KamioriInput
{
	public interface IKeyEventHandler : IInputEventHandler {
		void OnCrossKeyEvent (KeyInfo info);
	}
}