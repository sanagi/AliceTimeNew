using UnityEngine;
using System.Collections;

/// <summary>
/// カメラ管理
/// 現在のUIカメラとメインカメラを返すだけ
/// </summary>
public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [SerializeField]
    private Camera _uiCamera;
    public Camera GetUiCamera() => _uiCamera;
    
    public Camera GetMainCamera() => Camera.main;
    
    public Vector3 GetUiPos(Vector3 screenPos)
    {
        var uiPos = CameraManager.Instance.GetUiCamera().ScreenToWorldPoint(Input.mousePosition);
        uiPos.z = 0;
        return uiPos;
    }
}
