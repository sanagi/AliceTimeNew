using UnityEngine;
using System.Collections;

// プレイヤーがステージ外に出てしまった状態
public class Game_Death : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		// 死亡時の演出があれば

		//見た目を戻す
        // ハルの位置を補正
        //AibouManager.MoveToPosition(MainGameManager.RespawnPosition + Vector3.up, 0);
        //AibouManager.SetLayer("FrontGUI");

        // カメラにプレイヤーをセット
        //Camera_Move_Dangion.Instance.PlayerSet();
        //Camera_Move_Dangion.Instance.PosSetP();
        //Camera_Move_Dangion.Instance.PlayerCamPosSet();
        
        GameSceneManager.Goto(GameDefine.GAME_START);
	}

	public override void OnExit (PhaseBase nextPhase)
	{

	}
}
