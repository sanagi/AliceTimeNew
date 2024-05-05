using UnityEngine;
using System;
using System.Collections;
using Rewired;

public class Title_Start : PhaseBase {
	protected TitleStartPanel panel;

	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
        var canvas = GameObject.Find(GameDefine.FRONT_CANVAS);
        if (canvas == null)
        {
            Debug.Log("\"FrontCanvas\" is missing");
            return;
        }

        var objPanel = canvas.transform.Find(GameDefine.START_PANEL);
        if (objPanel == null)
        {
            Debug.Log("\"Start_Panel\" is missing");
            return;
        }

        panel = objPanel.GetComponent<TitleStartPanel>();
        panel.Enable();

        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeIn(() =>
        {
            // BGM再生
            SoundManager.Instance.PlayMusic(MusicId.Title);
        }));
    }

	public override void OnExit (PhaseBase nextPhase)
	{
        if (panel == null) return;
		panel.Disable ();
        SaveManager.Instance.tmpFromEnding = false;
	}
}
