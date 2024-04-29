using UnityEngine;
using System.Collections;

/*** エンディングシーンの種類 ***/
public enum ENDSCENE{
    NONE, INIT, TRUE, NORMAL, BAD, FINISH
}

public class EndSceneManager : MonoBehaviour {
    [SerializeField]
    private static PhaseStateMachine stateMachine;

    public static bool initialized = false;

    public void Initialization()
    {
        stateMachine = GetComponent<PhaseStateMachine>();
        if(stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<PhaseStateMachine>();
        }

        // フェーズの登録
        stateMachine.RegisterPhase(new End_Init());
        stateMachine.RegisterPhase(new End_True());
        stateMachine.RegisterPhase(new End_Normal());
        stateMachine.RegisterPhase(new End_Bad());
        stateMachine.RegisterPhase(new End_Finish());

        initialized = true;
    }

    public void Finalization()
    {
        // フェーズの登録解除
        stateMachine.UnredisterAllPhase();
        stateMachine = null;
    }

    public static PhaseBase CurrentPhase
    {
        get
        {
            return stateMachine.CurrentPhase;
        }
    }

    public static ENDSCENE CurrentPhaseState
    {
        get
        {
            switch(CurrentPhase.PhaseName)
            {
                case "End_Init":
                    return ENDSCENE.INIT;
                case "End_Normal":
                    return ENDSCENE.NORMAL;
                case "End_Bad":
                    return ENDSCENE.BAD;
                case "End_True":
                    return ENDSCENE.TRUE;
                case "End_Finish":
                    return ENDSCENE.FINISH;
            }
            return ENDSCENE.NONE;
        }
    }

    public static void Goto(PhaseBase phase)
    {
        stateMachine.Goto(phase);
    }

    public static void Goto(string phaseName)
    {
        #if UNITY_EDITOR
        Debug.Log("Move to " + phaseName);
        #endif
        stateMachine.Goto(phaseName);
    }
}
