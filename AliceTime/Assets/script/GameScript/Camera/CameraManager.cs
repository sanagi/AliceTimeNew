using UnityEngine;

using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class CameraManager : SingletonMonoBehaviour<CameraManager> {
	private Camera m_mainCamera;
	private Camera m_SubCamera;
	private Vector3 defaultPos;
    [SerializeField]
    private AspectOK aspectOK;
    
    [SerializeField]
    private CameraParam CameraParam;

    [SerializeField]
    private Fade _fade;

    public Transform VirtualCameraRootTransform;

    public GamePlay3dCamera GamePlay3DCamera;

    protected override void Init() {
		base.Init ();

		defaultPos = Camera.main.transform.position;
		DontDestroyOnLoad (gameObject);

		var subCamera = gameObject.transform.Find(GameDefine.SUB_CAMERA);
		m_SubCamera = subCamera.GetComponent<Camera>();
    }

	/// <summary>
	/// ゲーム用メインカメラ設定
	/// </summary>
	public void CrateMainGameGearCamera()
	{
		//メインカメラの設定
		/*var cameraManager = CameraManager.Instance;

		var cinemaBrain = cameraManager.MainCamera.GetComponent<CinemachineBrain>();
		if (cinemaBrain == null)
		{
			cinemaBrain = cameraManager.MainCamera.AddComponent<CinemachineBrain>();
			cinemaBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
			cinemaBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
			cinemaBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
		}

		//cameraManager.MainCamera.clearFlags = CameraClearFlags.SolidColor;
		//cameraManager.MainCamera.backgroundColor = Color.black;

		//VirtualCameraの親登録
		//var rigParent = new GameObject(GameDefine.CAMERA_RIG);
		//VirtualCameraRootTransform = rigParent.transform;
		
		//var virtualCamera = GameObject.Instantiate(CameraManager.Instance.CameraParam.VirtualCameraFollow);
		//virtualCamera.transform.parent = rigParent.transform;
		//GamePlayCamera = virtualCamera.GetComponent<GamePlayCamera>();
		
		//ゲームカメラ側で詳細設定
		//GamePlayCamera.SetUpGameCamera(MainCamera, CameraParam.FovSize, CameraParam.OlthoSize, CameraParam.FollowOffset);
		*/
	}
	
	/// <summary>
	/// 心臓部に入ったときのメインカメラ設定
	/// </summary>
	public void CrateAreaSelectCamera(Vector3 cameraPos)
	{
		//メインカメラの設定
		/*var cameraManager = CameraManager.Instance;
		var mainCam = cameraManager.MainCamera;
		
		//LookConstraintの設定
		var lookAt = cameraManager.MainCamera.AddComponent<LookAtConstraint>();
		var constraintSource = new ConstraintSource();
		constraintSource.sourceTransform = PlayerManager.Instance.CurrentPlayer.transform;
		lookAt.AddSource(constraintSource);
		lookAt.constraintActive = true;
		
		//LookConstraintの設定
		var positionConstraint = cameraManager.MainCamera.AddComponent<PositionConstraint>();
		var positionSource = new ConstraintSource();
		positionSource.sourceTransform = PlayerManager.Instance.CurrentPlayer.transform;
		positionConstraint.AddSource(positionSource);
		positionConstraint.constraintActive = true;

		mainCam.orthographic = false;
		mainCam.fieldOfView = CameraParam.FovSize;
		*/
		
		var cameraManager = CameraManager.Instance;

		var cinemaBrain = cameraManager.MainCamera.GetComponent<CinemachineBrain>();
		if (cinemaBrain == null)
		{
			cinemaBrain = cameraManager.MainCamera.AddComponent<CinemachineBrain>();
			cinemaBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
			cinemaBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
			cinemaBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
		}

		//cameraManager.MainCamera.clearFlags = CameraClearFlags.SolidColor;
		//cameraManager.MainCamera.backgroundColor = Color.black;

		//VirtualCameraの親登録
		var rigParent = new GameObject(GameDefine.CAMERA_RIG);
		VirtualCameraRootTransform = rigParent.transform;
		
		var virtualCamera = GameObject.Instantiate(CameraManager.Instance.CameraParam.VirtualCameraFollow);
		virtualCamera.transform.parent = rigParent.transform;
		GamePlay3DCamera = virtualCamera.GetComponent<GamePlay3dCamera>();
		virtualCamera.transform.localPosition = cameraPos;
		
		//ゲームカメラ側で詳細設定
		GamePlay3DCamera.SetUpGameCamera(MainCamera, CameraParam.FovSize, CameraParam.FollowOffset);
	}	

	protected override void Deinit() {
		base.Deinit ();
	}

	public void ResetPosition() {
		MainCamera.transform.position = defaultPos;
	}

	public Camera MainCamera {
		get {
			if (m_mainCamera == null) {
				m_mainCamera = Instance.GetComponent<Camera> ();
			}
			return m_mainCamera;
		}
	}

	public Camera SubCamera{
		get {
			if (m_mainCamera == null) {
				m_mainCamera = Instance.GetComponent<Camera> ();
			}
			return m_SubCamera;	
		}
	}

	// ぼかしを外すアニメーション
	public IEnumerator GaussOff(float time, Action complete = null) {
		yield return null;
	}

	/// <summary>
	/// Fade本体
	/// </summary>
	/// <param name="complete"></param>
	/// <param name="time"></param>
	/// <param name="endValue"></param>
	private void DoFade(Action complete, float time, float endValue)
	{
		LoadManager.Instance.HideLoadingMessage();
		var fadeDuration = time == 0f ? CameraParam.OutTime : time;
		_fade.FadeCanvasGroup.DOFade(endValue, fadeDuration).OnComplete(() =>
		{
			_fade.FadeDisable();
			complete();
		});
	}

	// フェードアウト
	public IEnumerator FadeOut(Action complete = null, float time = 0)
	{
		if (_fade != null)
		{
			_fade.FadeEnable(CameraParam.FadeColor);
			DoFade(complete, time, 1.0f);
		}
		else
		{
			complete();
		}
		yield return null;
	}

    // フェードイン
    public IEnumerator FadeIn(Action complete, float time = 0)
    {
	    if (_fade != null)
	    {
		    _fade.FadeEnable(CameraParam.FadeColor);
		    DoFade(complete, time, 0f);
	    }
	    else
	    {
		    complete();
	    }

	    yield return null;
	}
    
    // フェードアウト(色付き)
    public IEnumerator FadeOutColor(Color color, Action complete = null, float time = 0)
    {
	    if (_fade != null)
	    {
		    _fade.FadeEnable(color);
		    DoFade(complete, time, 1.0f);
	    }
	    else
	    {
		    complete();
	    }
	    yield return null;
    }

    // フェードイン(色付き)
    public IEnumerator FadeInColor(Color color, Action complete, float time = 0)
    {
	    if (_fade != null)
	    {
		    _fade.FadeEnable(color);
		    DoFade(complete, time, 0f);
	    }
	    else
	    {
		    complete();
	    }

	    yield return null;
    }    

	// Bloomによるフェードイン
	public IEnumerator BloomOn() {

		yield return null;
	}

	// Bloomによるフェードアウト
	public IEnumerator BloomOff() {

		yield return null;
	}

    public void AspectChange(bool _isOn)
    {
        if (!_isOn)
        {
            aspectOK.AspectChangeOff();
        }
        else
        {
            aspectOK.AspectChangeOn();
        }
    }
}