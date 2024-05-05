using UnityEngine;
using System.Collections;
using Rewired;

public class TitleUIManager : UIManager {
	public static TitlePanelBehaviour CurrentPanel;
	
	public static GameObject _continueButton;

	public static bool IsContorollable = false;

    IButtonEvent touchedButton;

    public static Player UiPlayer = null;

    private Canvas _titleCanvas = null;

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

	public int MyOrder {
		get {
			return 100;
		}
	}
}
