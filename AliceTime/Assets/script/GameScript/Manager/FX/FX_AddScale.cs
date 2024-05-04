using UnityEngine;
using System.Collections;




/// <summary>
/// 一定量ずつスケールを更新し続けるスクリプト
/// </summary>
public class FX_AddScale : MonoBehaviour
{



    public Vector3 scaleSpeedPerSecond = new Vector3(1, 0, 1);  //加算するスケール量
    private Vector3 scaleNow = Vector3.one;                     //現在のローカルスケール



    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //最初の値を取得
        scaleNow = transform.localScale;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //経過時間分のスケールを加える
        scaleNow += (scaleSpeedPerSecond * Time.deltaTime);
        //Transformを更新して角度を反映
        transform.localScale = scaleNow;
    }


}
