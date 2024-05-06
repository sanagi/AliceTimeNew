using UnityEngine;
using System.Collections;
using Rewired;

// ゲームが進行している状態
public class Game_Main : PhaseBase
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
        player.controllers.maps.SetMapsEnabled(true, GameDefine.GEAR_GAME_REWIRED);

        PlayerManager.Instance.EnableControllable();
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        // [Rewired] disable key map
        var player = ReInput.players.GetPlayer(0);
        player.controllers.maps.SetMapsEnabled(false, GameDefine.GEAR_GAME_REWIRED);

        GameUIManager.HidePanel(GAMESCENE.MAIN);    // Order:400
        
        PlayerManager.Instance.DisableControllable();
    }
}
