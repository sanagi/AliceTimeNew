using UnityEngine;
using System.Collections;




/// <summary>
/// 一定量ずつ回転を更新し続けるスクリプト
/// </summary>
public class FX_AddRotation : MonoBehaviour
{



    public Vector3 rotationSpeedPerSecond = new Vector3(0, 0, 60);      //加算する角度
    private Vector3 rotationNow = Vector3.zero;                     //現在の角度(オイラー角)



    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //角度を取得
        rotationNow = transform.localRotation.eulerAngles;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //経過時間分の回転を加える
        rotationNow += (rotationSpeedPerSecond * Time.deltaTime);
        //Transformを更新して角度を反映
        transform.localRotation = Quaternion.Euler(rotationNow);
    }

}
