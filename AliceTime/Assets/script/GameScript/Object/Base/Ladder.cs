using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using R3;
using R3.Triggers;

public class Ladder : Gimick_Base
{
    //[SerializeField]
    //private bool isRegistedMoveDelegate = false;
    //[SerializeField]
    //private bool isRegistedAnimDelegate = false;

    [SerializeField]
    private float inputMargin = 0f;      //梯子を上り下りし始める判定のY軸のマージン
    [SerializeField]
    public float hitPlayerMargin = 0.35f;	//梯子を上り下りすることが可能になるX軸のマージン（0より大きい数字）

    [SerializeField] 
    private Collider2D _xyCollider;
    [SerializeField] 
    private Collider2D _yzCollider;
    
    protected Transform m_transform;

    private Vector3 _originPos;
    private Vector2 _boxScale;
    private Quaternion _rotation;
    private float _maxDistance;

    private bool CheckEnableLadderDistance(Vector3 playerPos)
    { 
        return Mathf.Abs(m_transform.position.x - playerPos.x) < hitPlayerMargin;
    }
    void Awake() {
        m_transform = this.transform;
        
        if (hitPlayerMargin <= 0) {
            hitPlayerMargin = 0.01f;
        }
        
        _originPos = m_transform.position + Vector3.down * (m_transform.localScale.y * 0.5f - 0.05f);
        _boxScale = Vector3.one * m_transform.localScale.x * hitPlayerMargin + Vector3.forward * 2f;
        _rotation = m_transform.rotation;
        _maxDistance = m_transform.localScale.y + 0.35f;
        
        var ladderAbilityXY = _xyCollider.OnTriggerStay2DAsObservable().Select(collision => collision.GetComponent<LadderClimbAbility>());
        ladderAbilityXY.Subscribe(_ =>
        {
            if (CheckEnableLadderDistance(_.transform.position))
            {
                _.SetTransform(transform);
                _.SetHijackMove();
            }
        });
        var ladderAbilityYZ = _yzCollider.OnTriggerStay2DAsObservable().Select(collision => collision.GetComponent<LadderClimbAbility>());
        ladderAbilityYZ.Subscribe(_ =>
        {
            if (CheckEnableLadderDistance(_.transform.position))
            {
                _.SetTransform(transform);
                _.SetHijackMove();
            }
        });        
        
        var ladderAbilityExitXY = _xyCollider.OnTriggerExit2DAsObservable().Select(collision => collision.GetComponent<LadderClimbAbility>());
        ladderAbilityExitXY.Subscribe(_ =>
        {
            _.RemoveHijackMove();
        });
        var ladderAbilityExitYZ = _yzCollider.OnTriggerExit2DAsObservable().Select(collision => collision.GetComponent<LadderClimbAbility>());
        ladderAbilityExitYZ.Subscribe(_ =>
        {
            _.RemoveHijackMove();
        });
    }
    
    void OnTriggerEnter2D(Collider2D col){
        if(col.transform.gameObject.tag == GameDefine.PlayerTag)
        {
            Debug.Log("Test");
        }
    }

    void Update() {
        // ハシゴを利用可能な範囲にプレイヤーがいるかどうかの判定→Ability側でプレイヤーをハックして動かす
        /*foreach (var hit in Physics2D.BoxCastAll(_originPos, _boxScale, 0f,transform.forward, _maxDistance)) { //TODO プレイヤーのみのレイヤーマスクにして負荷軽減
            var ladderAbility = hit.collider.GetComponent<LadderClimbAbility>();
            if (ladderAbility != null) {
                ladderAbility.SetTransform(transform);
                ladderAbility.SetHijackMove();

                /*if (isLadderNow)
                {
                    
                }
                else {
                    
                }
                
                return;
            }
        }
        
        PlayerManager.Instance.RemoveHijackMove(PlayerAbilityBase.AbilityType.Ladder);
        */
    }
}