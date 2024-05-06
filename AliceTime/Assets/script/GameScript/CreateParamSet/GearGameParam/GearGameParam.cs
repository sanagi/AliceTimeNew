using UnityEngine;
using System.Collections;
using Cinemachine;


[CreateAssetMenu(menuName = "GearGameParam")]
[System.Serializable]
public class GearGameParam : ScriptableObject {
    public float NextButtonDelay = 0.05f; // 連打防止
}