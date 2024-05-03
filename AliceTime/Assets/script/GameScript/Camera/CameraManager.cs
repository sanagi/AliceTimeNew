using UnityEngine;

using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class CameraManager : SingletonMonoBehaviour<CameraManager> {
	private Camera m_mainCamera;
	private Camera m_SubCamera;
	private Vector3 defaultPos;

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
		
	}
	
	/// <summary>
	/// 心臓部に入ったときのメインカメラ設定
	/// </summary>
	public void CrateAreaSelectCamera(Vector3 cameraPos)
	{
		var cameraManager = CameraManager.Instance;

		var cinemaBrain = cameraManager.MainCamera.gameObject.GetComponent<CinemachineBrain>();
		if (cinemaBrain == null)
		{
			cinemaBrain = cameraManager.MainCamera.gameObject.AddComponent<CinemachineBrain>();
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
}