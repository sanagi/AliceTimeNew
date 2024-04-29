using UnityEngine;
using System.Collections;

// ステージ上でイベントが発生している状態
public class Explore_Event : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		
	}
}
