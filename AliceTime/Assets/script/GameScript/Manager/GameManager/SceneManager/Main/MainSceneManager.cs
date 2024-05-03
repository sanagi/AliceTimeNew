using UnityEngine;

public enum MAINSCENE {
	TITLE, GAME, AREASELECT, EXPLORE, END
};

public class MainSceneManager : MonoBehaviour {
	[SerializeField]
	private static PhaseStateMachine stateMachine;
	public static bool init = false;

	/// <summary>フェーズの開始時にManagerから呼ばれる</summary>
	public void Initialization() 
	{
		stateMachine = GetComponent<PhaseStateMachine> ();
		if (stateMachine == null) {
			stateMachine = gameObject.AddComponent<PhaseStateMachine> ();
		}

		// フェーズの登録
		stateMachine.RegisterPhase(new Title());
		stateMachine.RegisterPhase(new AreaSelect());
		stateMachine.RegisterPhase(new Explore());
		stateMachine.RegisterPhase(new Game());
		init = true;
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
			return stateMachine == null ? null : stateMachine.CurrentPhase;
		}
		set {
			stateMachine.CurrentPhase = value;
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
	}

	public static PhaseBase Find(string phaseName) 
	{
		return stateMachine.Find (phaseName);
	}
}
