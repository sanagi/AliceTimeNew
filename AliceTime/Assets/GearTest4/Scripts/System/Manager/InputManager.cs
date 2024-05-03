using UnityEngine;
using System.Collections;


/*
    InputManager.Instance
 */


/// <summary>
/// 入力管理
/// </summary>
public class InputManager : CommonManagerBase
{
    //外部アクセス用
    public static InputManager Instance;
    private static int _instanceCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        _instanceCount++;
        if (Instance)
        {
            return;
        }
        //インスタンス設定
        Instance = this;
        //マルチタッチを許可
        Input.multiTouchEnabled = true;
    }

    /// <summary>
    /// 破棄時
    /// </summary>
    private void OnDisable()
    {
        if (--_instanceCount <= 0)
        {
            //インスタンス破棄
            Instance = null;
        }
    }

    private void Update()
    {
        if (IsClick())
        {
            var tempPos = CameraManager.Instance.GetUiPos(Input.mousePosition);
            EffectManager.Instance.PlayEffect(EffectId.TouchHit, tempPos);      //エフェクト発行
        }
    }


    public bool IsClick()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }
}
