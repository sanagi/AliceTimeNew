using System;
using UnityEngine;
using System.Collections;

public enum AREASELECT
{
    INIT, START, MAIN, CAMERA,  EVENT, PAUSE, NEXT
};

public class AreaSelectSceneManager : MonoBehaviour
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
        stateMachine.RegisterPhase(new AreaSelect_Event());
        stateMachine.RegisterPhase(new AreaSelect_Init());
        stateMachine.RegisterPhase(new AreaSelect_Main());
        stateMachine.RegisterPhase(new AreaSelect_NextStage());
        stateMachine.RegisterPhase(new AreaSelect_Pause());
        stateMachine.RegisterPhase(new AreaSelect_Start());
        stateMachine.RegisterPhase(new AreaSelect_CameraMove());

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

    public static AREASELECT CurrentPhaseState
    {
        get
        {
            switch (stateMachine.CurrentPhase.PhaseName)
            {
                case GameDefine.AREASELECT_INIT:
                    return AREASELECT.INIT;
                case GameDefine.AREASELECT_START:
                    return AREASELECT.START;
                case GameDefine.AREASELECT_MAIN:
                    return AREASELECT.MAIN;
                case GameDefine.AREASELECT_PAUSE:
                    return AREASELECT.PAUSE;
                case GameDefine.AREASELECT_EVENT:
                    return AREASELECT.EVENT;
                case GameDefine.AREASELECT_NEXT:
                    return AREASELECT.NEXT;
                case GameDefine.AREASELECT_CAMERA:
                    return AREASELECT.CAMERA;
            }
            return AREASELECT.INIT;
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