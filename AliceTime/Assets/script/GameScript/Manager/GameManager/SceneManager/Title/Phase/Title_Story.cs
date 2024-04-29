using UnityEngine;
using System.Collections;

public class Title_Story : PhaseBase {
	protected TitlePanelBehaviour panel;
	public TitlePanelBehaviour Panel {
		get {
			return panel;
		}
	}

	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		var canvas = GameObject.Find ("FrontCanvas");
		if (canvas == null) {
			Debug.Log ("\"FrontCanvas\" is missing");
			return;
		}

		var objPanel = canvas.transform.Find ("StoryMode_Panel");
		if (objPanel == null) {
			Debug.Log ("\"StoryMode_Panel\" is missing");
			return;
		}

		panel = objPanel.GetComponent<TitlePanelBehaviour> ();
		if (panel == null) {
			Debug.Log ("\"StoryMode_Panel\" is not added \"TitlePanelBehaviour\"");
			return;
		}

		panel.Enable ();
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		if (panel == null)
			return;

		panel.Disable ();
	}
}
