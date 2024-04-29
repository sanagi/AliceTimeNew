using UnityEngine;
using System.Collections;

public class Explore_Gimick : PhaseBase
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
        PlayerManager.Instance.DisableControllable();
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        PlayerManager.Instance.EnableControllable();
    }
}
