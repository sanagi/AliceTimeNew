using UnityEngine;
using System.Collections;
using KamioriInput;
using Rewired;

public class TitleUIManager : UIManager {
	public static TitlePanelBehaviour CurrentPanel;
	
	public static GameObject _continueButton;

	public static bool IsContorollable = false;

    IButtonEvent touchedButton;

    public static Player UiPlayer = null;

    private void Start()
    {
	    if (UiPlayer == null)
	    {
		    UiPlayer = ReInput.players.GetPlayer(0);
	    }
    }

	    public void SetTsuzuki(GameObject tButton){
	    _continueButton = tButton;
	}

	    #region implemented abstract members of UIManager

    public override void DoCrossKeyEvent (KeyInfo info)
	{
		
	}

	public override void DoJumpKey (KeyInfo info)
	{
	}

	public override bool DoTouchBegan (TouchInfo[] info)
	{
		return false;
	}

	public override bool DoTouchMoved (TouchInfo[] info)
	{
		return false;
	}

	public override bool DoTouchEnded (TouchInfo[] info)
	{
		return false;
	}

	#endregion

	public int MyOrder {
		get {
			return 100;
		}
	}
}
