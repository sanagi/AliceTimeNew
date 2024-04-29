using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// はしごを登る
/// </summary>
public class LadderClimbAbility : PlayerAbilityBase
{
    private bool isRegistedMoveDelegate = false;

    public enum Updown
    {
        NONE,
        UP,
        DOWN
    }

    //どの方向に上っているか
    private Updown upDown = Updown.NONE;
    //梯子を登り終えたか
    bool end = false;

    /// <summary>
    /// 触れてるはしごのtransform
    /// </summary>
    private Transform _ladderTransform;

    // レイヤーキャッシュ
    private int playerLayer;
    private int blockLayer;
    
    private void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        playerLayer = LayerMask.NameToLayer (GameDefine.PLAYER_LAYER);
        blockLayer = LayerMask.NameToLayer (GameDefine.DEFAULT_LAYER);
    }

    /// <summary>
    /// ギミック側で動かしたいメソッドを登録
    /// </summary>
    public override void SetHijackMove()
    {
        if (!isRegistedMoveDelegate)
        {
            isRegistedMoveDelegate = true;
            _playerController.hijackMove += PlayerMove;
        }
    }

    /// <summary>
    /// ギミック側で動かしていたメソッドを解除
    /// </summary>
    public override void RemoveHijackMove()
    {
        //梯子に触れていない時は常に普通に動けるように
        upDown = Updown.NONE;
        end = false;
        
        if (!isRegistedMoveDelegate)
        {
            return;
        }
        
        _playerController.hijackMove -= PlayerMove;
        _playerController.IsGimickLadderNow = false;
        isRegistedMoveDelegate = false;
    }
    
    public void SetTransform(Transform ladderTransform)
    {
        _ladderTransform = ladderTransform;
    }

    public Vector3 PlayerMove(PlayerController player, Vector3 input, Vector3 currentMove) {

        //既にその方向に登り終わっているのに同じ入力をしていないかチェック
        if(end && ((input.y  > 0 && upDown == Updown.UP) || (input.y < 0 && upDown == Updown.DOWN)))
        {
            RemoveHijackMove();
            return currentMove;
        }

        var underDiff = Mathf.Abs(_ladderTransform.localScale.y - player.transform.localScale.y) * 0.5f - 0.15f;
        var topDiff = (_ladderTransform.localScale.y + player.transform.localScale.y) * 0.5f - 0.35f;

        var diff = player.transform.position.y - _ladderTransform.position.y;
        var absDiff = Mathf.Abs(diff);
		if(GameSceneManager.CurrentPhaseState == GAMESCENE.PAUSE){
			return Vector3.zero;
		}
		if (input.y > 0.5) {
            // 上方向入力
            if (_playerController.IsGimickLadderNow) {
                upDown = Updown.UP;
				player.JumpEnd();
                if (absDiff > topDiff && diff > 0) {
                    Physics2D.IgnoreLayerCollision (playerLayer, blockLayer, false);
                    _playerController.IsGimickLadderNow = false;
                    end = true;
                    
                    //KamioriInputManager.ClearInput();
                } else {
                    currentMove.y = _playerController.GetPlayerParams.climpSpeed * input.y;
                    end = false;
                }
            } else {
                if (diff < 0) {
                    _playerController.IsGimickLadderNow = true;
					Physics2D.IgnoreLayerCollision (playerLayer, blockLayer);
                    currentMove.y = _playerController.GetPlayerParams.climpSpeed * input.y;
                    end = false;
                }
            }
        } else if (input.y < -0.5) {
            // 下方向入力
            if (_playerController.IsGimickLadderNow) {
                upDown = Updown.DOWN;
                player.JumpEnd();
                if (absDiff > underDiff && diff < 0) {
                    Physics2D.IgnoreLayerCollision (playerLayer, blockLayer, false);
                    _playerController.IsGimickLadderNow = false;

                    //おり終わった処理
                    end = true;
                    //KamioriInputManager.ClearInput();
                } else {
                    currentMove.y = _playerController.GetPlayerParams.climpSpeed * input.y;
                    end = false;
                }
            } else {
                if (absDiff > topDiff && diff > 0) {
                    _playerController.IsGimickLadderNow = true;
                    Physics2D.IgnoreLayerCollision (playerLayer, blockLayer);
                    currentMove.y = _playerController.GetPlayerParams.climpSpeed * input.y;
                    end = false;
                }
            }
        } else {
            // 上下入力なし
            if (_playerController.IsGimickLadderNow) {
                currentMove.y = 0;
            }
        }
        currentMove.x = _playerController.IsGimickLadderNow ? 0f : currentMove.x;
        return currentMove;
    }

    public void PlayerAnim(PlayerController player, Vector3 input, Vector3 currentMove) {
        /*
        if (isLadderNow) {
            if (spriteRoot.AnimationNo != 3) {
                spriteRoot.AnimationPlay(3, 0, 0, 3);
            } else {
                int currentFrame = spriteRoot.FrameCountProgress;
                float speed = (int)2f * input.y;
                if (speed < 0)
                    speed /= 2;
                spriteRoot.AnimationPlay(3, 0, currentFrame + (int)speed, speed);
            }

        } else {
            spriteRoot.AnimationPlay(0, 0, 0, 1);
        }
        */
    }
}
