using UnityEngine;
using Rewired;
using UnityEngine.UI;

/// <summary>
/// 現在プレイ中の情報と進行管理を集約する管理クラス
/// ステージをクリアしたら破棄して次のプレイで作り直す
/// </summary>
public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    private RawImage _3dImage;
    
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

    [SerializeField]
    private Canvas _gameCanvas;

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

    void Awake()
    {
        sceneManager = (GameSceneManager)FindObjectOfType<GameSceneManager>();
        uiManager = (GameUIManager)FindObjectOfType<GameUIManager>();
    }

    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<GameSceneManager>();
            sceneManager.transform.SetParent(transform);
        }

        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<GameUIManager>();
            uiManager.transform.SetParent(transform);
        }

        sceneManager.Initialization();
        uiManager.Initialization(_gameCanvas);
        uiManager.Set3DRawImage(_3dImage);

        GameSceneManager.Goto(GameDefine.GAME_INIT);
    }
}