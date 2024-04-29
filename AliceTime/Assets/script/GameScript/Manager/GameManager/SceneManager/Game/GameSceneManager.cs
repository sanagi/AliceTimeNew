using System;
using UnityEngine;
using System.Collections;

public enum GAMESCENE
{
    INIT, START, MAIN, CAMERA, DEATH, EVENT, PAUSE, NEXT, GIMICK
};

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private static PhaseStateMachine stateMachine;

    public static bool initialized = false;

    /// <summary>フェーズの開始時にManagerから呼ばれる</summary>
    public void Initialization()
    {

        stateMachine = GetComponent<PhaseStateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<PhaseStateMachine>();
        }

        // フェーズの登録
        stateMachine.RegisterPhase(new Game_CameraMove());
        stateMachine.RegisterPhase(new Game_Death());
        stateMachine.RegisterPhase(new Game_Event());
        stateMachine.RegisterPhase(new Game_Init());
        stateMachine.RegisterPhase(new Game_Main());
        stateMachine.RegisterPhase(new Game_NextStage());
        stateMachine.RegisterPhase(new Game_Pause());
        stateMachine.RegisterPhase(new Game_Start());
        stateMachine.RegisterPhase(new Game_Gimick());

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

    public static GAMESCENE CurrentPhaseState
    {
        get
        {
            switch (stateMachine.CurrentPhase.PhaseName)
            {
                case GameDefine.GAME_INIT:
                    return GAMESCENE.INIT;
                case GameDefine.GAME_START:
                    return GAMESCENE.START;
                case GameDefine.GAME_MAIN:
                    return GAMESCENE.MAIN;
                case GameDefine.GAME_PAUSE:
                    return GAMESCENE.PAUSE;
                case GameDefine.GAME_DEATH:
                    return GAMESCENE.DEATH;
                case GameDefine.GAME_EVENT:
                    return GAMESCENE.EVENT;
                case GameDefine.GAME_GIMICK:
                    return GAMESCENE.GIMICK;
                case GameDefine.GAME_CAMERA:
                    return GAMESCENE.CAMERA;
            }
            return GAMESCENE.INIT;
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