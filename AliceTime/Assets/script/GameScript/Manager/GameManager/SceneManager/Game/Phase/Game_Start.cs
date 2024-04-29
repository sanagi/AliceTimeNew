using UnityEngine;
using System.Collections;

public class Game_Start : PhaseBase {
    PlayerController playerController;
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
    {
	    // キャラクターの位置をリスポーン
		//PlayerManager.Instance.Respawn ();
        //PlayerController.hijackMove = null;
        //PlayerController.hijackAnim = null;
        //PlayerManager.isGimickTouch = false;

        PlayerManager.Instance.SetState(PlayerController.STATE.WAIT);

        /*int playerLayer = LayerMask.NameToLayer("Player");
        int blockLayer = LayerMask.NameToLayer("Block");
        int liftLayer = LayerMask.NameToLayer("Lift");

        Physics.IgnoreLayerCollision(playerLayer, blockLayer, false);
        Physics.IgnoreLayerCollision(playerLayer, liftLayer, false);
		*/
        
        AliceInputManager.ClearInput();

        // 相棒の位置を補正
        //AibouManager.MoveToPosition (GameManager.RespawnPosition + Vector3.up, 0);
        //AibouManager.SetLayer("FrontGUI");

		// カメラにプレイヤーをセット
        //Camera_Move_Dangion.Instance.PlayerSet();
		//Camera_Move_Dangion.Instance.PosSetP();
		//Camera_Move_Dangion.Instance.PlayerCamPosSet();

        // ゲーム開始
        GameSceneManager.Goto(GameDefine.GAME_MAIN);
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		
	}
}
