using UnityEngine;
using System.Collections;
using InputSupport;

namespace KamioriInput
{
	public interface ITouchEventHandler : IInputEventHandler
	{
		bool OnTouchEventBegan (TouchInfo[] touchInfo);
		bool OnTouchEventEnded (TouchInfo[] touchInfo);
		bool OnTouchEventMoved (TouchInfo[] touchInfo);
	}
}
