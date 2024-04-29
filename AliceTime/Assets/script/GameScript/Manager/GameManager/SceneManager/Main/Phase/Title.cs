using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class Title : PhaseBase {
	public string PrePhaseName;
	public string SelectedID;

	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{
		CameraManager.Instance.ResetPosition();
		DebugManager.Instance.SetAreaName("");

        PrePhaseName = prevPhase == null ? "" : prevPhase.PhaseName;
        LoadManager.Instance.Transition(GameDefine.TITLE, LoadSceneMode.Single, () => {
            // [Rewired] enable key map
            var player = ReInput.players.GetPlayer(0);
            player.controllers.maps.SetMapsEnabled(true, "System");
        });
    }

	public override void OnExit (PhaseBase nextPhase)
	{

    }
}
