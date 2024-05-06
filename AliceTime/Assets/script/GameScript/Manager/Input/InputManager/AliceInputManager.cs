using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	/// Pointerボタン入力
	/// </summary>
	public ReadOnlyReactiveProperty<bool> Point => _point;
	public Observable<bool> PointObservable => _point;
	
	/// <summary>
	/// Pointerボタン離した
	/// </summary>
	public ReadOnlyReactiveProperty<bool> PointRelease => _pointRelease;

	
	/// <summary>
	/// 右に回すボタン
	/// </summary>
	public ReadOnlyReactiveProperty<bool> RightRotate => _rightRotate;
	
	/// <summary>
	/// 左に回すボタン入力
	/// </summary>
	public ReadOnlyReactiveProperty<bool> LeftRotate => _leftRotate;	

	// 実装
	private readonly ReactiveProperty<bool> _rewind = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _jump = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _point = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _pointRelease = new ReactiveProperty<bool>();	
	private readonly ReactiveProperty<bool> _rightRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<bool> _leftRotate = new ReactiveProperty<bool>();
	private readonly ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
	private readonly ReactiveProperty<Vector3> _keyPointMoveDirection = new ReactiveProperty<Vector3>();
	
	private Player _player;
	private void Awake()
	{
		_rewind.AddTo(this);

		_jump.AddTo(this);
		_point.AddTo(this);
		_moveDirection.AddTo(this);
		_keyPointMoveDirection.AddTo(this);
		_rightRotate.AddTo(this);
		_leftRotate.AddTo(this);

		_player = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		// 各種入力をReactivePropertyに反映
		_jump.Value = _player.GetButton(GameDefine.ACTION_JUMP);
		
		_point.Value = _player.GetButton(GameDefine.ACTION_POINT) || _player.GetButton(GameDefine.ACTION_POINT_GEAR) || _player.GetButton(GameDefine.ACTION_POINT_EXPLORE);

		_pointRelease.Value = _player.GetButtonUp(GameDefine.ACTION_POINT) || _player.GetButtonUp(GameDefine.ACTION_POINT_GEAR) || _player.GetButtonUp(GameDefine.ACTION_POINT_EXPLORE);

		
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
	}

	/// <summary>
	/// rewiredにはポインタ取得が無いため
	/// </summary>
	/// <returns></returns>
	public Vector3 GetMouseScreenPos()
	{
		return CameraManager.Instance.GetUiCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameDefine.Z_POS_0));
	}

	public static void ClearInput() {
		
	}
}