using UnityEngine;
using Rewired;

/// <summary>
/// 現在プレイ中の探検マネージャ
/// </summary>
public class ExploreManager : MonoBehaviour
{
    /// <summary>
    /// 現在プレイ中の部屋
    /// </summary>
    public static int CurrentStageID = 0;
    
    private ExploreSceneManager sceneManager;
    private ExploreUIManager uiManager;
    //private StageRotateController stageRotateController;

    private GameAudioController audioController;
    
    void Awake()
    {
        sceneManager = (ExploreSceneManager)FindObjectOfType<ExploreSceneManager>();
        uiManager = (ExploreUIManager)FindObjectOfType<ExploreUIManager>(); //GameSceneとUI

        audioController = (GameAudioController)FindObjectOfType<GameAudioController>();
        //stageRotateController = (StageRotateController)FindObjectOfType<StageRotateController>();
    }

    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<ExploreSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (audioController == null)
        {
            audioController = (new GameObject("AudioController")).AddComponent<GameAudioController>();
            audioController.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<ExploreUIManager>();
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

        ExploreSceneManager.Goto(GameDefine.EXPLORE_INIT);
    }
}