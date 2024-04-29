using UnityEngine;
using Rewired;

/// <summary>
/// 現在プレイ中の情報と進行管理を集約する管理クラス
/// ステージをクリアしたら破棄して次のプレイで作り直す
/// </summary>
public class MainGameManager : MonoBehaviour
{
    public static void SetPlayStage(int stageId)
    {
        CurrentStageID = stageId;
    }
    /// <summary>
    /// 現在プレイ中のステージID
    /// </summary>
    public static int CurrentStageID = 0;

    /// <summary>
    /// プレイ中のエリアID(つけるなら)
    /// </summary>
    public static int CurrentAreaID = 0;
    
    /// <summary>
    /// Respawn位置
    /// </summary>
    public static Vector3 RespawnPosition;

    public static void UpdateRespawnPosition(Vector3 pos)
    {
        RespawnPosition.x = pos.x;
        RespawnPosition.y = pos.y;
        RespawnPosition.z = pos.z;
    }
    
    public static void GameClear()
    {
        
    }

    public static void SetRotateChild()
    {
        
    }
    
    private GameSceneManager sceneManager;
    private GameUIManager uiManager;
    //private StageRotateController stageRotateController;

    private GameAudioController audioController;
    
    void Awake()
    {
        sceneManager = (GameSceneManager)FindObjectOfType<GameSceneManager>();
        uiManager = (GameUIManager)FindObjectOfType<GameUIManager>();

        audioController = (GameAudioController)FindObjectOfType<GameAudioController>();
        //stageRotateController = (StageRotateController)FindObjectOfType<StageRotateController>();
    }

    void Start()
    {
        /*var applicationManager = FindObjectOfType<ApplicationManager>();
        if (applicationManager == null)
            (new GameObject("MainManager")).AddComponent<ApplicationManager>();
        */

        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<GameSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (audioController == null)
        {
            audioController = (new GameObject("AudioController")).AddComponent<GameAudioController>();
            audioController.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<GameUIManager>();
            uiManager.transform.SetParent(transform);
        }
        /*if (stageRotateController == null)
        {
            stageRotateController = (new GameObject("RotateCenter")).AddComponent<StageRotateController>();
        }
        */

        sceneManager.Initialization();
        uiManager.Initialization();
        //stageRotateController.Initialize();

        GameSceneManager.Goto(GameDefine.GAME_INIT);
    }
}

/*
public enum GAMEMODE
{
    TRIAL, STORY
}
*/

/*public class GameManager : MonoBehaviour
{
    const string ACTION_SWITCH_JUMP_OR_TOUCH_AIBOU = "Switch Jump or Touch Aibou";

    /***************
     * Game Params
     **************
    [SerializeField]
    public static string PlayID = "";
    public static Vector3 RespawnPosition;

    private GameSceneManager sceneManager;
    private GameUIManager uiManager;

    private GameAudioController audioController;

    // プレイヤーコントロール用
    private Player player;

    public static GAMEMODE GameMode
    {
        get
        {
            if (int.Parse(PlayID.Substring(0, 1)) == 0)
            {
                return GAMEMODE.TRIAL;
            }
            else
            {
                return GAMEMODE.STORY;
            }
        }
    }

    public static int StageID
    {
        get
        {
            return int.Parse(PlayID.Substring(1, 2));
        }
        set
        {
            var newID = ((int)GameMode).ToString() + value.ToString("D2") + AreaID.ToString("D2");
            ((Game)MainSceneManager.CurrentPhase).StageID = newID;
            PlayID = newID;
        }
    }

    public static int AreaID
    {
        get
        {
            return int.Parse(PlayID.Substring(3, 2));
        }
        set
        {
            var newID = ((int)GameMode).ToString() + StageID.ToString("D2") + value.ToString("D2");
            ((Game)MainSceneManager.CurrentPhase).StageID = newID;
            PlayID = newID;
        }
    }



    /*
     * シーケンスマネージャの闇
     * ここから
    

    private static int Ticket_Num;//トライアルモードで集めた勾玉の数

    /// <summary>プロローグイベントの評価・実行フラグ</summary>
    private static bool m_isNovel = false;

    private static int FragmentNum;//集めたキオクの勾玉の数

    public static GameObject Master;
    public static GameObject InputMaster;
    public static GameObject Player;
    static PlayerController Con;
    public static GameObject ReadMaster;
    public static GameObject UIMaster;

    public const float ROTATETIME = 0.25f;

    public static int StoryStage;//現在クリアしているステージ

    public static int TutorialNum;//現在のチュートリアル数

    public static int Ticket_NumProp
    {
        set { Ticket_Num = value; }
        get { return Ticket_Num; }
    }
    public static int FragmentNumProp
    {
        get
        {
            FragmentNum = SaveManager.Instance.GetAllFragment();
            return FragmentNum;
        }
        set { FragmentNum = value; }
    }

    public static bool ISNovel
    {
        set { m_isNovel = value; }
        get { return m_isNovel; }
    }


    /*
     * シーケンスマネージャの闇
     * ここまで
     


    public enum ControlTarget
    {
        None,
        Player,
        Aibou
    }

    private static ControlTarget controlTarget;

    public static bool IsPlayerControllable()
    {
        return controlTarget == ControlTarget.Player;
    }

    public static bool IsAibouControllable()
    {
        return controlTarget == ControlTarget.Aibou;
    }

    public static bool isChangeAnimation = false;
    private float changeTimer = 0f;
    private float initRotate = 0f;

    void Awake()
    {
        sceneManager = (GameSceneManager)FindObjectOfType<GameSceneManager>();
        uiManager = (GameUIManager)FindObjectOfType<GameUIManager>();

        audioController = (GameAudioController)FindObjectOfType<GameAudioController>();
    }

    void Start()
    {
        var applicationManager = FindObjectOfType<ApplicationManager>();
        if (applicationManager == null)
            (new GameObject("MainManager")).AddComponent<ApplicationManager>();

        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<GameSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (audioController == null)
        {
            audioController = (new GameObject("AudioController")).AddComponent<GameAudioController>();
            audioController.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<GameUIManager>();
            uiManager.transform.SetParent(transform);
        }

        sceneManager.Initialization();
        uiManager.Initialization();

        GameSceneManager.Goto("Game_Init");

        // for rewired
        player = ReInput.players.GetPlayer(0);
        controlTarget = ControlTarget.Player;
    }


    void Update()
    {
        if (!AibouManager.IsDisplay() || PlayerController.isJump)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        /*if ((KurehaController.IsControllable() == false) && (GameManager.StageID == 0 && EventManager_K.Instance.InterNextEvent == "Event0_1"))
        {
            return;
        }

        if (player.GetButtonDown(ACTION_SWITCH_JUMP_OR_TOUCH_AIBOU))
        {
            switch (controlTarget)
            {
                case ControlTarget.Aibou:
                    ChangeAnimation(ControlTarget.Player);
                    break;
                case ControlTarget.Player:
                    ChangeAnimation(ControlTarget.Aibou);
                    break;
            }
        }

        //操作チェンジアニメーション中
        if (isChangeAnimation)
        {
            //自前で回す
            switch (controlTarget)
            {
                case ControlTarget.Aibou:
                    PlayerController.PlayerTransform.localRotation = Quaternion.Euler(0f, (initRotate + 360f) * (changeTimer / ROTATETIME), 0f);
                    break;
                case ControlTarget.Player:
                    AibouManager.AibouController.transform.localRotation = Quaternion.Euler(0f, (initRotate + 360f) * (changeTimer / ROTATETIME), 0f);
                    break;
            }
            changeTimer += Time.deltaTime;
            if(changeTimer>= ROTATETIME)
            {
                OnCompleteChangeAnimation();
            }
        }
    }

    /// <summary>
    /// 操作入れ替えるアニメーション
    /// </summary>
    private void ChangeAnimation(ControlTarget a_controlTarget)
    {
        AibouManager.DisableController();
        PlayerController.DisableControllable();
        isChangeAnimation = true;
        changeTimer = 0f;

        Audio_Manage.Play(SoundEnum.SE_TXT);

        switch (a_controlTarget)
        {
            case ControlTarget.Aibou:
                initRotate = AibouManager.AibouController.transform.localRotation.eulerAngles.y;
                //iTween.RotateTo(AibouManager.AibouController.gameObject, iTween.Hash("y", 720f, "time", ROTATETIME, "easeType", "easeOutQuart"));
                break;
            case ControlTarget.Player:
                initRotate = PlayerController.PlayerTransform.localRotation.eulerAngles.y;
                //iTween.RotateTo(PlayerController.PlayerTransform.gameObject, iTween.Hash("y", 720f, "time", ROTATETIME, "easeType", "easeOutQuart","islocal",true));
                break;
        }
    }

    /// <summary>
    /// チェンジアニメーション終了時
    /// </summary>
    private void OnCompleteChangeAnimation()
    {
        AibouManager.EnableController();
        PlayerController.EnableControllable();
        switch (controlTarget)
        {
            case ControlTarget.Aibou:
                // エフェクトの処理
                var PlaerPos = PlayerController.PlayerTransform.gameObject.transform.position; PlaerPos.z = -8f;
                TouchEffectManger.Instance.TouchEffect.transform.position = PlaerPos;
                TouchEffectManger.Instance.TouchEffect.Play();

                controlTarget = ControlTarget.Player;
                PlayerController.PlayerTransform.localRotation = Quaternion.Euler(0f, (initRotate), 0f);

                break;
            case ControlTarget.Player:

                var AibouPos = AibouManager.AibouController.transform.position; AibouPos.x += AibouController.Offsettouch.x; AibouPos.y += AibouController.Offsettouch.y; AibouPos.z = -8f;
                TouchEffectManger.Instance.TouchEffect.transform.position = AibouPos;
                TouchEffectManger.Instance.TouchEffect.Play();

                AibouManager.AibouController.transform.localRotation = Quaternion.Euler(0f, (initRotate), 0f);
                controlTarget = ControlTarget.Aibou;
                break;
        }
        isChangeAnimation = false;
    }

    public static ControlTarget GetControlTarget()
    {
        return controlTarget;
    }

    void OnDestroy()
    {
        //sceneManager.Finalization ();
        //uiManager.Finalization ();

        sceneManager = null;
        audioController = null;
        uiManager = null;
    }



    public static void GameClear()
    {
        if (GameManager.GameMode == GAMEMODE.TRIAL) TrialClear();
        else if (GameManager.GameMode == GAMEMODE.STORY) StoryDangionClear();
    }

    static void TrialClear()
    {
        Audio_Manage.Play(SoundEnum.SE_CLEAR);
        if (!SaveManager.Instance.Check_ClearTrialStage(StageID))
        {
            SaveManager.Instance.Write_ClearTrialStage(StageID);
        }
        if (SaveManager.Instance.Read_ClearTrialStage().Length == 40 && SaveManager.Instance.GetTrueEnd())
        {
            //Start画面に戻すためのフラグセット
            SaveManager.Instance.tmpFromEnding = true;
            SaveManager.Instance.SetAllClear();
        }
        GameSceneManager.Goto("Game_Result");
    }

    public static void StoryDangionClear()
    {
    }

    public static void Off()
    {
        PlayerController.DisableControllable();
    }
    public static void On()
    {
        PlayerController.EnableControllable();
    }

}*/