using UnityEngine;
using System.Collections;


/// <summary>
/// 一定時間後にスクリプトを起こすスクリプト
/// </summary>
public class FX_DelayEnable : MonoBehaviour
{



    public MonoBehaviour _targetScript = null;          //起こす対象
    public float _delaySec = 1.0f;                      //遅延時間



    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        //まず寝かせる
        _targetScript.enabled = false;
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //時間経過
        _delaySec -= Time.deltaTime;
        if (_delaySec <= 0)
        {
            //時間切れになったら起こす
            _targetScript.enabled = true;
            //自分は破棄
            Destroy(this);
        }
    }


    /// <summary>
    /// 起こす時間を指定
    /// </summary>
    public void SetTimer(float sec)
    {
        _delaySec = sec;
    }


}
