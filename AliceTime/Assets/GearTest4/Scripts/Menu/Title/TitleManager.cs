using UnityEngine;
using System.Collections;
using System.Collections.Generic;




/// <summary>
/// タイトル画面管理
/// </summary>
public class TitleManager : MonoBehaviour
{




    //ステップ
    private enum MainStep
    {
        Initialize,             //初期化
        Control_Init,           //操作待機
        Control_Wait,           //操作待機
        End,                    //終了
    }


    private MainStep _mainStep = (MainStep)0;           //メインステップ




    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        MainStep step = _mainStep;

        //現在のステップで処理を分岐
        switch (step)
        {
            case MainStep.Initialize:
                step++;
                break;

            case MainStep.Control_Init:
                if (!SystemManager.Instance.Waiting())
                {
                    step++;
                }
                break;
            case MainStep.Control_Wait:
                //操作待機
                if (InputManager.Instance.IsClick())
                {
                    //メニュー切り替え
                    SystemManager.Instance.ChangeMenuRequest(MenuId.Game);
                    step++;
                }
                break;

            case MainStep.End:
                //終了
                break;
        }

        //ステップ切り替え
        if (_mainStep != step)
        {
            _mainStep = step;
        }
    }


}

