using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class End_Init : PhaseBase
{
    /*** エンディング判定のしきい値 ***/
    public int BAD_BORDER = 12;

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

        // 各種マネージャの不整合を排除

        // セーブデータを参照して獲得した勾玉の数を取得
        int fragmentCount = SaveManager.Instance.GetAllFragment();
        /*var badTipsText = EndManager.Instance.badText;
        if (badTipsText != null)
        {
            badTipsText.GetComponent<Text>().text += fragmentCount + "/12";
        }
        var normalTipsText = EndManager.Instance.normalText;
        if (normalTipsText != null)
        {
            normalTipsText.GetComponent<Text>().text += fragmentCount + "/12";
        }*/
#if UNITY_EDITOR
        Debug.Log("勾玉獲得数: " + fragmentCount);
#endif
        //Saveはエンディングに入ってすぐ
        if (fragmentCount < BAD_BORDER)
        {
            SaveManager.Instance.ClearBadStory();
        }
        else
        {
            SaveManager.Instance.ClearAllStory();
        }

        EndManager.Instance.endingController.StartStaffRoll(() =>
        {
            if (fragmentCount < BAD_BORDER)
            {
                EndSceneManager.Goto("End_Bad");
            }
            else
            {
                EndSceneManager.Goto("End_True");
            }
        });

        CameraManager.Instance.AspectChange(false);
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        base.OnExit(nextPhase);

    }
}