using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// ロゴ画面管理
/// </summary>
public class LogoManager : MonoBehaviour
{
    //ステップ
    private enum MainStep
    {
        Initialize,             //初期化
        Control_Init,           //操作待機
        Control_Wait,           //操作待機
        End                     //終了
    }


    private MainStep _mainStep = (MainStep)0;           //メインステップ

    private float _waitSec = 1f;

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
                //初期化
                //SoundManager.Instance.PlaySound(SoundId.XXXX);           //効果音 再生
                step++;
                break;

            case MainStep.Control_Init:
                if (!SystemManager.Instance.Waiting())
                {
                    _waitSec -= Time.deltaTime;
                    if (_waitSec <= 0)
                    {
                        step++;
                    }
                }
                break;
            case MainStep.Control_Wait:
                //メニュー切り替え
                SystemManager.Instance.ChangeMenuRequest(MenuId.Title);             //メニュー切り替え
                //全部終了したなら次へ
                step++;
                break;

            case MainStep.End:
                break;
        }

        //ステップ切り替え
        if (_mainStep != step)
        {
            _mainStep = step;
        }
    }
}

