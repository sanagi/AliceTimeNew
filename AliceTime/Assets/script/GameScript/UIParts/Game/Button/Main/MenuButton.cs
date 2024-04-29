using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuButton : GameButtonBehaviour
{
    private Player player0;
    
    void Start()
    {
        player0 = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (player0.GetButtonDown("Open Menu"))
        {
            GotoNextPhase();
        }
    }
}
