using UnityEngine;
using System.Collections;




/// <summary>
/// スケールを指定の位置まで更新するスクリプト
/// </summary>
public class FX_ZoomScale : MonoBehaviour
{



    public int scaleSpeed = 7;                          //更新速度(0に近いほど速い)
    public Vector3 scaleLast = new Vector3(1, 0, 1);    //目的のスケール
    private Vector3 scaleNow = Vector3.one;             //現在のローカルスケール



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
        //スケールを目的地に近づける
        scaleNow = (((scaleNow * scaleSpeed) + scaleLast) / (scaleSpeed + 1));
        //Transformを更新
        transform.localScale = scaleNow;
    }


}
