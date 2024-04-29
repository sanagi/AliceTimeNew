using UnityEngine;
using System.Collections;

public class KeyPointManager : MonoBehaviour {
    public enum STATE
    {
        WAIT, HOLD, CONTROL, FIRE, CANCEL,
    }

    [SerializeField]
    private static STATE currentState;
    public static STATE CurrentState { get { return currentState; } }
    public static void SetState(STATE state)
    {
        if (_keyPointAnimation == null) return;
        currentState = state;
        _keyPointAnimation.SetAnimation(state);
    }
    
    // コントロールフラグ
    public static bool isControllable;

    // アニメーション管理
    private static KeyPointAnimation _keyPointAnimation;

    private static KeyPointController _keyPointController;
    public static KeyPointController KeyPointController{
        get
        {
            return _keyPointController;
        }
    }
    public static void EnableController()
    {
	    isControllable = true;
    }
    public static void DisableController()
    {
	    isControllable = false;
    }

	// 表示/非表示
	public static void Display() 
	{
		if (_keyPointController == null)
			return;
		
		_keyPointController.gameObject.SetActive (true);
	}
	public static void Hide()
	{
		if (_keyPointController == null)
			return;

		_keyPointController.gameObject.SetActive (false);
	}

	// 処理軽量化用のキャッシュ
	private PlayerController playerController;

	void Awake ()
	{
		currentState = STATE.WAIT;
		_keyPointAnimation = GetComponent<KeyPointAnimation>();
		_keyPointController = GetComponent<KeyPointController>();
	}

	void Update ()
	{
		if (_keyPointController == null) {
			_keyPointController = (KeyPointController)FindObjectOfType<KeyPointController> ();
			if (_keyPointController == null) return;
		}

		if (_keyPointAnimation == null) {
			_keyPointAnimation = (KeyPointAnimation)FindObjectOfType<KeyPointAnimation> ();
			if (_keyPointAnimation == null) return;
		}

		if (playerController == null) {
			playerController = (PlayerController)FindObjectOfType<PlayerController> ();
			if (playerController == null) return;
		}

		_keyPointController.PlayerPosition = playerController.transform.position;
		_keyPointController.PlayerForwardAngle = playerController.forwardAngle;
	}
	
	/// <summary>
	/// KeyPointを外部から目標地点へ移動
	/// </summary>
	/// <param name="pos">目標地点</param>
	/// <param name="time">0で瞬間移動</param>
	public static void MoveToPosition (Vector3 pos, float time = float.MinValue) 
	{
		if (_keyPointController == null)
			return;
		 
		if (time == float.MinValue) {
			_keyPointController.SetTargetPosition (pos);	
		} else {
			if (time == 0) {
				_keyPointController.transform.position = pos;
				return;
			}
			_keyPointController.SetTargetPositionAnimation (pos, time);
		}
	}
}