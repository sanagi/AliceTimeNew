using UnityEngine;
using System.Collections;

// トライアルモードでステージをクリア後、次のステージを引き続き遊ぶ場合に遷移
public class AreaSelect_NextStage : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		PlayerManager.Instance.CurrentPlayer.DisableControllable();
		
		((AreaSelect)MainSceneManager.CurrentPhase).SelectedID = AreaSelectSceneManager.NextID;
		
		LoadManager.Instance.StartCoroutine(LoadManager.Instance.FadeOut(() =>
		{
			Scene3DStageManager.Instance.ResetStage();			

			GameObject Player = GameObject.Find(GameDefine.PLAYER);
			GameObject.Destroy(Player);// プレイヤーの破棄
			
			//パーティクルの破棄
			
			//オーディオ関連の破棄

			switch (AreaSelectSceneManager.NextMainScene)
			{
				case MAINSCENE.AREASELECT:
					//次のフロアへ遷移
					AreaSelectSceneManager.Goto(GameDefine.AREASELECT_INIT);
					break;
				case MAINSCENE.EXPLORE:
					//探索シーンへ遷移
					MainSceneManager.Goto(GameDefine.Explore);
					break;
			}
		}));
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		// 時間に余裕があれば、ここで遷移前の状態を確認できれば最高
	}
}
