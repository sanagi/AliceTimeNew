using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KamioriInput;
using Rewired;
using DG.Tweening;
using R3;
using System;

public delegate Vector3 HijackMove(PlayerController player, Vector3 input, Vector3 currentMove);
public delegate void HijackAnimation(PlayerController player, Vector3 input, Vector3 currentMove);

[System.Serializable]
public abstract class PlayerController : MonoBehaviour, IKeyEventHandler
{
    // ギミックなどで動きを掌握するための
    public HijackMove hijackMove;
    public HijackAnimation hijackAnim;
    public bool IsHijack()
    {
        return (hijackMove != null) || (hijackAnim != null);
    }

    public Dictionary<PlayerAbilityBase.AbilityType, PlayerAbilityBase> AbilityDictionary = new Dictionary<PlayerAbilityBase.AbilityType, PlayerAbilityBase>();
    public void AddAbility(PlayerAbilityBase.AbilityType abilityType, PlayerAbilityBase ability)
    {
        if (!AbilityDictionary.ContainsKey(abilityType))
        {
            AbilityDictionary.Add(abilityType, ability);
        }
    }

    public PlayerAbilityBase GetAbilityType(PlayerAbilityBase.AbilityType abilityType)
    {
        if (AbilityDictionary.ContainsKey(abilityType))
        {
            return AbilityDictionary[abilityType];
        }

        return null;
    }

    // 継承先で定義するメソッド
    protected abstract void SetupDefaultPlayerParams();
    protected abstract void SetupAnimation();

    // プレイヤーの動作パラメータを継承先から設定
    public void SetPlayerParams(PlayerParams param) { playerParams = param; }
    private PlayerParams playerParams;
    public PlayerParams GetPlayerParams => playerParams;

    // プレイヤーのアニメーション情報を継承先から設定
    public void SetPlayerAnimation(PlayerAnimation anim) { playerAnim = anim; }
    private PlayerAnimation playerAnim;

    // プレイヤーの状態管理
    public enum STATE
    {
        WAIT, WALK, JUMP, DEATH, HIJACK
    }
    private STATE currentState;
    public STATE CurrentState { get { return currentState; } }
    public void SetState(STATE state)
    {
        currentState = state;
        playerAnim.SetAnimation(currentState, currentMove.magnitude);
    }

    // プレイヤーのコントロールフラグ
    private bool isControllable;
    private bool isPhysics;

    public bool IsGimickLadderNow = false;

    // キャッシュ
    private Transform m_Transform;
    public Transform PlayerTransform
    {
        get
        {
            return m_Transform;
        }
    }

    private CharacterController m_Controller;

    //private OptionManager.ControllerType controllerType;

    public void RigidbodyWakeUp()
    {
        
    }

    // 移動用
    private Vector3 currentInput;
    private Vector3 currentMove;

    // 着地判定用
    [SerializeField]
    private bool isGrounded;                                // 接地しているかどうかのフラグ
    public bool IsGrounded { get { return isGrounded; } }

    // 坂道用
    [System.Serializable]
    private class LANDED
    {
        public Vector3 position;
        public bool isCollision;
        public float degree;
        public float hitDegree;
        public float MostLeftXPos;
        public float MostRightXPos;

        public LANDED()
        {
            position = Vector3.zero;
            isCollision = false;
            degree = 0f;
            hitDegree = 0f;
            MostLeftXPos = 0f;
            MostRightXPos = 0f;
        }
    }
    [SerializeField]
    private LANDED currentLanded;   // 直前に接触していた床オブジェクト
    private bool isHitRightCapsel;  //カプセルの下側に何か当たったか

    [SerializeField]
    private bool isSlope;

    //InputManager多用するのでキャッシュします
    private AliceInputManager _aliceInputManager;

    //protected Action OnRespawn;

    // ジャンプ用
    [SerializeField]
    public bool isJump;
    private bool isHead;
    private float timer;
    private float gravity;
    public void SetDefaultGravity()
    {
        SetGravity(playerParams.gravity);
    }
    public void SetGravity(float g)
    {
        gravity = g;
    }
    public void ResetGravity()
    {
        if (gravity > -playerParams.jumpPower)
        {
            gravity = -playerParams.jumpPower;
        }
    }

    // アニメーション用
    public float forwardAngle;
    //GameObject kureha;

    // エリア移動用
    private bool isAreaMove;
    private Vector3 movingDistanceByAreaMove;

    // レイヤー
    private int layer_Lift;
    private int layer_Block;
    private int layer_Player;

    //回転判定用
    private bool rotate;

    private void SetupFlag()
    {
        isControllable = false;
        isPhysics = false;

        isSlope = false;
        isJump = false;
        isGrounded = false;
        isAreaMove = false;
    }

    public void Display()
    {

        m_Transform.gameObject.SetActive(true);
    }

    public void Hide()
    {

        m_Transform.gameObject.SetActive(false);
    }

    private void SetupParam()
    {
        // ギミック用デリゲートの初期化
        hijackMove = null;
        hijackAnim = null;

        // 必須の項目を確認
        if (playerParams == null || playerAnim == null)
        {
            Debug.LogError("CharacterParam or CharacterAnimation ScriptableObject is null!!");
            return;
        }

        // キャッシュ関係の初期化
        m_Transform = gameObject.transform;

        m_Controller = GetComponent<CharacterController>();
        m_Controller.slopeLimit = playerParams.maxSlopeDegree;

        // 移動関係の初期化
        currentMove = Vector3.zero;
        currentInput = Vector3.zero;

        // 坂道関係用の初期化
        currentLanded = new LANDED();

        // ジャンプ関係の初期化
        timer = 0f;
        SetGravity(playerParams.gravity);

        // アニメーション関係の初期化
        forwardAngle = 0f;
        playerAnim.isStop = false;

        // エリア移動用
        movingDistanceByAreaMove = Vector3.zero;
        
        //controllerType = OptionManager.Instance.GetControllerType();
    }

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _aliceInputManager = AliceInputManager.Instance;
        
        // 継承先の初期化メソッドを呼ぶ
        SetupDefaultPlayerParams();
        SetupAnimation();

        // 基本ステータスの初期化
        SetupFlag();
        SetupParam();

        SetState(STATE.WAIT);
        playerAnim.SetAnimation(STATE.WAIT, 0.0f);

        IsGimickLadderNow = false;

        AliceInputManager.ClearInput();

        var virtualCameraObj = CameraManager.Instance.gameObject.transform.Find(GameDefine.VIRTUAL_CAMERA_FOLLOW);
        if (virtualCameraObj != null)
        {
            var virtualCamera = virtualCameraObj.GetComponent<CinemachineVirtualCameraBase>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = m_Controller.transform;
            }
        }    
        
        //親の登録
        //var stageRotateController = (StageRotateController)FindObjectOfType<StageRotateController>();
        //stageRotateController.SetParent(transform);
        
        //初期のplayerをOn
        if (playerParams.initialAwake)
        {
            PlayerManager.Instance.SetCurrentPlayer(this);
        }
    }

    void Update()
    {
        if (!isPhysics)
        {
            return;
        }

        isGrounded = CheckGrounded();

        bool groundSECheck = false;
        if (currentState == STATE.JUMP)
        {
            groundSECheck = true;
        }
        
        UpdateState(currentInput, currentMove);

        if (timer < playerParams.delayJumpTime)
        {
            timer += Time.deltaTime; //タイマーのカウントアップ
        }

        if (isGrounded && groundSECheck && !IsGimickLadderNow && currentState != STATE.JUMP)
        {
            //着地音
            //SoundManager.Instance.PlaySound(SoundEnum.SE_JumpDown);
        }

        if (hijackAnim != null && hijackMove != null)
        {
            IsGimickLadderNow = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.transform.tag == "gimick" || hit.transform.tag == "Ghost") || hit.transform.name.Contains("Gimick"))
        {
            return;
        }
        else
        {//カプセルの側で当たった

            if (hit.point.x < gameObject.transform.position.x)
            {
                if ((hit.transform.tag == "gimick" || hit.transform.tag == "Ghost") || hit.transform.name.Contains("Gimick"))
                {
                    return;
                }
                else
                {
                    isHitRightCapsel = true;
                }
            }
            else
            {
                isHitRightCapsel = false;
            }
        }
    }

    /// <summary>
    /// キャラクターが接地しているかどうか
    /// CharacterControllerのisGroundedではガバガバなので補完
    /// TODO:坂の処理を記載しておく
    /// </summary>
    private bool CheckGrounded()
    {
        //return m_Controller.isGrounded;

        isSlope = false;
        currentLanded.isCollision = false;

        var origin = m_Transform.position;
        Vector2 origin2D = Vector2.zero;
        origin2D.x = origin.x;
        origin2D.y = origin.y;
        var radius = m_Transform.localScale.x * 0.5f;
        var maxDistace = m_Transform.localScale.y * 0.5f;// - radius + 0.1f;
        int maskLayer = 1 << LayerMask.NameToLayer(GameDefine.DEFAULT_LAYER);
        
        RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.down, maxDistace, maskLayer);

        var mostMinDeg = 90f;
        foreach (var hit in hits)
        {
            /*if ((hit.transform.tag == "gimick" || hit.transform.tag == "Ghost") || hit.transform.name.Contains("Gimick"))
            {
                continue;
            }*/
            currentLanded.isCollision = true;

            var hitTrans = hit.transform;

            var hitDeg = Vector3.Angle(Vector3.right, hit.normal);
            var absDeg = hitDeg >= 90 ? hitDeg - 90f : 90f - hitDeg;

            if (mostMinDeg < absDeg)
            {
                continue;
            }

            mostMinDeg = absDeg;
            currentLanded.position = hit.transform.position;
            currentLanded.hitDegree = hitDeg;

            // 床オブジェクトの角度を取得
            var degree = 180f <= hitTrans.eulerAngles.z ? hitTrans.eulerAngles.z - 180f : hitTrans.eulerAngles.z;
            if (hitTrans.eulerAngles.y != 0)
            {
                degree = 180f - degree;
            }

            // ブロックのX軸における端の座標を計算
            var widthBase = (hitTrans.localScale.x * 0.5f) * (degree > 90f ? Mathf.Cos((180f - degree) * Mathf.Deg2Rad) : Mathf.Cos(degree * Mathf.Deg2Rad));
            var widthSub = hitTrans.localScale.y * Mathf.Sin(degree * Mathf.Deg2Rad);
            if (degree > 90f)
            {
                currentLanded.MostLeftXPos = hitTrans.position.x - widthBase - widthSub;
                currentLanded.MostRightXPos = hitTrans.position.x + widthBase;
            }
            else
            {
                currentLanded.MostLeftXPos = hitTrans.position.x - widthBase;
                currentLanded.MostRightXPos = hitTrans.position.x + widthBase + widthSub;
            }

            // 坂と認識する角度の床かどうかを計算
            if (playerParams.minSlopeDegree < degree && degree < 180f - playerParams.minSlopeDegree)
            {
                isSlope = true;
            }

            // プレイヤーの位置関係から現在の床の角度を計算
            var diffX = m_Transform.position.x - hitTrans.position.x;
            if (0 < diffX)
            {
                if (0f < degree && degree < 90f && widthBase < diffX)
                {
                    degree = 180f - degree;
                }
            }
            else
            {
                if (90f < degree && degree < 180f && diffX < -widthBase)
                {
                    degree = degree - 90f;
                }
            }
            currentLanded.degree = degree;
        }

        if (hits.Length > 1)
        {
            //ちゃんとした着地判定に通ったらカプセルの下側で当たったフラグをオフ
            return true;
        }

        if (mostMinDeg < 30f)
        {
            //ちゃんとした着地判定に通ったらカプセルの下側で当たったフラグをオ
            return true;
        }

        return false;
    }

    private bool CheckPause()
    {
        if (MainSceneManager.CurrentPhase == null)
        {
            return false;
        }
        switch (MainSceneManager.CurrentPhase.PhaseName)
        {
            case GameDefine.AreaSelect:
                if (AreaSelectSceneManager.CurrentPhaseState == AREASELECT.PAUSE)
                {
                    return true;
                }
                return false;
            case GameDefine.Explore:
                if (ExploreSceneManager.CurrentPhaseState == EXPLORESCENE.PAUSE)
                {
                    return true;
                }
                return false;
            case GameDefine.GAME:
                if (GameSceneManager.CurrentPhaseState == GAMESCENE.PAUSE)
                {
                    return true;
                }
                return false;
        }

        return false;
    }
    
    /// <summary>
    /// 移動系処理
    /// </summary>
    void FixedUpdate()
    {
        UpdateCurrentInput();
        UpdateJump();
        
        if (!isPhysics || CheckPause())
        {
            if (currentState == STATE.WALK)
            {
                currentState = STATE.WAIT;
                playerAnim.SetAnimation(STATE.WAIT, 0.0f);
            }
            return;
        }

        if (!isControllable)
        {  //入力キャンセル中
            if (isAreaMove)
            { //エリア移動中
                currentMove = movingDistanceByAreaMove * playerParams.areaMoveSpeed;
                if (currentMove.x != 0)
                {
                    SetGravity(gravity + playerParams.gravity * Time.fixedDeltaTime);
                    currentMove.y = gravity;
                }
                m_Controller.Move(currentMove);
                return;
            }

            if ((currentState != STATE.WAIT || playerAnim.CurrentAnimationType() != STATE.WAIT))
            {
                SetState(STATE.WAIT);
                playerAnim.SetAnimation(STATE.WAIT, 0.0f);
            }
            currentMove.x = 0;
            currentMove.z = 0;

            if (isJump)
            {
                //上にぶつかったら
                CheckHead();
            }

            //落下速度の調整
            if (currentState == STATE.JUMP && isHead)
            {
                SetGravity(gravity + playerParams.gravity * Time.fixedDeltaTime);
                currentMove.y = gravity;
            }
            else
            {
                SetGravity(gravity + playerParams.gravity * Time.fixedDeltaTime);
                currentMove.y = playerParams.jumpPower + gravity;
            }

            if (currentMove.y < playerParams.maxFallSpeed)
                currentMove.y = playerParams.maxFallSpeed;
            if (isJump == true && currentMove.y < 0 && currentLanded.isCollision)
            {
                JumpEnd();
            } 
            m_Controller.Move(currentMove);
            
            return;
        }

        //入力から移動値計算
        Move(currentInput, ref currentMove);

        m_Controller.Move(currentMove);
    }

    public static Vector3 SphereOrCapsuleCastCenterOnCollision(Vector3 origin, Vector3 directionCast, float hitInfoDistance)
    {
        return origin + (directionCast.normalized * hitInfoDistance);
    }

    /// <summary>
    /// 上にぶつったかチェック
    /// </summary>
    private void CheckHead()
    {
        var origin = m_Transform.position;
        var maxDistace = (m_Transform.localScale.y / 2) - 0.05f;
        int maskLayer = 1 << layer_Lift | 1 << layer_Block;
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.up, maxDistace, maskLayer);
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer(GameDefine.TRIGGER_LAYER))
            {
                isHead = true;
                return;
            }
        }
    }

    private void Move(Vector3 currentInput, ref Vector3 currentMove)
    {
        //上にぶつかったら
        if (isJump)
        {
            CheckHead();
        }

        //落下速度の調整
        if (currentState == STATE.JUMP && isHead)
        {
            SetGravity(gravity + playerParams.gravity * Time.fixedDeltaTime);
            currentMove.y = gravity;
        }
        else
        {
            SetGravity(gravity + playerParams.gravity * Time.fixedDeltaTime);
            currentMove.y = playerParams.jumpPower + gravity;
        }

        if (currentMove.y < playerParams.maxFallSpeed)
            currentMove.y = playerParams.maxFallSpeed;

        if (isSlope)
            gravity += playerParams.jumpLimitOnSlope * Time.fixedDeltaTime;

        if (isJump == true && currentMove.y < 0 && currentLanded.isCollision)
        {
            JumpEnd();
        }
        switch (currentState)
        {
            case STATE.WAIT:
            case STATE.DEATH:
                currentMove.x = 0;
                currentMove.z = 0;
                break;
            case STATE.WALK:
                var normalMoveX = currentInput.x * playerParams.moveSpeed;
                var normalMoveZ = currentInput.z * playerParams.moveSpeed;
                currentMove.x = normalMoveX;
                currentMove.z = normalMoveZ;
                
                break;
            case STATE.JUMP:

                // X軸の移動
                currentMove.x = currentInput.x * playerParams.moveSpeed * playerParams.moveLimitInJump;
                currentMove.z = currentInput.z * playerParams.moveSpeed * playerParams.moveLimitInJump;

                // 急な角度の坂に接触している時の処理
                if (!isJump && isSlope && currentLanded.isCollision)
                {
                    var slipMove = currentMove.y / Mathf.Tan(currentLanded.degree * Mathf.Deg2Rad);
                    if (playerParams.maxSlopeDegree < currentLanded.degree && currentLanded.degree < 90f)
                    {
                        if (currentInput.x < 0)
                        {
                            currentMove.x += slipMove;
                        }
                        else
                        {
                            currentMove.x = slipMove;
                        }

                        //坂がどっちに傾いているか
                        if (isHitRightCapsel)
                        {
                            currentMove.x *= -1.0f;
                        }

                    }
                    else if (90f < currentLanded.degree && currentLanded.degree < 180f - playerParams.maxSlopeDegree)
                    {
                        if (currentInput.x > 0)
                        {
                            currentMove.x += slipMove;
                        }
                        else
                        {
                            currentMove.x = slipMove;
                        }
                    }
                }

                var margin = m_Transform.localScale.x * 0.51f;
                var pos = m_Transform.position.x;
                var left = currentLanded.MostLeftXPos;
                var right = currentLanded.MostRightXPos;
                if (!isJump && ((left - margin <= pos && pos <= left) || (right <= pos && pos <= right + margin)))
                {
                    // ブロックの端で滑る挙動
                    if (m_Transform.position.x - currentLanded.position.x > 0)
                    {
                        currentMove.x += playerParams.slipSpeed;
                        if (currentLanded.isCollision && currentMove.x < playerParams.slipSpeed)
                        {
                            currentMove.x = playerParams.slipSpeed;
                        }
                    }
                    else if (m_Transform.position.x - currentLanded.position.x < 0)
                    {
                        currentMove.x += -1 * playerParams.slipSpeed;
                        if (currentLanded.isCollision && currentMove.x > -playerParams.slipSpeed)
                        {
                            currentMove.x = -1 * playerParams.slipSpeed;
                        }
                    }
                }
                break;
        }

        currentMove = hijackMove == null ? currentMove : hijackMove(this, currentInput, currentMove);

        if (currentMove.x != 0 || currentMove.z != 0)
        {
            RotateDir();
        }
    }

    private void RotateDir()
    {
        var rotateDir = currentMove;
        rotateDir.y = 0;
        //進行方向を向く
        // 進行方向（移動量ベクトル）に向くようなクォータニオンを取得
        var rotation = Quaternion.LookRotation(rotateDir, Vector3.up);
        // オブジェクトの回転
        transform.rotation = rotation;
    }

    private void Jump()
    {
        isJump = true;
        SetGravity(0f);
        SetState(STATE.JUMP);
        //Audio_Manage.Play(SoundEnum.SE_JumpUp);
    }

    public void JumpEnd()
    {
        if (isJump)
        {
            SetGravity(gravity + playerParams.gravity);
        }
        isJump = false;
        isHead = false;
    }

    void UpdateState(Vector3 currentInput, Vector3 currentMove)
    {
        // アニメーションが上書きされるべき状態かをチェックしてState切り替え
        if (hijackAnim == null)
        {
            ChangeState();
        }
        else
        {
            hijackAnim(this, currentInput, currentMove);
        }

        var moveXZVector = currentMove;
        moveXZVector.y = 0f;
        UpdateAnimation(moveXZVector.magnitude * playerParams.animSpeed);
    }

    /// <summary>
    /// 状態によってStateを切り替える
    /// </summary>
    private void ChangeState()
    {
        switch (currentState)
        {
            case STATE.WAIT:
                if (isGrounded)
                {
                    if (currentInput.x != 0 || currentInput.z != 0)
                    {
                        SetState(STATE.WALK);
                    }
                }
                else if (!isGrounded)
                { 
                    SetState(STATE.JUMP);
                }
                break;
            case STATE.WALK:
                if (isGrounded)
                {
                    if (currentInput.x == 0 && currentInput.z == 0)
                    {
                        SetState(STATE.WAIT);
                    }
                }
                else if (!isGrounded)
                {
                    SetState(STATE.JUMP);
                }
                break;
            case STATE.JUMP:
                if (isGrounded && !isJump)
                {
                    if (gravity < 0)
                    {
                        if (currentLanded.degree < playerParams.maxSlopeDegree)
                        {
                            JumpEnd();
                        }
                        timer = 0;
                        SetState(currentMove == Vector3.zero ? STATE.WAIT : STATE.WALK);
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// アニメーション更新
    /// </summary>
    private void UpdateAnimation(float moveSpeed)
    {
                //Animationオブジェクトの位置補正が必要なら行っておく
        /*if (GameSceneManager.CurrentPhaseState == GAMESCENE.MAIN)
        {
            if (currentInput.x == currentMove.x || (currentInput.x < 0 && currentMove.x < 0) || (currentInput.x > 0 && currentMove.x > 0))
            {
                forwardAngle = currentMove.x < 0 ? 180f : currentMove.x > 0 ? 0 : forwardAngle;
                if (Mathf.Abs(Mathf.DeltaAngle(m_Transform.localEulerAngles.y, forwardAngle)) > 0.1f)
                {
                    rotate = true;
                    m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, m_Transform.eulerAngles.y + playerParams.deltaTurnAngle, 0f));
                }
                else
                {
                    rotate = false;
                    m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, forwardAngle, 0f));
                    Vector3 euler = gameObject.transform.localRotation.eulerAngles;
                    if (euler.y == 0)
                    {
                        kureha.transform.localPosition = new Vector3(kureha.transform.localPosition.x, kureha.transform.localPosition.y, -1.5f);
                    }
                    else if (175.0f < euler.y && euler.y < 181.0f)
                    {
                        kureha.transform.localPosition = new Vector3(kureha.transform.localPosition.x, kureha.transform.localPosition.y, 1.5f);
                    }
                }
            }
            else
            {

                if (m_Transform.eulerAngles.y != 0f || m_Transform.eulerAngles.y != 180f)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(m_Transform.localEulerAngles.y, forwardAngle)) > 0.1f)
                    {
                        rotate = true;
                        m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, m_Transform.eulerAngles.y + playerParams.deltaTurnAngle, 0f));
                    }
                    else
                    {
                        rotate = false;
                        m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, forwardAngle, 0f));
                    }
                }
            }
        }
        */
        
        // アニメーションの更新
        if (playerAnim != null && playerAnim.CurrentAnimationType() == currentState)
        {
            playerAnim.SetAnimation(currentState, moveSpeed);
        }
    }

    public bool IsControllable()
    {
        return isControllable;
    }

    /// <summary>
    ///  キャラクターをコントロール可能な状態に
    /// </summary>
    public void EnableControllable()
    {
        isControllable = true;
    }

    /// <summary>
    /// キャラクターをコントロール不可能な状態に
    /// </summary>
    public void DisableControllable()
    {
        isControllable = false;
    }

    public void EnablePhysics()
    {
        isPhysics = true;
    }

    public void DisablePhysics()
    {
        isPhysics = false;
    }

    public void StopAnimation()
    {
        playerAnim.StopAnim();
    }
    public void ReStartAnimation()
    {
        playerAnim.RestartAnim();
    }

    /// <summary>
    /// キャラクターをリスポーン
    /// </summary>
    /*public void Respawn()
    {
        m_Transform.position = MainGameManager.RespawnPosition;
        SetState(STATE.WAIT);
        OnRespawn();
    }*/

    public void StartInertial(Vector3 distance)
    {
        movingDistanceByAreaMove = distance;
        isAreaMove = true;
    }
    public void StopInertial()
    {
        isAreaMove = false;
    }

    public void Turn()
    {
        transform.DORotate(new Vector3(0,0,180), 1, RotateMode.LocalAxisAdd);  //ローカル軸に対して
    }

    private void UpdateCurrentInput()
    {
        if (!isControllable)
        {
            currentInput = Vector3.zero;
            return;
        }
        currentInput.x = _aliceInputManager.MoveDirection.CurrentValue.x;
        currentInput.z = _aliceInputManager.MoveDirection.CurrentValue.z;
    }

    private void UpdateJump()
    {
        if (!isControllable) return;

        _aliceInputManager.Jump.
            //ボタンが押されて
            Where(x => x)
            //既にジャンプしてなくて
            .Where(_ => !isJump)
            //接地中で
            .Where(_ => isGrounded)
            //はしご上ってなくて
            .Where(_ => !IsGimickLadderNow)
            //ジャンプしてからx秒以上たってれば
            .ThrottleFirst(TimeSpan.FromSeconds(playerParams.delayJumpTime))
            .Subscribe(_ =>
            {
                Jump();
            });
    }

    #region IKeyEventHandler implementation
    public void OnCrossKeyEvent(KeyInfo info)
    {
#if UNITY_SWITCH
        return;
#else
        if (!isControllable) return;
        var preInput = currentInput;

        currentInput.x = 0f;
        currentInput.x += info.Right;
        currentInput.x -= info.Left;

        currentInput.y = 0f;
        currentInput.y += info.Up;
        currentInput.y -= info.Down;

        if (info.Jump == 1) {
            Jump();
        }
#endif
    }
    #endregion

    #region IInputEventHandler implementation
    public int Order
    {
        get
        {
            return 0;
        }
    }
    public bool Process
    {
        get
        {
            return isControllable;
        }
    }
    #endregion
}