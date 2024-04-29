using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

public class UIAnimationUtil : MonoBehaviour {

	public static void Wait(float waitTime, Action callback) {
		CameraManager.Instance.StartCoroutine(WaitRoutine(waitTime, callback));
	}

	public static void FadeIn(GameObject obj, float animationTime, Action callback)
	{
		CameraManager.Instance.StartCoroutine(FadeInAnimation(obj, callback, animationTime));
	}

	public static void FadeOut(GameObject obj, float animationTime, Action callback) {
		CameraManager.Instance.StartCoroutine(FadeOutAnimation(obj, callback, animationTime));
	}

	public static void Scale(GameObject obj, float from, float to, float animationTime, Action callback)
	{
		CameraManager.Instance.StartCoroutine(ScaleAnimation(obj, from, to, animationTime, callback));
	}

	public static void Move(GameObject obj, Vector3 from, Vector3 to, float animationTime, Action callback)
	{
		CameraManager.Instance.StartCoroutine(MoveAnimation(obj, from, to, animationTime, callback));
	}

	// アニメーション待機
	private static IEnumerator WaitRoutine(float waitTime, Action callback)
	{
		var startTime = Time.time;
		while (true)
		{
			var diffTime = Time.time - startTime;
			if(diffTime > waitTime)
			{
				break;
			}
			yield return null;
		}
		callback();
		yield break;
	}

	// フェードアウト
	public static IEnumerator FadeOutAnimation(GameObject targetObj, Action complete = null, float time = 0)
	{
		var images = targetObj.GetComponentsInChildren<Image>();
		ChangeImageAlpha(images, 1f);
		
		var startTime = Time.time;
		while (true) {
			var diff = Time.time - startTime;
		
			ChangeImageAlpha(images, (1f-diff/time));
		
			if (diff > time) {
				break;
			}

			yield return null;
		}

		ChangeImageAlpha(images, 0f);
		
		if (complete != null) {
			complete();
		}
		yield return null;
	}

	// フェードイン
	public static IEnumerator FadeInAnimation(GameObject targetObj, Action complete = null, float time = 0)
	{
		var images = targetObj.GetComponentsInChildren<Image>();
		ChangeImageAlpha(images, 0f);
		
		var startTime = Time.time;
		while (true) {
			var diff = Time.time - startTime;
			if (diff > time) {
				break;
			}

			ChangeImageAlpha(images, (diff / time));
			yield return null;
		}

		if (complete != null) {
			complete ();			
		}
		
		ChangeImageAlpha(images, 1f);
		yield return null;
	}

	private static void ChangeImageAlpha(Image[] images, float alpha)
	{
		var imagesLength = images.Length;
		for (var i = 0; i < imagesLength; i++)
		{
			var color = images[i].color;
			color.a = alpha;
			images[i].color = color;
		}
	}
	
	// 拡大・縮小アニメーション
	public static IEnumerator ScaleAnimation(GameObject targetObj, float from, float to, float animationTime, Action complete)
	{
		var diffScale = (to - from) * Vector3.one;
		var startTime = Time.time;

		var defaultScale = Vector3.one * from;
		targetObj.transform.localScale = Vector3.one * from;
		
		while (true)
		{
			var diffTime = Time.time - startTime;

			targetObj.transform.localScale = defaultScale + ((diffTime / animationTime) * diffScale);
			
			if (diffTime > animationTime)
			{
				break;
			}
			yield return null;
		}
		
		targetObj.transform.localScale = Vector3.one * to;

		if (complete != null)
		{
			complete();
		}

		yield return null;
	}	
	
	// 移動アニメーション
	public static IEnumerator MoveAnimation(GameObject targetObj, Vector3 from, Vector3 to, float animationTime,
		Action complete=null)
	{

		var diffDistance = to - from;
		var startTime = Time.time;

		targetObj.transform.localPosition = from;

		while (true)
		{
			var diffTime = Time.time - startTime;

			targetObj.transform.localPosition = from + (diffDistance * (diffTime / animationTime));

			if (diffTime > animationTime)
			{
				break;
			}
			yield return null;
		}

		targetObj.transform.localPosition = to;

		if (complete != null)
		{
			complete();
		}
		
		yield return null;
	}
}
