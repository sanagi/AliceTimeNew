using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    const int REFRESH_RATE = 60;    // 60fps

    public bool isInitialization = false; //初期化完了フラグ

    private MainSceneManager sceneManager;
    private Audio_Manage audioManager;
    private SaveManager saveManager;
    private DebugManager debugManager;
    private PrefabPoolManager prefabPoolManager;
    private EffectManager effectManager;

    static private ApplicationManager _instance;
    static public ApplicationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ApplicationManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        sceneManager = FindObjectOfType<MainSceneManager>();
        audioManager = FindObjectOfType<Audio_Manage>();
        saveManager = FindObjectOfType<SaveManager>();
        effectManager = FindObjectOfType<EffectManager>();

        debugManager = FindObjectOfType<DebugManager>();
    }

    private void InitializeApplication()
    {
        // フレームレートの設定
        Application.targetFrameRate = REFRESH_RATE;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        InitializeApplication();

        if (sceneManager == null)
        {
            sceneManager = (new GameObject(GameDefine.SCENE_MANAGER)).AddComponent<MainSceneManager>();
            sceneManager.transform.SetParent(transform);
        }

        if (saveManager == null)
        {
            saveManager = (new GameObject(GameDefine.SAVE_MANAGER)).AddComponent<SaveManager>();
            saveManager.transform.SetParent(transform);
        }

        if (debugManager == null)
        {
            debugManager = (new GameObject(GameDefine.DEBUG_MANAGER)).AddComponent<DebugManager>();
            debugManager.transform.SetParent(transform);
        }

        if (prefabPoolManager == null)
        {
            prefabPoolManager = (new GameObject(GameDefine.PREFAB_MANAGER)).AddComponent<PrefabPoolManager>();
            prefabPoolManager.transform.SetParent(transform);
        }

        if (audioManager == null)
        {
            audioManager = (new GameObject(GameDefine.AUDIO_MANAGER)).AddComponent<Audio_Manage>();
            audioManager.transform.SetParent(transform);
        }
        if (effectManager == null)
        {
            effectManager = (new GameObject(GameDefine.EFFECT_MANAGER)).AddComponent<EffectManager>();
            effectManager.transform.SetParent(transform);
        }
        

        sceneManager.Initialization();
        audioManager.Initialization();
        //effectManager.Initialization();

        isInitialization = true;
        
        //デバッグシーンからの起動でなければタイトルシーンへの遷移
        CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeIn(() =>
        {
            MainSceneManager.Goto(GameDefine.TITLE);
        }));
    }


    void OnDestroy()
    {
        DestroyPointer();
    }

    public void DestroyPointer()
    {
        sceneManager.Finalization();
        audioManager.Finalization();

        sceneManager = null;
        audioManager = null;
    }
}
