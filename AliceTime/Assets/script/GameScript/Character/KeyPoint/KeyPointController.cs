using System;
using UnityEngine;
using System.Collections;
using Rewired;
using KamioriInput;
using System.Collections.Generic;
using InputSupport;
using R3;

public class KeyPointController : MonoBehaviour
{
    public enum FORWARD { RIGHT, LEFT };
    
    [SerializeField] private bool isManualMove; //特定の位置への演出中ならtrue
    [SerializeField] private bool isManualTurn; //特定の向きへの反転演出中ならtrue

    private float _moveForward; //X軸方向の移動方向キャッシュ

    // アニメーション用
    private Vector3 playerPositon;
    public Vector3 PlayerPosition
    {
        set
        {
            playerPositon = new Vector3(value.x, value.y, GameDefine.Z_POS_DANGION);
        }
        get
        {
            return playerPositon;
        }
    }
    private float playerForwardAngle;
    public float PlayerForwardAngle
    {
        set
        {
            if (PlayerManager.Instance.IsControllable())
            {
                playerForwardAngle = value;
            }
        }
    }
    private Vector3 startPosition;
    private Vector3 StartPosition
    {
        set
        {
            startPosition = new Vector3(value.x, value.y, GameDefine.Z_POS_DANGION);
        }
    }
    private Vector3 targetPosition;
    private Vector3 TargetPosition
    {
        set
        {
            targetPosition = new Vector3(value.x, value.y, GameDefine.Z_POS_DANGION);
        }
    }

    // 回転用
    private float startMoveAnimation;
    private float timerMoveAnimation;
    private float startTurnAnimation;
    private float timerTurnAnimation;
    private FORWARD currentForward;
    private FORWARD nextForward;

    // キャッシュ
    private Transform m_Transform;

    //オフセット
    public Vector3 nextOffset;

    //相棒タッチのオフセット
    public static readonly Vector2 Offsettouch = new Vector2(0.15f, 0.38f);

    public KeyPointParam KeyPointParameter;

    private AliceInputManager _aliceInputManager;

    private KeyPointMagicalCreator _pastMagicalCreator = new KeyPointMagicalCreator();
    [SerializeField]
    private GameObject PastMagicalCircle = null;

    [SerializeField]
    private GameObject PastMagicalCircleGhost = null;

    private bool isCreatedGhost = false;
    
    void Awake()
    {
        m_Transform = transform;
        
        isManualMove = false;
        isManualTurn = false;

        TargetPosition = m_Transform.position;
        PlayerForwardAngle = 0f;
        PlayerPosition = Vector3.zero;

        startMoveAnimation = 0f;
        timerMoveAnimation = 0f;
        startTurnAnimation = 0f;
        timerTurnAnimation = 0f;
        currentForward = FORWARD.RIGHT;

        _moveForward = 0f;

        _aliceInputManager = AliceInputManager.Instance;
        _pastMagicalCreator.CreatePastMagicalObj = PastMagicalCircle;
        _pastMagicalCreator.CreatePastMagicalObjGhost = PastMagicalCircleGhost;
    }

    void Update()
    {
        if (KeyPointManager.CurrentState == KeyPointManager.STATE.WAIT && !isManualMove && PlayerManager.Instance.IsControllable())
        {
            // プレイヤーの位置へ自動的に追尾
            nextOffset = (nextForward == FORWARD.RIGHT ? Vector3.left : Vector3.right) * KeyPointParameter.offsetForward;
            targetPosition = playerPositon + Vector3.up + nextOffset;
        }

        ControllerSimulatePointer();

        ContollerMove();
        ControllerTurn();

        AutoMove();
        AutoTurn();

        CheckPastMagiclCircle();
    }

    private void ControllerSimulatePointer()
    {
        if (!KeyPointManager.isControllable)
        {
            return;
        }

        Vector3 simulatePos = transform.position;
        simulatePos.x += Offsettouch.x;
        simulatePos.y += Offsettouch.y;

        List<TouchInfo> touches = new List<TouchInfo>();

        TouchInfo info = new TouchInfo();
        info.deltaTime = Time.deltaTime;
        info.Id = -1;

        TouchInputManager.Instance.HandlerManager.FireEvent(touches);
    }

    /// <summary>
    /// 画面外制限
    /// </summary>
    private void InScreen()
    {
        {
            var viewport = Camera.main.WorldToViewportPoint(targetPosition);
            if (viewport.x < 0f)
            {
                targetPosition.x = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
            }
            if (viewport.x > 1.0f)
            {
                targetPosition.x = Camera.main.ViewportToWorldPoint(Vector3.right).x;
            }
            if (viewport.y < 0f)
            {
                targetPosition.y = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
            }
            // なぜか余白が生まれてしまうので0.95fで対応
            if (viewport.y > 0.95f)
            {
                targetPosition.y = Camera.main.ViewportToWorldPoint(Vector3.up * 0.95f).y;
            }

            targetPosition.z = GameDefine.Z_POS_DANGION;
        }
    }
    
    private void ContollerMove()
    {/*
        if (!KeyPointManager.isControllable)
        {
            return;
        }

        var horizontal = _aliceInputManager.KeyPointMoveDirection.CurrentValue.x;
        var vertical = _aliceInputManager.KeyPointMoveDirection.CurrentValue.y;

        //入力があればtargetPos調整
        isManualMove = isManualMove | Mathf.Abs(horizontal) >= 0.001f || Mathf.Abs(vertical) >= 0.001f;
        _aliceInputManager.PastMagicCancel.Where(x => x).Where(_ => isManualMove).ThrottleFirst(TimeSpan.FromSeconds(GameDefine.STOP_TOP_HITS_SECONDS)).Subscribe(_ =>
        {
            isManualMove = false;
        });
        
        if (isManualMove)
        {
            if (_aliceInputManager.CurrentInput == AliceInputManager.InputType.KeyMouse)
            {
                targetPosition = _aliceInputManager.GetMouseScreenPos();
            }
            else
            {
                targetPosition = m_Transform.position + new Vector3(horizontal * KeyPointParameter.MinSpeed, vertical * KeyPointParameter.MinSpeed, 0);
            }
            InScreen(); //補正
        }
        */
    }

    private void ControllerTurn()
    {
        
    }

    private void AutoMove()
    {
        _moveForward = 0f;

        if (!KeyPointManager.isControllable)
        {   //移動が許可されていない場合
            SetTargetPositionAnimation(m_Transform.position, 0);
            return;
        }

        if (!isManualMove && GameSceneManager.CurrentPhaseState == GAMESCENE.MAIN)
        {   //通常アニメーション中かどうか
            var distance = Vector3.Distance(m_Transform.position, targetPosition);
            distance = distance > KeyPointParameter.MaxSpeed ? KeyPointParameter.MaxSpeed : distance < KeyPointParameter.MinSpeed ? KeyPointParameter.MinSpeed : distance;

            var time = (KeyPointParameter.accelSpeed / 100f) * (distance / KeyPointParameter.inertia);
            _moveForward = m_Transform.position.x;
            m_Transform.position = Vector3.Lerp(m_Transform.position, targetPosition, time);
            _moveForward = m_Transform.position.x - _moveForward;
        }
        else
        {   //イベントなどの特定のアニメーション中の場合
            var diff = Time.time - startMoveAnimation;
            if (diff < timerMoveAnimation)
            {
                _moveForward = m_Transform.position.x;
                m_Transform.position = Vector3.Lerp(startPosition, targetPosition, diff / KeyPointParameter.turnAnimationTime);
                _moveForward = m_Transform.position.x - _moveForward;
            }
            else
            {
                startMoveAnimation = 0;
                timerMoveAnimation = 0;
                m_Transform.position = targetPosition;
            }
        }
    }


    private void AutoTurn()
    {
        if (!KeyPointManager.isControllable)
        {
            return;
        }
        // 進行方向方向の計算
        if (!isManualTurn && GameSceneManager.CurrentPhaseState == GAMESCENE.MAIN)
        {

            if (Mathf.Abs(_moveForward) < 0.1f && KeyPointManager.CurrentState == KeyPointManager.STATE.WAIT)
            {
                nextForward = playerForwardAngle == 0f ? FORWARD.RIGHT : playerForwardAngle == 180f ? FORWARD.LEFT : nextForward;
                var nextOffset = (nextForward == FORWARD.RIGHT ? Vector3.left : Vector3.right) * KeyPointParameter.offsetForward;
                var targetPos = playerPositon + Vector3.up + nextOffset;
                if (Mathf.Abs(_moveForward) < 0.005f)
                {
                    currentForward = nextForward;
                }
            }
            else
            {
                nextForward = _moveForward > 0f ? FORWARD.RIGHT : FORWARD.LEFT;
                var nextOffset = (nextForward == FORWARD.RIGHT ? Vector3.left : Vector3.right) * KeyPointParameter.offsetForward;
                var targetPos = playerPositon + Vector3.up + nextOffset;
                currentForward = nextForward;
            }

        }
        else
        {
            if (Time.time - startTurnAnimation < timerTurnAnimation)
            {

            }
            else
            {
                startTurnAnimation = 0;
                timerTurnAnimation = 0;
                isManualTurn = false;
                m_Transform.eulerAngles = currentForward == FORWARD.RIGHT ? Vector3.zero : Vector3.up * 180f;
            }
        }

        // 回転アニメーション
        var targetAngle = currentForward == FORWARD.RIGHT ? 0f : 180f;
        if (Mathf.Abs(Mathf.DeltaAngle(m_Transform.localEulerAngles.y, targetAngle)) > 0.1f)
        {
            m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, m_Transform.eulerAngles.y + KeyPointParameter.deltaAngle, 0f));
        }
        else
        {
            m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, targetAngle, 0f));
        }
    }
    
    // 通常の移動用メソッド
    public void SetTargetPosition(Vector3 targetPos)
    {
        if (isManualMove || !KeyPointManager.isControllable) return;
        TargetPosition = targetPos;
    }

    // アニメーション用のメソッド
    public void SetTargetPositionAnimation(Vector3 targetPos, float time)
    {
        if (!KeyPointManager.isControllable) return;

        isManualMove = true;
        startMoveAnimation = Time.time;
        timerMoveAnimation = time / KeyPointParameter.turnMoveSpeed;
        StartPosition = m_Transform.position;
        TargetPosition = targetPos;
    }

    public void SetTurnAnimation(FORWARD forward, float time)
    {
        startTurnAnimation = Time.time;
        timerTurnAnimation = time;
        currentForward = forward;
        isManualTurn = true;
    }

    public void SetParent(Transform a_parent)
    {
        m_Transform.parent = a_parent;
    }

    /// <summary>
    /// 過去魔法陣作るかどうかのチェック
    /// </summary>
    private void CheckPastMagiclCircle()
    {
        /*
        //長押しでゴースト作る(既に作ってたらポインタに沿って移動)
        _aliceInputManager.PastMagic.Where(x=>x)
            .SelectMany(x => Observable.Interval(TimeSpan.FromSeconds(GameDefine.LONG_PRESS_SECONDS)))
            .TakeUntil(_aliceInputManager.PastMagicFire.Where(x=>x))
            .Subscribe(_ =>
            {
                //ゴーストまだ作って無ければ自身の位置にゴーストを作る
                if (!isCreatedGhost)
                {
                    _pastMagicalCreator.CreatePastMagicalCircleGhost(transform.position, transform);
                }
                isCreatedGhost = true;
            });
        
        //離したら魔法陣作る
        _aliceInputManager.PastMagicFire.
            //ボタンが押されて
            Where(x => x)
            //最後に離してからx秒以上たってれば
            .ThrottleFirst(TimeSpan.FromSeconds(GameDefine.STOP_TOP_HITS_SECONDS))
            .Subscribe(_ =>
            {
                //自身の位置に魔法陣を作る
                _pastMagicalCreator.CreatePastMagicalCircle(transform.position);
                isCreatedGhost = false;
            });
        
        //魔法陣を消す監視
        _aliceInputManager.PastMagicCancel.
            //ボタンが押されて
            Where(x => x)
            //最後に離してからx秒以上たってれば
            .ThrottleFirst(TimeSpan.FromSeconds(GameDefine.STOP_TOP_HITS_SECONDS))
            .Subscribe(_ =>
            {
                //魔法陣を消す
                _pastMagicalCreator.DestroyPastMagicalCircle();
                //ゴーストがあれば消す
                _pastMagicalCreator.DestroyPastMagicalCircleGhost();
                isCreatedGhost = false;
            });
            */
    }

    /// <summary>
    /// 外部から魔法陣を消したいとき
    /// </summary>
    public void ResetCircle()
    {
        //魔法陣を消す
        _pastMagicalCreator.DestroyPastMagicalCircle();
    }
}