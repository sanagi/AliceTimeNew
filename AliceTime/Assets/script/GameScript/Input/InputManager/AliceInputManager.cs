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
	/// 巻き戻し銃ボタン
	/// </summary>
	public ReadOnlyReactiveProperty<bool> Rewind => _rewind;
	
	/// <summary>
	/// 過去魔法ボタン押す
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PastMagicHold => _pastMagicHold;
	
	/// <summary>
	/// 過去魔法ボタン離す
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PastMagic => _pastMagic;
	
	/// <summary>
	/// 過去魔法ボタン離す
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PastMagicFire => _pastMagicFire;
	
	/// <summary>
	/// 過去魔法ボタンキャンセル
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PastMagicCancel => _pastMagicCancel;

	/// <summary>
	/// 過去魔法ボタン切り替え
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PastMagicChange => _pastMagicChange;

	/// <summary>
	/// プレイヤーの移動方向入力
	/// </summary>
	public ReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
	
	/// <summary>
    /// KeyPointの移動方向入力
    /// </summary>
    public ReadOnlyReactiveProperty<Vector3> KeyPointMoveDirection => _keyPointMoveDirection;

	/// <summary>
	/// BackGroundPointの移動方向入力
	/// </summary>
	public ReadOnlyReactiveProperty<Vector3> BackGroundMoveDirection => _backGroundDirection;
	
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
	private readonly ReactiveProperty<bool> _pastMagicFire = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _pastMagic = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _pastMagicHold = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _pastMagicCancel = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _pastMagicChange = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _rewind = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _jump = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _rightRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _leftRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
	private readonly ReactiveProperty<Vector3> _keyPointMoveDirection = new ReactiveProperty<Vector3>();
	private readonly ReactiveProperty<Vector3> _backGroundDirection = new ReactiveProperty<Vector3>();

	private Player _player;
	private void Start()
	{
		// Destroy時にDispose()する
		_pastMagicFire.AddTo(this);
		_pastMagic.AddTo(this);
		_pastMagicHold.AddTo(this);
		_pastMagicCancel.AddTo(this);
		_pastMagicChange.AddTo(this);
		
		_rewind.AddTo(this);

		_jump.AddTo(this);
		_moveDirection.AddTo(this);
		_keyPointMoveDirection.AddTo(this);
		_backGroundDirection.AddTo(this);
		_rightRotate.AddTo(this);
		_leftRotate.AddTo(this);

		_player = ReInput.players.GetPlayer(0);
		
		touchInputManager = (new GameObject ("TouchInputManager")).AddComponent<TouchInputManager> ();
	}

	private void Update()
	{
		// 各種入力をReactivePropertyに反映
		_jump.Value = _player.GetButton(GameDefine.ACTION_JUMP);
		_pastMagicHold.Value = _player.GetButtonDown(GameDefine.ACTION_MAGIC);
		_pastMagic.Value = _player.GetButton(GameDefine.ACTION_MAGIC);
		_pastMagicFire.Value = _player.GetButtonUp(GameDefine.ACTION_MAGIC);
		_pastMagicCancel.Value = _player.GetButton(GameDefine.ACTION_MAGIC_CANCEL);
		_pastMagicChange.Value = _player.GetButton(GameDefine.ACTION_MAGIC_CHANGE);

		_rewind.Value = _player.GetButton(GameDefine.ACTION_REWIND);
		
		_rightRotate.Value = _player.GetButton(GameDefine.RIGHT_ROTATE);
		_leftRotate.Value = _player.GetButton(GameDefine.LEFT_ROTATE);

		Vector3 currentInputAxis = Vector3.zero;
		currentInputAxis.x = _player.GetAxis(GameDefine.ACTION_MOVE_HORIZONTAL);
		currentInputAxis.z = _player.GetAxis(GameDefine.ACTION_MOVE_VERTICAL);

		Vector3 currentInputKeyPointAxis = Vector3.zero;
		currentInputKeyPointAxis.x = _player.GetAxis(GameDefine.ACTION_MOVE_KEYPOINT_HORIZONTAL);
		currentInputKeyPointAxis.y = _player.GetAxis(GameDefine.ACTION_MOVE_KEYPOINT_VERTICAL);
		
		Vector3 currentInputBackGroundAxis = Vector3.zero;
		currentInputBackGroundAxis.x = _player.GetAxis(GameDefine.ACTION_BACKGROUND_HORIZONTAL);

		_moveDirection.Value = currentInputAxis;
		_keyPointMoveDirection.Value = currentInputKeyPointAxis;
		_backGroundDirection.Value = currentInputBackGroundAxis;
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
}