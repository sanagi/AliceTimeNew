using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using DG.Tweening.Core.Easing;

[CreateAssetMenu(menuName = "RotateParam")]
[System.Serializable]
public class RotateParams : ScriptableObject
{
    public Vector3 position = Vector3.zero; //初期位置
    public float duration = 1.15f; //回すスピード
    public Ease easeType = Ease.InCubic; //補間
    public RotateMode rotateMode = RotateMode.WorldAxisAdd; //回転モード指定
}