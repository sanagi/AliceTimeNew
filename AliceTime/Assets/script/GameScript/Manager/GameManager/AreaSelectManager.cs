using UnityEngine;
using Rewired;

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
    //private StageRotateController stageRotateController;

    private GameAudioController audioController;
    
    void Awake()
    {
        sceneManager = (AreaSelectSceneManager)FindObjectOfType<AreaSelectSceneManager>();
        uiManager = (AreaSelectUIManager)FindObjectOfType<AreaSelectUIManager>(); //GameSceneとUI

        audioController = (GameAudioController)FindObjectOfType<GameAudioController>();
        //stageRotateController = (StageRotateController)FindObjectOfType<StageRotateController>();
    }

    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<AreaSelectSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (audioController == null)
        {
            audioController = (new GameObject("AudioController")).AddComponent<GameAudioController>();
            audioController.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<AreaSelectUIManager>();
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

        AreaSelectSceneManager.Goto(GameDefine.AREASELECT_INIT);
    }
}