using UnityEngine;
using System.Collections;
using Rewired;

// ゲームが進行している状態
public class AreaSelect_Main : PhaseBase
{
    public override string PhaseName
    {
        get
        {
            return this.GetType().FullName;
        }
    }

    public override void OnEnter(PhaseBase prevPhase)
    {
        // [Rewired] キーマップを登録
        var player = ReInput.players.GetPlayer(0);
        player.controllers.maps.SetAllMapsEnabled(false);
        player.controllers.maps.SetMapsEnabled(true, GameDefine.EXPLORE_MAPS_REWIRED);

        PlayerManager.Instance.EnableControllable();

        //KamioriInputManager.ClearInput();

#if UNITY_ANDROID || UNITY_IOS
        if ((!EventManager_K.Instance.isOffVirtualCanvas || (GameManager.StageID == 0 && GameManager.AreaID == 0 && prevPhase.PhaseName == "Game_Event"))
           && (Camera_Move_Dangion.Instance.IsTargetCharacter() || prevPhase.PhaseName != "Game_CameraMove")) {
            KamioriInput.VirtualControllerEvent.EnableVirtualController();  // Order:100
        }
#endif
        KeyPointManager.EnableController();
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        // [Rewired] disable key map
        var player = ReInput.players.GetPlayer(0);
        player.controllers.maps.SetMapsEnabled(false, GameDefine.EXPLORE_MAPS_REWIRED);

#if UNITY_ANDROID || UNITY_IOS
		KamioriInput.VirtualControllerEvent.DisableVirtualController (); // Order:100
#endif

        KeyPointManager.DisableController();
        PlayerManager.Instance.DisableControllable();

        if (nextPhase.PhaseName != "Game_CameraMove")
        {
            //KamioriInputManager.ClearInput();
        }
    }
}
