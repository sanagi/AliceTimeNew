﻿using UnityEngine;
using System.Collections;

public class AreaSelect_Start : PhaseBase {
    PlayerController playerController;
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
    {
	    // キャラクターの状態を初期化

	    PlayerManager.Instance.SetState(PlayerController.STATE.WAIT);

        //入力を初期化
        AliceInputManager.ClearInput();

        //フェードインしてゲーム開始
        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeIn(() =>
        {
	        // ゲーム開始
	        AreaSelectSceneManager.Goto(GameDefine.AREASELECT_MAIN);
        }));

	}

	public override void OnExit (PhaseBase nextPhase)
	{
		
	}
}
