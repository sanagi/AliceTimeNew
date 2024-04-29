using UnityEngine;
using System.Collections;

public class Title_Dialog : PhaseBase {
	private static TitlePanelBehaviour panel;

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

		var objPanel = canvas.transform.Find ("Dialog_Panel");
		if (objPanel == null) {
			Debug.Log ("\"Dialog_Panel\" is missing");
			return;
		}

		panel = objPanel.GetComponent<TitlePanelBehaviour> ();
		panel = objPanel.GetComponent<TitlePanelBehaviour> ();
		if (panel == null) {
			Debug.Log ("\"Dialog_Panel\" is not added \"TitlePanelBehaviour\"");
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
