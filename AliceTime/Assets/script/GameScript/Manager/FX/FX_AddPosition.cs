using UnityEngine;
using System.Collections;




/// <summary>
/// 一定量ずつ位置を更新し続けるスクリプト
/// </summary>
public class FX_AddPosition : MonoBehaviour
{



    public Vector3 addPosPerSecond = new Vector3(0, 0, 1);      //加算する量


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //経過時間分の回転を加える
        Vector3 vecPos = transform.position;
        vecPos += (addPosPerSecond * Time.deltaTime);
        //Transformを更新して角度を反映
        transform.position = vecPos;
    }



}
