using UnityEngine;
using System.Collections;
using Cinemachine;


[CreateAssetMenu(menuName = "CursorParam")]
[System.Serializable]
public class CursorParam : ScriptableObject {
    public float AccelSpeed = 0.7f; // Pointerの加速スピード
    public float MaxSpeed = 1.7f;   // Pointerの最大速度
    public float MinSpeed = 0.15f;  // Pointerの最小速度
    public float Inertia = 0.225f; //惰性
    public float DelayPointTime = 0.01f;		//次のボタン押しまでの制限時間
}