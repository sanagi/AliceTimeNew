using UnityEngine;
using Rewired;
using Cinemachine;

/// <summary>
/// 現在プレイ中のマップ選択マネージャ
/// </summary>
public class AreaSelectManager : MonoBehaviour
{
    public static void SetPlayFloor(int floorId)
    {
        CurrentFloorID = floorId;
    }
    /// <summary>
    /// 現在プレイ中の階数
    /// </summary>
    public static int CurrentFloorID = 0;
    
    private AreaSelectSceneManager sceneManager;
    private AreaSelectUIManager uiManager;

    void Awake()
    {
        sceneManager = (AreaSelectSceneManager)FindObjectOfType<AreaSelectSceneManager>();
        uiManager = (AreaSelectUIManager)FindObjectOfType<AreaSelectUIManager>(); //GameSceneとUI
    }

    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<AreaSelectSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<AreaSelectUIManager>();
            uiManager.transform.SetParent(transform);
        }

        sceneManager.Initialization();
        uiManager.Initialization();

        AreaSelectSceneManager.Goto(GameDefine.AREASELECT_INIT);
    }
    
    /// <summary>
    /// 心臓部に入ったときのメインカメラ設定
    /// </summary>
    public void CrateAreaSelectCamera(Vector3 cameraPos)
    {
        /*
        var cinemaBrain = cameraManager.MainCamera.gameObject.GetComponent<CinemachineBrain>();
        if (cinemaBrain == null)
        {
            cinemaBrain = cameraManager.MainCamera.gameObject.AddComponent<CinemachineBrain>();
            cinemaBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            cinemaBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
            cinemaBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        }

        //cameraManager.MainCamera.clearFlags = CameraClearFlags.SolidColor;
        //cameraManager.MainCamera.backgroundColor = Color.black;

        //VirtualCameraの親登録
        var rigParent = new GameObject(GameDefine.CAMERA_RIG);
        VirtualCameraRootTransform = rigParent.transform;
		
        var virtualCamera = GameObject.Instantiate(CameraManager.Instance.CameraParam.VirtualCameraFollow);
        virtualCamera.transform.parent = rigParent.transform;
        GamePlay3DCamera = virtualCamera.GetComponent<GamePlay3dCamera>();
        virtualCamera.transform.localPosition = cameraPos;
		
        //ゲームカメラ側で詳細設定
        GamePlay3DCamera.SetUpGameCamera(MainCamera, CameraParam.FovSize, CameraParam.FollowOffset);
        */
    }
}