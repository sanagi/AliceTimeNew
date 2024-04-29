using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class End : PhaseBase
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
        Audio_Manage.StopBGM();
        LoadManager.Instance.Transition("End", LoadSceneMode.Single, () => { });
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        Audio_Manage.StopBGM();
    }
}
