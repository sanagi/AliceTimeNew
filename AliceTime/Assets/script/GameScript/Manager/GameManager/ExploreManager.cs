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
        uiManager.Initialization();

        ExploreSceneManager.Goto(GameDefine.EXPLORE_INIT);
    }
}