using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// ゲームプレイ用のカメラ
/// </summary>
public class GamePlay3dCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera gameVirtualCamera;
    //private CinemachineFramingTransposer artViewCameraTransposer;

    private Camera _mainCamera;
    
    private Vector3 _followOffset;
    
    // Start is called before the first frame update
    public void SetUpGameCamera(Camera mainCamera, float fovSize, Vector3 followOffset)
    {
        _mainCamera = mainCamera;
        
        gameVirtualCamera.Follow = PlayerManager.Instance.CameraTarget;
        gameVirtualCamera.m_Lens.Orthographic = false;
        gameVirtualCamera.m_Lens.FieldOfView = fovSize;

        _mainCamera.enabled = false;
        _mainCamera.enabled = true;
        
        _followOffset = followOffset;
        
        //artViewCameraTransposer = gameVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        //artViewCameraTransposer.m_TrackedObjectOffset = _followOffset;

        gameVirtualCamera.LookAt = PlayerManager.Instance.CurrentPlayer.PlayerTransform;
    }

    /*public void RotateCameraStart()
    {
        _mainCamera.fieldOfView = _fovSize;
        
        gameVirtualCamera.m_Lens.Orthographic = false;
        gameVirtualCamera.m_Lens.FieldOfView = _fovSize;
        
        artViewCameraTransposer.m_TrackedObjectOffset = Vector3.zero;
    }

    public void RotateCameraEnd()
    {
        _mainCamera.orthographicSize = _orthoSize;
        
        gameVirtualCamera.m_Lens.Orthographic = true;
        gameVirtualCamera.m_Lens.OrthographicSize = _orthoSize;
        
        artViewCameraTransposer.m_TrackedObjectOffset = _followOffset;
    }
    
    public void ChangeTarget(Transform followTransform)
    {
        gameVirtualCamera.Follow = followTransform;
    }*/
}
