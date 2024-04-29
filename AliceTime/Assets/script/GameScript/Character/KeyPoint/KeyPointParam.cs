using UnityEngine;
using System.Collections;
using Cinemachine;


[CreateAssetMenu(menuName = "KeyPointParam")]
[System.Serializable]
public class KeyPointParam : ScriptableObject {
    public float accelSpeed = 0.7f; // Pointerの加速スピード
    public float MaxSpeed = 1.7f;   // Pointerの最大速度
    public float MinSpeed = 0.15f;  // Pointerの最小速度
    public float inertia = 0.225f; //惰性
    public float deltaAngle = 15f; // 反転スピード
    public float offsetForward = 1f; //進行方向によるPointerのオフセット

    public float turnAnimationTime = 0.083f;    //回転アニメーションの時間
    public float turnMoveSpeed = 1f;            //回転中の移動速度

    public float playerNearXOffset = 0.1f;  //プレイヤーのそばにいるときのxオフセット
    public float playerNearYOffset = 0.1f;  //プレイヤーのそばにいるときのYオフセット
}