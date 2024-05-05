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
        player.controllers.maps.SetMapsEnabled(true, GameDefine.SYSTEM_REWIRED);

        //TouchEffectManger.Instance.dragEffect = false;
        TitleManager.Instance.wait = true;
	}

	public override void OnExit (PhaseBase nextPhase)
	{
		
	}
}
