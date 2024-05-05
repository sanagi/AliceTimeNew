using UnityEngine;
using Rewired;
using Cinemachine;
using UnityEngine.UI;

/// <summary>
/// 現在プレイ中の探検マネージャ
/// </summary>
public class ExploreManager : SingletonMonoBehaviour<ExploreManager>
{
    /// <summary>
    /// 現在プレイ中の部屋
    /// </summary>
    public static int CurrentStageID = 0;
    
    private ExploreSceneManager sceneManager;
    private ExploreUIManager uiManager;
    //private StageRotateController stageRotateController;
    
    [SerializeField]
    private CameraParam _exploreCameraParam;
    
    [SerializeField]
    private Canvas _exploreCanvas;

    [SerializeField]
    private RawImage _3dImage;
    
    void Awake()
    {
        sceneManager = (ExploreSceneManager)FindObjectOfType<ExploreSceneManager>();
        uiManager = (ExploreUIManager)FindObjectOfType<ExploreUIManager>(); //GameSceneとUI
    }

    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<ExploreSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<ExploreUIManager>();
            uiManager.transform.SetParent(transform);
        }

        sceneManager.Initialization();
        uiManager.Initialization(_exploreCanvas);
        uiManager.Set3DRawImage(_3dImage);

        ExploreSceneManager.Goto(GameDefine.EXPLORE_INIT);
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

        var virtualCamera = GameObject.Instantiate(_exploreCameraParam.VirtualCameraFollow);
        virtualCamera.transform.parent = rigParent.transform;
        var gamePlay3DCamera = virtualCamera.GetComponent<GamePlay3dCamera>();
        virtualCamera.transform.localPosition = cameraPos;
		
        //詳細設定
        gamePlay3DCamera.SetUpGameCamera(mainCamera, _exploreCameraParam.FovSize, _exploreCameraParam.FollowOffset);
    }
}