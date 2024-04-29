using UnityEngine;
using System.Collections;

public class EffectDeath : MonoBehaviour {
	private float efect_timer;
	// Use this for initialization
	void Start () {
		efect_timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		efect_timer += Time.deltaTime;
		if(efect_timer >= gameObject.GetComponent<ParticleSystem>().startLifetime 
		   && gameObject.GetComponent<ParticleSystem>().loop == false){
			Destroy(this.gameObject);

		}
	}
}
