using UnityEngine;
using System.Collections;

public class Explore_CameraMove : PhaseBase {
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
        /*if (GameManager.StageID == 2 && EventManager_K.Instance.InterNextEvent != "")
        {//次に控えてるイベントがあれば
            EventManager_K.Instance.EventConditionCheck("Ex");
        }*/
    }
}
