using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
public class DebugManager : SingletonMonoBehaviour<DebugManager> {
	/*public class DebugScene
	{
		public int stageId = -1;
		public Vector3 RespawnPos = new Vector3(-2.5f, -1.5f, 0f);
	}

	public DebugScene DebugSceneInfo = new DebugScene();
	public bool IsPlayDebugScene;
	
	//トリプルタップ判定用
	public float touchIntervalTime = 1.0f;
	private float touchStartTime;
	private int touchCount;

	// デバッグメニューが開いているかどうか
	[SerializeField] private bool isOpenDebugPanel;
	[SerializeField] private bool isShowExecInfo;

	private bool isStory;
	private int numStage;
	private int numArea;

	// メモリの使用量チェック
	private float lastCollect = 0f;
	private float lastCollectNum = 0f;
	private float delta = 0f;
	private float lastDeltaTime = 0f;
	private int allocRate = 0;
	private int lastAllocMemory = 0;
	private float lastAllocSet = -9999f;
	private int allocMem = 0;
	private int peakAlloc = 0;

	public int DebugFrag = 0;
	*/

	[SerializeField]
	private Text _displayPhaseText;
	
	[SerializeField]
	private Text _displayAreaNameText;
	
	private void Start()
	{
		DontDestroyOnLoad (gameObject);
	}

	public void SetCurrentPhase(string phaseName)
	{
		_displayPhaseText.text = phaseName;
		Debug.Log(phaseName);
	}

	public void SetAreaName(string areaName)
	{
		_displayAreaNameText.text = areaName;
	}

	public void Update()
	{
		//デバッグ機能
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//一時停止を呼ぶ
			Debug.LogError("[BREAK]");
		}
		else
		if (Input.GetKey(KeyCode.Tab))
		{
			Time.timeScale = 0f;
		}
		else
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Time.timeScale = 0.2f;
		}
		else
		if (Input.GetKey(KeyCode.LeftControl))
		{
			Time.timeScale = 4f;
		}
		else
		{
			//通常の再生
			Time.timeScale = 1;
		}
	}
	
	/*
	private const int LOG_MAX = 5;
    private Queue<string> logStack = new Queue<string>(LOG_MAX);

    [SerializeField]
    public Text logText;

    void Awake() 
	{
		touchStartTime = 0f;
		touchCount = 0;

		isOpenDebugPanel = false;
		isShowExecInfo = false;

		isStory = true;
		numStage = 0;
		numArea = 0;

        Application.logMessageReceived += LogCallback;  // ログが書き出された時のコールバック設定

        /*Debug.LogWarning("hogehoge");   // テストでワーニングログをコール

        GameObject tmp = GameObject.Find("DebugTextLog").gameObject;
        if (tmp != null)
        {
            logText = tmp.GetComponent<Text>();
        }
    }

	void Start()
	{
		if (UnityEngine.Debug.isDebugBuild) {
			AliceInputManager.RegisterTouchEventHandler (this);
		}
	}

	void OnDestroy()
	{
		if (UnityEngine.Debug.isDebugBuild) {
			AliceInputManager.UnregisterTouchEventHandler (this);
		}
	}

	#region ITouchEventHandler implementation

	public bool OnTouchEventBegan (KamioriInput.TouchInfo[] touchInfo)
	{
		if (isOpenDebugPanel) {
			return false;
		}

		// デバッグモードを開く(PC用)
		if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) {
			isOpenDebugPanel = true;
		}

		// デバッグモードを開く(スマホ用)
		touchCount++;
		if (touchCount == 4) {
			if (Time.time - touchStartTime < touchIntervalTime) {
				isOpenDebugPanel = true;
			}
			touchCount = 0;
		} else if (touchCount > 1) {
			if (Time.time - touchStartTime > touchIntervalTime) {
				touchCount = 0;
			}
		} else if (touchCount == 1) {
			touchStartTime = Time.time;
		}

		return false;
	}

	public bool OnTouchEventEnded (KamioriInput.TouchInfo[] touchInfo)
	{
		
		return false;
	}

	public bool OnTouchEventMoved (KamioriInput.TouchInfo[] touchInfo)
	{
		
		return false;
	}

	#endregion

	#region IInputEventHandler implementation

	public int Order {
		get {
			return 10000;
		}
	}

	public bool Process {
		get {
			return true;
		}
	}

    #endregion

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        // メインウィンドウ
        if (isOpenDebugPanel)
        {
			Vector2 guiScreenSize = new Vector2(1280,720);

			GUIUtility.ScaleAroundPivot(new Vector2(Screen.width/guiScreenSize.x,Screen.height/guiScreenSize.y),Vector2.zero);

            GUI.Box(new Rect(5, 5, 225, 450), "");

            isShowExecInfo = GUI.Toggle(new Rect(10, 10, 250, 40), isShowExecInfo, "Show Execution Info");

            if (MainSceneManager.CurrentPhase.GetType() != typeof(Game) && MainSceneManager.CurrentPhase.GetType() != typeof(End))
            {
                if (GUI.Toggle(new Rect(10, 60, 80, 40), isStory, "Story"))
                {
                    isStory = true;
                }
                if (GUI.Toggle(new Rect(120, 60, 80, 40), !isStory, "Trial"))
                {
                    isStory = false;
                }

                GUI.Label(new Rect(10, 95, 210, 30), "StageID");
                if (GUI.Button(new Rect(70, 95, 30, 30), "-"))
                {
                    numStage--;
                }
                GUI.TextArea(new Rect(95, 95, 45, 30), numStage.ToString());
                if (GUI.Button(new Rect(170, 95, 30, 30), "+"))
                {
                    numStage++;
                }

                GUI.Label(new Rect(10, 120, 210, 50), "AreaID");
                if (GUI.Button(new Rect(70, 120, 30, 30), "-"))
                {
                    numArea--;
                }
                GUI.TextArea(new Rect(95, 120,45, 30), numArea.ToString());
                if (GUI.Button(new Rect(170, 120, 30, 30), "+"))
                {
                    numArea++;
                }

                if (GUI.Button(new Rect(10, 170, 210, 30), "GO"))
                {
                    var stageID = GetStageID(isStory, numStage, numArea);
                    SaveManager.Instance.Story_Hajime();
                    for (int i = 0; i < numStage; i++)
                    {
                        SaveManager.Instance.Write_ClearStoryStage(i);
                    }
                    SaveManager.Instance.Write_ClearStoryStage(numStage - 1);

                    GoGameScene(stageID);
                }
            }
            /*GUI.Label(new Rect(10, 360, 210, 30), "Fragment Num");
            if (GUI.Button(new Rect(70, 360, 30, 30), "-"))
            {
				DebugFrag--;
            }
            GUI.TextArea(new Rect(95, 360, 30, 30), GameManager.FragmentNumProp.ToString());
            if (GUI.Button(new Rect(130, 360, 30, 30), "+"))
            {
				DebugFrag++;
            }
            
			if (GUI.Button(new Rect(10, 205, 210, 30), "CLOSE"))
            {
                isOpenDebugPanel = false;
            }

            if (GUI.Button(new Rect(10, 240, 210, 30), "Clear"))
            {
				MainGameManager.GameClear();
            }
            if (GUI.Button(new Rect(10, 280, 210, 30), "EventAuto"))
            {
	            
            }
            // 強制的にエンディングを開始
            if (GUI.Button(new Rect(10, 320, 210, 30), "Start NormalEnd"))
            {
                MainSceneManager.Goto("End");
            }

            // メモリ使用量表示ウィンドウ
            if (isShowExecInfo)
            {
                CheckMemoryInfo();

                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.LowerLeft;
                style.fontSize = h / 50;
                style.normal.textColor = new Color(1f, 1f, 1f, 1f);

                Rect rect1 = new Rect(w * 0.25f + 5, h - 5, w, 0);
                Rect rect2 = new Rect(w * 0.25f + w * 2 / 5 + 5, h - 5, w, 0);

                GUI.Box(new Rect(w * 0.25f, h - (h / 50 * 6) - 10, w * 0.5f, h / 50 * 6 + 10), "");

                GUI.Label(rect1, "Last Collect Delta", style);
                GUI.Label(rect2, string.Format(": {0:0.000}ms", lastDeltaTime / 1000f), style);
                rect1.y -= h / 50;
                rect2.y -= h / 50;
                GUI.Label(rect1, "Collection Frequency", style);
                GUI.Label(rect2, string.Format(": {0:0.000}ms", delta / 1000f), style);
                rect1.y -= h / 50;
                rect2.y -= h / 50;
                GUI.Label(rect1, "Allocation Rate", style);
                GUI.Label(rect2, string.Format(": {0:0.0}MB", allocRate / 1000000f), style);
                rect1.y -= h / 50;
                rect2.y -= h / 50;
                GUI.Label(rect1, "Peak Allocated", style);
                GUI.Label(rect2, string.Format(": {0:0.0}MB", peakAlloc / 1000000f), style);
                rect1.y -= h / 50;
                rect2.y -= h / 50;
                GUI.Label(rect1, "Currently Allocated", style);
                GUI.Label(rect2, string.Format(": {0:0.0}MB", allocMem / 1000000f), style);
                rect1.y -= h / 50;
                rect2.y -= h / 50;
                GUI.Label(rect1, "FramesPerSecond", style);
                GUI.Label(rect2, string.Format(": {0:0.0}fps", 1f / lastDeltaTime), style);
            }
        }
    }

	private void CheckMemoryInfo() 
	{
		int collCount = System.GC.CollectionCount (0);

		if (lastCollectNum != collCount) {
			lastCollectNum = collCount;
			delta = Time.realtimeSinceStartup - lastCollect;
			lastCollect = Time.realtimeSinceStartup;
			lastDeltaTime = Time.deltaTime;
		}

		allocMem = (int)System.GC.GetTotalMemory (false);

		peakAlloc = allocMem > peakAlloc ? allocMem : peakAlloc;

		if (Time.realtimeSinceStartup - lastAllocSet > 0.3f) {
			int diff = allocMem - lastAllocMemory;
			lastAllocMemory = allocMem;
			lastAllocSet = Time.realtimeSinceStartup;

			if (diff >= 0) {
				allocRate = diff;
			}
		}
	}

	private void GoGameScene(string stageID)
	{
		if (MainSceneManager.CurrentPhase.GetType () == typeof(Game)) {
			((Game)MainSceneManager.CurrentPhase).StageID = GetStageID (isStory, numStage - 1, numArea);
			GameSceneManager.Goto ("Game_NextStage");
		} else if (MainSceneManager.CurrentPhase.GetType () == typeof(Title)) {
			((Title)MainSceneManager.CurrentPhase).SelectedID = stageID;
            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() => {
                MainSceneManager.Goto("Game");
            }));
        } else if (MainSceneManager.CurrentPhase.GetType () == typeof(AreaSelect)) {
			((AreaSelect)MainSceneManager.CurrentPhase).SelectedID = stageID;
            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() => {
                MainSceneManager.Goto("Game");
            }));
        }
	}

	private string GetStageID(bool isStory, int numStage, int numArea) 
	{
		return string.Format ("{0}{1}{2}", isStory ? 1 : 0, numStage.ToString ("D2"), numArea.ToString ("D2"));
	}

    /// <summary>
    /// ログを取得するコールバック
    /// </summary>
    /// <param name="condition">メッセージ</param>
    /// <param name="stackTrace">コールスタック</param>
    /// <param name="type">ログの種類</param>
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        string trace = null;
        string color = null;

        switch (type)
        {
            case LogType.Log:
                trace = stackTrace;
                color = "white";
                break;
            case LogType.Warning:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                //trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                //color = "yellow";
                break;
            case LogType.Error:
            case LogType.Assert:
                //UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "red";
                break;
            case LogType.Exception:
                trace = stackTrace;
                color = "red";
                break;
        }

        // ログの行制限
        if (this.logStack.Count == LOG_MAX)
            this.logStack.Dequeue();

        string message = string.Format("<color={0}>{1}</color> <color=white>on {2}</color>", color, condition, trace);
        this.logStack.Enqueue(message);
        //logText.text = "";
        //logText.text = GetLog();
    }

    /// <summary>
    /// エラーログ表示
    /// </summary>
    public string GetLog()
    {
        string log = "";
        foreach(string _log in logStack)
        {
            log += _log + "\n";
        }
        return log;
    }
    */
}
#endif
