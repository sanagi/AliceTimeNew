using UnityEngine;
using System.Collections;

public class AliceAnimation : PlayerAnimation {
	public PlayerController.STATE currentAnimState = PlayerController.STATE.WAIT;
	
	/// <summary>アニメ</summary>
	[SerializeField]
	private Animator _playerAnimator = null;
	
	#region implemented abstract members of PlayerAnimation
	public override PlayerController.STATE CurrentAnimationType ()
	{
		return currentAnimState;
	}

	public override void SetAnimation (PlayerController.STATE state, float moveSpeed)
	{
		switch (state) {
		case PlayerController.STATE.WAIT:
		case PlayerController.STATE.DEATH:
			PlayIdle ();
			break;
		case PlayerController.STATE.WALK:
			PlayWalk (moveSpeed);
			break;
		case PlayerController.STATE.JUMP:
			PlayJump ();
			break;
		}
		currentAnimState = state;
	}

	#endregion


	public enum AnimationType{
		IDLE = 0,
        TAMEJUMP = 1,
        JUMP = 2,
        LADDER = 3,
        WALK = 4
	}

	public void PlayIdle(){
		isStop = false;
		_playerAnimator.SetFloat("MoveSpeed", 0f);
	}

    public void PlayJump()
    {
		isStop = false;
    }

    public void PlayWalk(float moveSpeed){
		isStop = false;
		_playerAnimator.SetFloat("MoveSpeed", moveSpeed);
    }


	public override void StopAnim(){
		isStop = true;
	}
	public override void RestartAnim(){
		isStop = false;
	}

}
