using UnityEngine;
using Rewired;
using Cinemachine;
using UnityEngine.UI;

/// <summary>
/// 現在プレイ中のマップ選択マネージャ
/// </summary>
public class AreaSelectManager : SingletonMonoBehaviour<AreaSelectManager>
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

    [SerializeField]
    private CameraParam _areaCameraParam;

    [SerializeField]
    private Canvas _areaSelectCanvas;

    [SerializeField]
    private RawImage _3dImage;
    
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
        uiManager.Initialization(_areaSelectCanvas);
        uiManager.Set3DRawImage(_3dImage);

        AreaSelectSceneManager.Goto(GameDefine.AREASELECT_INIT);
    }

    /// <summary>
    /// 心臓部に入ったときのメインカメラ設定
    /// </summary>
    public void CrateAreaSelectCamera(Vector3 cameraPos)
    {
        var mainCamera = CameraManager.Instance.GetMainCamera();
        var cinemaBrain = CameraManager.Instance.GetMainCamera().gameObject.GetComponent<CinemachineBrain>();
        if (cinemaBrain == null)
        {
            cinemaBrain = CameraManager.Instance.GetMainCamera().gameObject.AddComponent<CinemachineBrain>();
            cinemaBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            cinemaBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
            cinemaBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        }
        
        //VirtualCameraの親登録
        var rigParent = new GameObject(GameDefine.CAMERA_RIG);

        var virtualCamera = GameObject.Instantiate(_areaCameraParam.VirtualCameraFollow);
        virtualCamera.transform.parent = rigParent.transform;
        var gamePlay3DCamera = virtualCamera.GetComponent<GamePlay3dCamera>();
        virtualCamera.transform.localPosition = cameraPos;
		
        //詳細設定
        gamePlay3DCamera.SetUpGameCamera(mainCamera, _areaCameraParam.FovSize, _areaCameraParam.FollowOffset);
    }
}