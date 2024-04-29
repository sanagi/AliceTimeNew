using UnityEngine;
using System.Collections;

public enum TITLESCENE {
	START,STORY,DIALOG,OPTION,LOGO
};

public class TitleSceneManager : MonoBehaviour {
	[SerializeField]
	private static PhaseStateMachine stateMachine;

	/// <summary>フェーズの開始時にManagerから呼ばれる</summary>
	public void Initialization() 
	{
		stateMachine = GetComponent<PhaseStateMachine> ();
		if (stateMachine == null) {
			stateMachine = gameObject.AddComponent<PhaseStateMachine> ();
		}

		// フェーズの登録
		stateMachine.RegisterPhase(new Title_Init());
		stateMachine.RegisterPhase(new Title_Final ());
		stateMachine.RegisterPhase(new Title_Start());
		stateMachine.RegisterPhase(new Title_Story());
		stateMachine.RegisterPhase(new Title_Dialog ());
		stateMachine.RegisterPhase(new Title_Option());
        stateMachine.RegisterPhase(new Title_Logo());
	}

    public static TITLESCENE CurrentPhaseState
    {
        get
        {
            switch (stateMachine.CurrentPhase.PhaseName)
            {
                case GameDefine.TITLE_START:
                    return TITLESCENE.START;
                case GameDefine.TITLE_STORY:
                    return TITLESCENE.STORY;
                case GameDefine.TITLE_DIALOG:
                    return TITLESCENE.DIALOG;
                case GameDefine.TITLE_OPTION:
                    return TITLESCENE.OPTION;
                case GameDefine.TITLE_LOGO:
                    return TITLESCENE.LOGO;
            }
            return TITLESCENE.START;
        }
    }
    
    /// <summary>フェーズの終了時にManagerから呼ばれる</summary>
    public void Finalization()
	{
		// フェーズの登録解除
		stateMachine.UnredisterAllPhase();

		stateMachine = null;
	}

	/// <summary>現在のフェーズを返す</summary>
	public static PhaseBase CurrentPhase {
		get {
			return stateMachine.CurrentPhase;
		}
	}

	/// <summary>次のフェーズを指定して遷移</summary>
	public static void Goto(PhaseBase phase) 
	{
		stateMachine.Goto (phase);
	}

	/// <summary>次のフェーズ名を指定して遷移</summary>
	public static void Goto(string phaseName) 
	{
		stateMachine.Goto (phaseName);
		DebugManager.Instance.SetCurrentPhase(phaseName);
	}
}
