 using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {
	private TitleSceneManager sceneManager;
	private TitleUIManager uiManager;
	
	public GameObject TsuzukiButton;
	public GameObject BackGroundLogoJpg;
    public GameObject BackGroundLogoEng;
    public GameObject BackGround;

    public bool init = false;
	public bool wait = false;

	public bool trialNextOpen = false;
	public bool openAnimation = false;

	static private TitleManager _instance;
	static public TitleManager Instance
	{
		get
		{
			if (_instance == null) {
				_instance = FindObjectOfType<TitleManager>();
			}
			return _instance;
		}
	}

    void Awake()
    {
        sceneManager = (TitleSceneManager)FindObjectOfType<TitleSceneManager>();
        uiManager = (TitleUIManager)FindObjectOfType<TitleUIManager>();
    }

    void Start() 
	{
		StartCoroutine (Initialization ());
	}

	IEnumerator Initialization() {
        init = false;
        wait = false;

        // Check MainManager
        var applicationManager = SystemManager.Instance;

		// Wait Init MainManager
		while (!applicationManager.IsInitialized) {
			yield return null;
		}

		// Check localManagers 
		if (sceneManager == null) {
			sceneManager = (new GameObject (GameDefine.SCENE_MANAGER)).AddComponent<TitleSceneManager> ();
			sceneManager.transform.SetParent (transform);
		}
		if (uiManager == null) {
			uiManager = (new GameObject (GameDefine.TITLE_UI_MANAGER)).AddComponent<TitleUIManager> ();
			uiManager.transform.SetParent (transform);
		}

		sceneManager.Initialization ();
		uiManager.Initialization ();
		uiManager.SetTsuzuki(TsuzukiButton);

		TitleSceneManager.Goto (GameDefine.TITLE_INIT);
	
		yield break;
	}

	void Update(){
        if (wait && !init && !SaveManager.Instance.tmpFromEnding) {
	        //最初の起動
            if (!Application.isShowingSplashScreen) {
                init = true;
                wait = false;
                TitleSceneManager.Goto(GameDefine.TITLE_LOGO);
            }
        }
        else if(wait && !init && SaveManager.Instance.tmpFromEnding)
        {
	        //EDから帰ってきたとき
            init = true;
            wait = false;
            SaveManager.Instance.tmpFromEnding = false;
            TitleSceneManager.Goto(GameDefine.TITLE_START);
        }
	}

	void OnDestroy()
	{
		sceneManager.Finalization ();
		uiManager.Finalization ();

		sceneManager = null;
		uiManager = null;
	}
}
