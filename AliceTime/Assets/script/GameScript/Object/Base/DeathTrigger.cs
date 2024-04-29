using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col){
		if(col.transform.gameObject.tag == GameDefine.PlayerTag)
		{
			float deathTime = col.GetComponent<AliceController>().GetPlayerDefaultParam.deathTriggerTime;
			StartCoroutine(Player_respawn(deathTime));
		}
	}

	public IEnumerator Player_respawn(float deathTime){
		yield return new WaitForSeconds (deathTime);
		GameSceneManager.Goto(GameDefine.GAME_DEATH);
	}
}