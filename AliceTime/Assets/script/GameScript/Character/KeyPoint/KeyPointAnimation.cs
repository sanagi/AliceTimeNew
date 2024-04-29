using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Spineアニメーション再生
/// パーティクルの管理
/// </summary>
public class KeyPointAnimation : MonoBehaviour {
	
	[System.Serializable]
	public class ParticleData
	{
		public string key;
		public GameObject particle;
		
		public ParticleData(string key, GameObject particle){
			this.key = key;
			this.particle = particle;
		}
	}

	private GameObject currentParticle; // 子オブジェクトとしてエフェクトは持っている
	private GameObject CurrentParticle {
		set {
			currentParticle = value;
			currentParticle.transform.SetParent (transform);
			currentParticle.transform.localPosition = Vector3.zero;
		}
	}
	[SerializeField]
	private List<ParticleData> particles;

	public void Awake() 
	{
		particles = new List<ParticleData> ();

		//パーティクルのロード
		/*foreach (GameObject p in Resources.LoadAll("Particles/Aibou", typeof(GameObject))) {
			particles.Add (new ParticleData (p.name, p));
		}*/

    }
	
	public void SetAnimation(KeyPointManager.STATE state) {
		switch (state) {
		case KeyPointManager.STATE.WAIT:
			WaitAnimation ();
			break;
		case KeyPointManager.STATE.CONTROL:
			ControlAnimation ();
			break;
		case KeyPointManager.STATE.HOLD:
			HoldAnimation();
			break;
		case KeyPointManager.STATE.FIRE:
			FireAnimation ();
			break;
		case KeyPointManager.STATE.CANCEL:
			CancelAnimation ();
			break;
		}
	}

	private void WaitAnimation() 
	{
		if (currentParticle != null) {
			Destroy (currentParticle);
			currentParticle = null;
		}
	}
	
	private void ControlAnimation()
	{
		
	}

	private void HoldAnimation()
	{
		//Holdパーティクル読み込み
		//パーティクル放出
	}

	private void FireAnimation()
	{
		//過去に戻す魔法発動パーティクル読み込み
		/*ParticleData effectData = particles.Find (p => p.key == "Release");
		if (effectData == null) {
			Debug.LogWarning ("\"Particles/Line1\" is missing");
			return;
		}*/
		
        //Holdパーティクルを消す
        if(currentParticle != null)
        {
            Destroy(currentParticle.gameObject);
        }
        
        //再生が終われば自動で消す設定して再生
        //CurrentParticle = Instantiate (effectData.particle) as GameObject;
		//currentParticle.GetComponent<ParticleAutoDestroy>().EventAction += (() => KeyPointManager.SetState(KeyPointManager.STATE.WAIT));
	}

	private void CancelAnimation()
	{
		//チャージパーティクルを消す
        if (currentParticle != null)
        {
            Destroy(currentParticle.gameObject);
        }
		//残ってたら
		var power = gameObject.transform.Find("Power(Clone)");
		if(power != null){
			Destroy(power.gameObject);
		}
		//残ってたら
		var line = gameObject.transform.Find("Line_1(Clone)");
		if(line != null){
			Destroy(line.gameObject);
		}
        KeyPointManager.SetState(KeyPointManager.STATE.WAIT);
	}
}
