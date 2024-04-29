using UnityEngine;
using System.Collections;

public class End_Finish : PhaseBase
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
        base.OnEnter(prevPhase);

        // タイトル画面へ戻す
        //Start画面に戻すためのフラグセット
        SaveManager.Instance.tmpFromEnding = true;
        CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() => {
            MainSceneManager.Goto("Title");
        }));
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        base.OnExit(nextPhase);

    }
}