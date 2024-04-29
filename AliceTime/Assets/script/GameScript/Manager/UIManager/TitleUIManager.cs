using UnityEngine;
using System.Collections;
using KamioriInput;
using Rewired;

public class TitleUIManager : UIManager {
	public static TitlePanelBehaviour CurrentPanel;

	public static GameObject galleryButton;
	public static GameObject tsuzukiButton;
	public static GameObject trialButton;

    public static bool IsContorollable = false;

    IButtonEvent touchedButton;

	private ParticleSystem effectTouch;
	private ParticleSystem TouchEffect{
		get {
			if (effectTouch != null) {
				return effectTouch;
			}

			var effect = Resources.Load<GameObject> ("Input/TouchEffect") as GameObject;
			var effectObj = Instantiate (effect) as GameObject;
			effectTouch = effectObj.GetComponent<ParticleSystem> ();
			effectTouch.loop = false;
			return effectTouch;	
		}
	}

    public static Player uiPlayer = null;

	public void SetGallery(GameObject gButton){
		galleryButton = gButton;
	}

	public void SetTsuzuki(GameObject tButton){
		tsuzukiButton = tButton;
	}	

	public void SetTrial(GameObject trButton){
		trialButton = trButton;
	}

	private ParticleSystem effectDrag;
	private ParticleSystem DragEffect{
		get {
			if (effectDrag != null) {
				return effectDrag;
			}

			var effect = Resources.Load<GameObject> ("Input/DragEffect") as GameObject;
			var effectObj = Instantiate (effect) as GameObject;
			effectDrag = effectObj.GetComponent<ParticleSystem> ();
			effectDrag.loop = false;
			return effectDrag;
		}
	}

#if UNITY_SWITCH
    private void Update()
    {
        if (uiPlayer == null)
        {
            uiPlayer = ReInput.players.GetPlayer(0);
        }
    }
#endif

    #region implemented abstract members of UIManager

    public override void DoCrossKeyEvent (KeyInfo info)
	{
		
	}

	public override void DoJumpKey (KeyInfo info)
	{
        Debug.Log("ジャンプ話した");
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
