using UnityEngine;
using System.Collections;

public abstract class PlayerAnimation : MonoBehaviour {
	public bool isStop=false;
	public abstract PlayerController.STATE CurrentAnimationType ();
	public abstract void SetAnimation (PlayerController.STATE state, float moveSpeed);
    public abstract void StopAnim();
    public abstract void RestartAnim();
}
