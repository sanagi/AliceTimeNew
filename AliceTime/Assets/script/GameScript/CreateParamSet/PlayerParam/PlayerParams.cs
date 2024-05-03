using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerParams : ScriptableObject {
	public Vector3 characterScale;	//キャラクターのスケール
	//public float historyCorePastScale = 0.5f; //coreのpast判定用scale
	//public float historyCoreNowScale = 2.0f; //coreのscale

	//public float historyChangeJump = 0.1f; //見た目変わったときにジャンプする高さ

	public float moveSpeed;			//移動速度
	public float moveLimitInJump;	//ジャンプ中の移動制限
	public float minSlopeDegree;	// 坂だと認識する最小角度
	public float maxSlopeDegree;	// 坂だと認識する最大角度

	public float gravity;			//かかる重力
	public float maxFallSpeed;		//最大の落下速度

	public float jumpPower;			//ジャンプ力
	public float jumpLimitOnSlope;	//坂道でのジャンプに制限を加える
	public float delayJumpTime;		//次のジャンプまでの制限時間

	public float slipSpeed;			//ブロックを滑っているときのスピード

	public float areaMoveSpeed;		//エリア移動時の移動量

    public float deltaTurnAngle;	//反転時の回転速度

    public float deathTriggerTime; //死ぬときにかかる秒数

    public bool initialAwake;
    
    // プレイヤーのハシゴを登る速度
    public float climpSpeed = 0.035f;

    public float animSpeed = 2f;
}
