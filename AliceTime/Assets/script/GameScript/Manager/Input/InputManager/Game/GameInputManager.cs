using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInputManager : MonoBehaviour {
	private static GameInputBase.STATE currentInputState;

	public static void SetCurrentInputState(GameInputBase.STATE state) 
	{
		currentInputState = state;
	}

	public static GameInputBase.STATE GetCurrentInputState() 
	{
		return currentInputState;
	}

	void Awake() 
	{
		currentInputState = GameInputBase.STATE.NULL;
	}
}
