using UnityEngine;
using System.Collections;


/// <summary>
/// 一定時間後に自動的にオブジェクトを破棄するスクリプト
/// </summary>
public class FX_Timer : MonoBehaviour
{



    public float _waitSec = 1.0f;           //待ち時間



    /// <summary>
    /// 消滅までの時間を指定
    /// </summary>
    public void SetTimer(float sec)
    {
        _waitSec = sec;
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //時間経過
        _waitSec -= Time.deltaTime;
        if (_waitSec <= 0)
        {
            //時間切れになったら破棄
            Destroy(gameObject);
        }
    }


}
