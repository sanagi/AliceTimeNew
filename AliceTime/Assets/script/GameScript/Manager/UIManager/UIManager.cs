using UnityEngine;
using System.Collections;
using KamioriInput;

public abstract class UIManager : MonoBehaviour, IKeyEventHandler, ITouchEventHandler {
    public abstract void DoCrossKeyEvent (KeyInfo info);
	public abstract void DoJumpKey (KeyInfo info);
	public abstract bool DoTouchBegan (TouchInfo[] info);
	public abstract bool DoTouchMoved (TouchInfo[] info);
	public abstract bool DoTouchEnded (TouchInfo[] info);
	public virtual int MyOrder (){
		return 0;
	}

	private static bool isInput = false;

	public static bool IsInput() { return isInput; }
	public static void EnableInput() { isInput = true; }
	public static void DisableInput() { isInput = false; }

    private float border = 0.25f;

    public void Initialization() 
	{
		AliceInputManager.RegisterTouchEventHandler (this);
    }

    public void Finalization()
	{
		AliceInputManager.UnregisterTouchEventHandler (this);
	}

	#region IKeyEventHandler implementation

	public void OnCrossKeyEvent (KeyInfo info)
	{
        DoCrossKeyEvent (info);
	}

	public void OnJumpKeyEvent (KeyInfo info)
	{
		DoJumpKey (info);
	}

	#endregion

	#region IInputEventHandler implementation

	bool ITouchEventHandler.OnTouchEventBegan (TouchInfo[] touchInfo)
	{
		return DoTouchBegan (touchInfo);
	}

	bool ITouchEventHandler.OnTouchEventEnded (TouchInfo[] touchInfo)
	{
		return DoTouchEnded (touchInfo);
	}

	bool ITouchEventHandler.OnTouchEventMoved (TouchInfo[] touchInfo)
	{
		return DoTouchMoved (touchInfo);
	}

	#endregion

	public int Order {
		get {
			return MyOrder();
		}
	}

	public bool Process {
		get {
			return isInput;
		}
	}
}
