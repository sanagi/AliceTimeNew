using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class RotateController : MonoBehaviour {
	[SerializeField]
	protected RotateParams rotateParam = null;

	/// <summary>
	/// 見えている平面
	/// </summary>
	public enum PlaneType
	{
		NONE,
		XY,
		YZ
	}

	protected PlaneType _currentPlaneType;
	public PlaneType CurrentPlaneType => _currentPlaneType;

	public void SetParent(Transform child)
	{
		child.SetParent(transform);
	}

	public void Rotate(Vector3 rotAngle, System.Action onStartRotate, System.Action onFinishRotate)
	{
		onStartRotate?.Invoke();
		transform.DORotate(rotAngle, rotateParam.duration, rotateParam.rotateMode).SetEase(rotateParam.easeType).OnComplete(() =>
		{
			onFinishRotate?.Invoke();
		});
	} 
}
