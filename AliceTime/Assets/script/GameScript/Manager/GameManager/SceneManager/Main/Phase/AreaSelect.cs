using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class AreaSelect : PhaseBase {
	public string SelectedID; //どの情報を読み込むか(階層？)

	/// <summary>フェーズの名称</summary>
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	/// <summary>フェーズに入った時に呼ばれる</summary>
	public override void OnEnter (PhaseBase prevPhase)
	{
		//Camera_Move_Dangion.Instance.Enable();
		if (prevPhase == null) {
			SelectedID = "01"; //はじめから
		} else {
			if (prevPhase.GetType () == typeof(Title)) {
				SelectedID = ((Title)prevPhase).SelectedID;
			}
			else if (prevPhase.GetType() == typeof(AreaSelect))
			{
				SelectedID = ((AreaSelect)prevPhase).SelectedID;
			}
		}
		
		var sceneName = GameDefine.AreaSelect;
		if (SelectedID == "-1")
		{
			sceneName = "DebugWorldGameScene";
		}
		
		LoadManager.Instance.Transition(sceneName, LoadSceneMode.Single, () => {
            // [Rewired] enable key map
            var player = ReInput.players.GetPlayer(0);
            player.controllers.maps.SetAllMapsEnabled(false);
            player.controllers.maps.SetMapsEnabled(true, "System");
            
        });
	}

	/// <summary>フェーズを出るときに呼ばれる</summary>
	public override void OnExit (PhaseBase nextPhase)
	{
        Audio_Manage.StopBGM();
    }
}
