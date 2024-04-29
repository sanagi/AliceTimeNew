using UnityEngine;
using System.Collections;

public class Title_Final : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		Audio_Manage.StopBGM ();
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		Debug.LogError ("くれは「タイトルシーンは終わってるはずだよ？」");
	}
}
