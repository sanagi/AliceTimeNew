using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

//レイヤーの種類
public enum LayerId
{
    Ignore = -1,				//無視
    Default = 0,                //3D画面など
};


/// <summary>
/// メニュー管理
/// </summary>
public class SystemManager : SingletonMonoBehaviour<SystemManager>
{
    const int REFRESH_RATE = 60;    // 60fps
    
    //外部アクセス用
    public static SystemManager Instance;
    private static int _instanceCount = 0;
    
    private bool _firstChange = true;

    private Coroutine _changeMenuLog = null;

    [SerializeField]
    private MainSceneManager _sceneManager;
    [SerializeField]
    private SoundManager _soundManager;
    [SerializeField]
    private SaveManager _saveManager;
    [SerializeField]
    private DebugManager _debugManager;
    [SerializeField]
    private PrefabPoolManager _prefabPoolManager;
    [SerializeField]
    private EffectManager _effectManager;
    [SerializeField]
    private CameraManager _cameraManager;
    [SerializeField]
    private AliceInputManager _aliceInputManager;
    [SerializeField]
    private LoadManager _loadManager;
    [SerializeField]
    private FadeManager _fadeManager;    

    public bool IsInitialized = false;

    /// <summary>
    /// 初期化
    /// </summary>
    public static void Initialize()
    {
        //まだなければ作る
        if (null != Instance)
        {
            return;
        }
        GameObject sysMngObj = Instantiate(Resources.Load("System/__SystemManager")) as GameObject;
    }

    private void InitializeApplication()
    {
        // フレームレートの設定
        Application.targetFrameRate = REFRESH_RATE;
        
        //通常の再生
        Time.timeScale = 1;
        
        //Managerの取得(キホン、子にある)
        if (_soundManager == null)
        {
            _soundManager = (new GameObject(GameDefine.AUDIO_MANAGER)).AddComponent<SoundManager>();
            _soundManager.transform.SetParent(transform);
        }
        
        if (_sceneManager == null)
        {
            _sceneManager = (new GameObject(GameDefine.SCENE_MANAGER)).AddComponent<MainSceneManager>();
            _sceneManager.transform.SetParent(transform);
        }

        if (_saveManager == null)
        {
            _saveManager = (new GameObject(GameDefine.SAVE_MANAGER)).AddComponent<SaveManager>();
            _saveManager.transform.SetParent(transform);
        }

#if UNITY_EDITOR
        if (_debugManager == null)
        {
            _debugManager = (new GameObject(GameDefine.DEBUG_MANAGER)).AddComponent<DebugManager>();
        }
#endif        

        if (_prefabPoolManager == null)
        {
            _prefabPoolManager = (new GameObject(GameDefine.PREFAB_MANAGER)).AddComponent<PrefabPoolManager>();
            _prefabPoolManager.transform.SetParent(transform);
        }
        
        if (_effectManager == null)
        {
            _effectManager = (new GameObject(GameDefine.EFFECT_MANAGER)).AddComponent<EffectManager>();
            _effectManager.transform.SetParent(transform);
        }
        if (_cameraManager == null)
        {
#if UNITY_EDITOR
            Debug.Log("カメラを登録してください");
#endif            
        }
        
        if (_aliceInputManager == null)
        {
            _aliceInputManager = (new GameObject(GameDefine.ALICE_INPUT_MANAGER)).AddComponent<AliceInputManager>();
        }
        if (_loadManager == null)
        {
            _loadManager = (new GameObject(GameDefine.LOAD_MANAGER)).AddComponent<LoadManager>();
        }
        if (_fadeManager == null)
        {
            _fadeManager = (new GameObject(GameDefine.FADE_MANAGER)).AddComponent<FadeManager>();
        }        
        
        //stateMachineの登録
        _sceneManager.Initialization();
        
        IsInitialized = true;
    }

    /// <summary>
    /// 起動時
    /// </summary>
    private void Awake()
    {
        _instanceCount++;
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        //シーン切替時に削除しないものに登録
        DontDestroyOnLoad(gameObject);
        Instance = this;
        
        //初期化
        InitializeApplication();
        
        //タイトルシーンへの遷移
        MainSceneManager.Goto(GameDefine.TITLE);

    }
    /// <summary>
    /// オブジェクト破棄時
    /// </summary>
    private void OnDestroy()
    {
        if (--_instanceCount <= 0)
        {
            //インスタンス破棄
            Instance = null;
        }
    }
}
