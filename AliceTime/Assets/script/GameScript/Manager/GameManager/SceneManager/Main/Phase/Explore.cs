using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class Explore : PhaseBase {
	public string SelectedID; //どのエリアを読み込むか(階層？)

	/// <summary>フェーズの名称</summary>
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	/// <summary>フェーズに入った時に呼ばれる</summary>
	public override void OnEnter (PhaseBase prevPhase)
	{
		var sceneName = GameDefine.Explore;
		
		//Camera_Move_Dangion.Instance.Enable();
		if (prevPhase == null) {
			SelectedID = "0000"; //チュートリアル？
		} else {
			if (prevPhase.GetType () == typeof(Title)) {
				SelectedID = ((Title)prevPhase).SelectedID;
			}
			else if (prevPhase.GetType() == typeof(AreaSelect))
			{
				SelectedID = ((AreaSelect)prevPhase).SelectedID;
			}
		}
		
		//デバッグモードでの起動なら強制的にID切り替え
		if (DebugManager.Instance.IsDebugMode)
		{
			SelectedID = DebugManager.Instance.DebugExploreAreaId;
		}		
		
        LoadManager.Instance.Transition(sceneName, LoadSceneMode.Single, () => {
            // [Rewired] enable key map
            var player = ReInput.players.GetPlayer(0);
            
            player.controllers.maps.SetAllMapsEnabled(false);
            player.controllers.maps.SetMapsEnabled(true, GameDefine.EXPLORE_MAPS_REWIRED);
        });
	}

	/// <summary>フェーズを出るときに呼ばれる</summary>
	public override void OnExit (PhaseBase nextPhase)
	{
        SoundManager.Instance.StopMusic();
    }
}
