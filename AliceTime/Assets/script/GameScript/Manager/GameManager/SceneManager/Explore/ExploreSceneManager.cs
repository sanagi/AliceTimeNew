using System;
using UnityEngine;
using System.Collections;

public enum EXPLORESCENE
{
    INIT, START, MAIN,  CAMERA, EVENT, PAUSE, NEXT, GIMICK
};

public class ExploreSceneManager : MonoBehaviour
{
    [SerializeField]
    private static PhaseStateMachine stateMachine;

    public static bool initialized = false;
    
    /// <summary>
    /// 次のID
    /// </summary>
    public static string NextID = "0";
    
    /// <summary>
    /// 次の遷移シーン
    /// </summary>
    public static MAINSCENE NextMainScene;

    /// <summary>フェーズの開始時にManagerから呼ばれる</summary>
    public void Initialization()
    {

        stateMachine = GetComponent<PhaseStateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<PhaseStateMachine>();
        }

        // フェーズの登録
        stateMachine.RegisterPhase(new Explore_Init());
        stateMachine.RegisterPhase(new Explore_Start());
        stateMachine.RegisterPhase(new Explore_Event());
        stateMachine.RegisterPhase(new Explore_Main());
        stateMachine.RegisterPhase(new Explore_NextStage());
        stateMachine.RegisterPhase(new Explore_Pause());
        stateMachine.RegisterPhase(new Explore_CameraMove());
        stateMachine.RegisterPhase(new Explore_Gimick());

        initialized = true;
    }

    /// <summary>フェーズの終了時にManagerから呼ばれる</summary>
    public void Finalization()
    {
        // フェーズの登録解除
        stateMachine.UnredisterAllPhase();

        stateMachine = null;
    }

    /// <summary>現在のフェーズを返す</summary>
    public static PhaseBase CurrentPhase
    {
        get
        {
            return stateMachine.CurrentPhase;
        }
    }

    public static EXPLORESCENE CurrentPhaseState
    {
        get
        {
            if (stateMachine == null)
            {
                return EXPLORESCENE.INIT;
            }
            
            switch (stateMachine.CurrentPhase.PhaseName)
            {
                case GameDefine.EXPLORE_INIT:
                    return EXPLORESCENE.INIT;
                case GameDefine.EXPLORE_START:
                    return EXPLORESCENE.START;
                case GameDefine.EXPLORE_MAIN:
                    return EXPLORESCENE.MAIN;
                case GameDefine.EXPLORE_PAUSE:
                    return EXPLORESCENE.PAUSE;
                case GameDefine.EXPLORE_CAMERA:
                    return EXPLORESCENE.CAMERA;
                case GameDefine.EXPLORE_EVENT:
                    return EXPLORESCENE.EVENT;
                case GameDefine.EXPLORE_NEXT:
                    return EXPLORESCENE.NEXT;
                case GameDefine.EXPLORE_GIMICK:
                    return EXPLORESCENE.GIMICK;                
            }
            return EXPLORESCENE.INIT;
        }
    }

    /// <summary>次のフェーズを指定して遷移</summary>
    public static void Goto(PhaseBase phase)
    {
        stateMachine.Goto(phase);
#if UNITY_EDITOR
        Debug.Log ("Move to " + phase.PhaseName);
#endif        
    }

    /// <summary>次のフェーズ名を指定して遷移</summary>
    public static void Goto(string phaseName)
    {
        stateMachine.Goto(phaseName);
        DebugManager.Instance.SetCurrentPhase(phaseName);
    }
}