using UnityEngine;
using System.Collections;

// トライアルモードでステージをクリア後、次のステージを引き続き遊ぶ場合に遷移
public class Explore_NextStage : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		PlayerManager.Instance.CurrentPlayer.DisableControllable();
		
		((Explore)MainSceneManager.CurrentPhase).SelectedID = ExploreSceneManager.NextID;
		
		FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeOut(() =>
		{
			Scene3DStageManager.Instance.ResetStage();			
			
			GameObject.DestroyImmediate(PlayerManager.Instance.CurrentPlayer.gameObject);// プレイヤーの破棄
			
			//パーティクルの破棄
			
			//オーディオ関連の破棄

			switch (ExploreSceneManager.NextMainScene)
			{
				case MAINSCENE.AREASELECT:
					//次のフロアへ遷移
					MainSceneManager.Goto(GameDefine.AreaSelect);
					break;
				case MAINSCENE.EXPLORE:
					//探索シーンへ遷移
					ExploreSceneManager.Goto(GameDefine.EXPLORE_INIT);
					break;
				case MAINSCENE.GAME:
					//探索シーンへ遷移
					MainSceneManager.Goto(GameDefine.GAME);
					break;				
			}
		}));
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		// 時間に余裕があれば、ここで遷移前の状態を確認できれば最高
	}
}
