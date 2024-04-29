/*using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using R3;

public class StageRotateController : RotateController {

	//InputManager多用するのでキャッシュします
	private AliceInputManager _aliceInputManager;

	private GamePlayCamera _gamePlayCamera;

	public void Initialize()
	{
		_aliceInputManager = AliceInputManager.Instance;
		transform.position = rotateParam.position;
		_gamePlayCamera = CameraManager.Instance.GamePlayCamera;
		StageManager.Instance.SetStageRotateController(this);

		_currentPlaneType = PlaneType.XY; //StartはXYから
	}

	public void Update()
	{
		if (!PlayerManager.Instance.IsControllable()) return;

		void DoRotate(PlaneType nextPlaneType)
		{
			var rotateAngle = transform.rotation.eulerAngles;
			switch (nextPlaneType)
			{
				case PlaneType.XY:
					rotateAngle = GameDefine.XY_ROT;
					break;
				case PlaneType.YZ:
					rotateAngle = GameDefine.YZ_ROT;
					break;
			}
			Rotate(rotateAngle, ()=>
			{
				OnRotateStart(nextPlaneType);
			}, ()=>
			{
				OnRotateFinish(nextPlaneType);
			});
		}
		_aliceInputManager.RightRotate.
			//ボタンが押されて
			Where(x => x)
			//現在がXY平面で
			.Where(_ => _currentPlaneType == PlaneType.XY && PlayerManager.Instance.IsControllable())
			.Subscribe(_ =>
			{
				DoRotate(PlaneType.YZ);
			});
		
		_aliceInputManager.LeftRotate.
			//ボタンが押されて
			Where(x => x)
			//現在がYZ平面で
			.Where(_ => _currentPlaneType == PlaneType.YZ  && PlayerManager.Instance.IsControllable())
			.Subscribe(_ =>
			{
				DoRotate(PlaneType.XY);
			});
	}

	public void OnRotateStart(PlaneType nextPlaneType)
	{
		PlayerManager.Instance.DisablePhysics();
		PlayerManager.Instance.DisableControllable();
		
		PlayerManager.Instance.MoveCameraTarget(rotateParam.duration, rotateParam.easeType, nextPlaneType);
		if (_gamePlayCamera == null)
		{
			_gamePlayCamera = CameraManager.Instance.GamePlayCamera;
		}
		//_gamePlayCamera?.RotateCameraStart();
	}

	public void OnRotateFinish(PlaneType nextPlaneType)
	{
		switch (nextPlaneType)
		{
			case PlaneType.XY:
				transform.rotation = Quaternion.Euler(GameDefine.XY_ROT);
				break;
			case PlaneType.YZ:
				transform.rotation = Quaternion.Euler(GameDefine.YZ_ROT);
				break;
		}
		_currentPlaneType = nextPlaneType;
		PlayerManager.Instance.ChangePlayer(_currentPlaneType);
		PlayerManager.Instance.EnableControllable();
		if (_gamePlayCamera == null)
		{
			_gamePlayCamera = CameraManager.Instance.GamePlayCamera;
		}
		
		//_gamePlayCamera?.RotateCameraEnd();
	}
}
*/