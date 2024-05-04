using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KamioriInput;
using Rewired;
using R3;

/// <summary>
/// 基本的にrewiredInputMangerを利用、独自の入力があればこのクラスに追加。Rewiredに直接アクセスせず、このクラスを通す
/// このマネージャーを取得して入力を問い合わせる
/// マウスポインタ等、足りない部分はここで保持
/// </summary>
public class AliceInputManager : SingletonMonoBehaviour<AliceInputManager>
{
	public enum InputType
	{
		KeyMouse,
		Controller,
	}
	
	private static TouchInputManager touchInputManager;
	//private static KeyInputManager keyInputManager;

	public InputType CurrentInput = InputType.KeyMouse;

	/// <summary>
	/// プレイヤーの移動方向入力
	/// </summary>
	public ReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
	
	/// <summary>
    /// KeyPointの移動方向入力
    /// </summary>
    public ReadOnlyReactiveProperty<Vector3> KeyPointMoveDirection => _keyPointMoveDirection;

	/// <summary>
	/// ジャンプ入力
	/// </summary>
	public ReadOnlyReactiveProperty<bool> Jump => _jump;
	
	/// <summary>
	/// 右に回すボタン
	/// </summary>
	public ReadOnlyReactiveProperty<bool> RightRotate => _rightRotate;
	
	/// <summary>
	/// ジャンプ入力
	/// </summary>
	public ReadOnlyReactiveProperty<bool> LeftRotate => _leftRotate;	

	// 実装
	private readonly ReactiveProperty<bool> _rewind = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _jump = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _rightRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _leftRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
	private readonly ReactiveProperty<Vector3> _keyPointMoveDirection = new ReactiveProperty<Vector3>();
	
	private Player _player;
	private void Awake()
	{
		_rewind.AddTo(this);

		_jump.AddTo(this);
		_moveDirection.AddTo(this);
		_keyPointMoveDirection.AddTo(this);
		_rightRotate.AddTo(this);
		_leftRotate.AddTo(this);

		_player = ReInput.players.GetPlayer(0);
		
		touchInputManager = (new GameObject ("TouchInputManager")).AddComponent<TouchInputManager> ();
	}

	private void Update()
	{
		// 各種入力をReactivePropertyに反映
		_jump.Value = _player.GetButton(GameDefine.ACTION_JUMP);

		_rightRotate.Value = _player.GetButton(GameDefine.RIGHT_ROTATE);
		_leftRotate.Value = _player.GetButton(GameDefine.LEFT_ROTATE);

		Vector3 currentInputAxis = Vector3.zero;
		currentInputAxis.x = _player.GetAxis(GameDefine.ACTION_MOVE_HORIZONTAL);
		currentInputAxis.z = _player.GetAxis(GameDefine.ACTION_MOVE_VERTICAL);

		Vector3 currentInputKeyPointAxis = Vector3.zero;
		currentInputKeyPointAxis.x = _player.GetAxis(GameDefine.ACTION_MOVE_KEYPOINT_HORIZONTAL);
		currentInputKeyPointAxis.y = _player.GetAxis(GameDefine.ACTION_MOVE_KEYPOINT_VERTICAL);

		_moveDirection.Value = currentInputAxis;
		_keyPointMoveDirection.Value = currentInputKeyPointAxis;

		/*if (IsClick())
		{
			var tempPos = CameraManager.Instance.GetUiPos(Input.mousePosition);
			EffectManager.Instance.PlayEffect(EffectId.TouchHit, tempPos);      //エフェクト発行
		}*/
	}

	/// <summary>
	/// rewiredにはポインタ取得が無いため
	/// </summary>
	/// <returns></returns>
	public Vector3 GetMouseScreenPos()
	{
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameDefine.Z_POS_DANGION));
	}

	protected override void Init() {
		base.Init ();
	}

	public static void RegisterTouchEventHandler(ITouchEventHandler eventHandler)
	{
        if (touchInputManager != null) {
			touchInputManager.RegisterEventHandler (eventHandler);
			return;
		}
        Debug.LogWarning ("【KamioriInput】InputManager is null");
	}

	public static void UnregisterTouchEventHandler(ITouchEventHandler eventHandler)
	{
        if (touchInputManager != null) {
			touchInputManager.UnregisterEventHandler (eventHandler);
        }
	}

	public static void ClearInput() {
		
		if(touchInputManager == null) return;
		// preClear
		foreach(var info in touchInputManager.InfoManager.InputInfo) {
			info.deltaDistance = Vector3.zero;
			info.deltaTime = 0f;
		}

		// clear
        touchInputManager.ClearInput();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	protected override void Initialize()
	{
		//マルチタッチを許可
		Input.multiTouchEnabled = true;
	}

	public bool IsClick()
	{
		return Input.GetKeyDown(KeyCode.Mouse0);
	}
}