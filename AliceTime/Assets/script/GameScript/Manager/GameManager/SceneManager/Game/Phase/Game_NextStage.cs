using UnityEngine;
using System.Collections;

// トライアルモードでステージをクリア後、次のステージを引き続き遊ぶ場合に遷移
public class Game_NextStage : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeOut(() => {
			GearStageManager.Instance.ResetStage();			

			GameObject Player = GameObject.Find(GameDefine.PLAYER);
			GameObject.Destroy(Player);// プレイヤーの破棄

			//GameObject Aibou = GameObject.Find("aibou_Control");
			//GameObject.Destroy(Aibou);// ハルの破棄

			/*int i = GameManager.StageID;
			GameManager.StageID = i + 1;
			GameManager.AreaID = 0;
			*/
			
			// 次のステージ番号をGameフェーズへ登録
			
            //パーティクルの破棄

            // 全ての状態が初期化されたと判断したらGame_Initへ遷移
			GameSceneManager.Goto(GameDefine.GAME_INIT);
		}));
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		// 時間に余裕があれば、ここで遷移前の状態を確認できれば最高
	}
}
