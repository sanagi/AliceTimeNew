using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEditor.UIElements;


[CreateAssetMenu(menuName = "CameraParam")]
[System.Serializable]
public class CameraParam : ScriptableObject {
    public Vector3 FollowOffset;	//追いかけるオフセット
    public float OlthoSize = 7; //画面の広さ
    public float FovSize = 30; //画角の広さ(透視投影)
    public float OutTime = 0.2f;
    public float InTime = 0.2f;
    public Color FadeColor = Color.black;

    //VirtualCamera
    public GameObject VirtualCameraFollow; //Follow用のVirtualCamera

}