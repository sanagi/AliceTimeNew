using UnityEngine;
using System.Collections;


/*
    CameraManager.Instance
 */


/// <summary>
/// カメラ管理
/// </summary>
public class CameraManager : CommonManagerBase
{
    //外部アクセス用
    public static CameraManager Instance;
    private static int _instanceCount = 0;

    [SerializeField]
    private Camera _uiCamera;
    public Camera GetUiCamera() => _uiCamera;

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

    public Vector3 GetUiPos(Vector3 screenPos)
    {
        var uiPos = CameraManager.Instance.GetUiCamera().ScreenToWorldPoint(Input.mousePosition);
        uiPos.z = 0;
        return uiPos;
    }
}
