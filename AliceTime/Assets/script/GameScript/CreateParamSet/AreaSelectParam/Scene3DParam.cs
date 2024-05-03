using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEditor.UIElements;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "3DSceneParam")]
[System.Serializable]
public class Scene3DParam : ScriptableObject {
    public Vector3 Player_InitPos;	//プレイヤーの初期位置
    public Vector3 Player_InitRot; //プレイヤーの回転
    public string PlayerPath;
    
    public Vector3 Camera_InitPos; //カメラの初期位置
    public Vector3 Camera_InitRot; //カメラの初期回転

    public Vector3 Stage_Pos; //ステージ位置
    public Vector3 Stage_Rot; //ステージ回転
    public string StagePath;

    public string DisplayName;

}