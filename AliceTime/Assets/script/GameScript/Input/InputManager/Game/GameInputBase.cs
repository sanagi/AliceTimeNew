using UnityEngine;
using System.Collections;
using KamioriInput;

public abstract class GameInputBase : MonoBehaviour, ITouchEventHandler {
	public enum STATE {
		LINE, PAPER, WORLD, EXPANDED, FLICK, NULL
	};

	public abstract void OnEnableAction ();
	public abstract void OnDisableAction ();

	public abstract bool OnBeganAction (TouchInfo[] info);
	public abstract bool OnEndedAction (TouchInfo[] info);
	public abstract bool OnMovedAction (TouchInfo[] info);

	public abstract int myOrder{ get; }
	public abstract STATE myState{ get; }

	protected static bool isTouch;
	public static bool IsEnable() { return isTouch; }
	public static void Enable() { isTouch = true; }
	public static void Disable() { isTouch = false;}

	#region ITouchEventHandler implementation
	public bool OnTouchEventBegan (TouchInfo[] touchInfo)
	{
		if (OnBeganAction (touchInfo)) {
			GameInputManager.SetCurrentInputState (myState);
		}
		return false;
	}

	public bool OnTouchEventEnded (TouchInfo[] touchInfo)
	{
		OnEndedAction (touchInfo);
		return false;
	}

	public bool OnTouchEventMoved (TouchInfo[] touchInfo)
	{
        OnMovedAction (touchInfo);
		return false;
	}
	#endregion

	#region IInputEventHandler implementation
	public int Order { get { return myOrder; } }
	public bool Process { get { return isTouch; } }
	#endregion

	void Start()
	{
		AliceInputManager.RegisterTouchEventHandler (this);
	}

	void OnDestory()
	{
		AliceInputManager.UnregisterTouchEventHandler (this);
	}
}
