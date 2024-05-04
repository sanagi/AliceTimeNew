using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : PhaseBase {
	public string StageID;

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
			StageID = "00001"; //はじめから
		} else {
			if (prevPhase.GetType () == typeof(Title)) {
				StageID = ((Title)prevPhase).SelectedID;
            }
            else if (prevPhase.GetType() == typeof(AreaSelect))
            {
                StageID = ((AreaSelect)prevPhase).SelectedID;
			}
		}

        LoadManager.Instance.Transition("Game", LoadSceneMode.Single, () => {});
	}

	/// <summary>フェーズを出るときに呼ばれる</summary>
	public override void OnExit (PhaseBase nextPhase)
	{
		SoundManager.Instance.StopMusic();
		//Audio_Manage.eventAudioList.Clear();
		//Audio_Manage.StopLoopAllSE();
		//Camera_Move_Dangion.Instance.Disable();
    }
}
