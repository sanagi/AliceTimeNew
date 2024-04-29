using UnityEngine;
using System.Collections;
using System.IO;

public class Title_Option : PhaseBase
{
	private TitlePanelBehaviour panel;

	public override string PhaseName
	{
		get
		{
			return this.GetType().FullName;
		}
	}

	public override void OnEnter(PhaseBase prevPhase) {
		var canvas = GameObject.Find("FrontCanvas");
		if (canvas == null) {
			Debug.Log("\"FrontCanvas\" is missing");
			return;
		}

		var objPanel = canvas.transform.Find("Option_Panel");
		if (objPanel == null) {
			Debug.Log("\"Option_Panel\" is missing");
			return;
		}

		panel = objPanel.GetComponent<TitlePanelBehaviour>();
		if (panel == null) {
			Debug.Log("\"Option_Panel\" is not added \"TitlePanelBehaviour\"");
			return;
		}
		
		panel.Enable();
	}

	public override void OnExit(PhaseBase nextPhase) {
		if (panel == null)
			return;

		// オプション画面を閉じたときにユーザー設定を保存する
		OptionManager.Instance.Save ();

		panel.Disable();
	}
}
