using UnityEngine;
using System.Collections;
using Rewired;

public class Title_Init : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
        // [Rewired] enable key map
        var player = ReInput.players.GetPlayer(0);
        player.controllers.maps.SetAllMapsEnabled(false);
        player.controllers.maps.SetMapsEnabled(true, GameDefine.CATEGORY_SYSTEM);

        //TouchEffectManger.Instance.dragEffect = false;
        // 起動時とEndから帰って来た時はスタート画面へ遷移する
        var currentPhase = MainSceneManager.CurrentPhase;
		if (currentPhase == null || ((Title)currentPhase).PrePhaseName == "" || SaveManager.Instance.tmpFromEnding) {
			TitleManager.Instance.wait = true;
		} 
		else {
            TitleManager.Instance.BackGround.SetActive(true);

            Audio_Manage.Play(SoundEnum.BGM_TITLE);
			TitleSceneManager.Goto("Title_SelectMode");
            TitleManager.Instance.init = true;
		}
        //CameraManager.Instance.AspectChange(true);
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		
	}
}
