using UnityEngine;
using System.Collections;




/// <summary>
/// 点滅スクリプト
/// </summary>
public class FX_Blink : MonoBehaviour
{



    public Renderer _targetRenderer = null;             //レンダラー更新

    public float _waitSec_All = 0.5f;                   //点滅間隔
    public float _waitSec_Invisible = 0.1f;             //非表示の時間

    private float _waitSec_Now = 0.0f;                  //待機時間
    private bool _visibleFlag = true;                   //表示/非表示フラグ



    /// <summary>
    /// 起動時
    /// </summary>
    private void Awake()
    {
        //レンダラーを持ってるか確認
        if (null == _targetRenderer)
        {
            //自分の中にあるかもう一度確認
            _targetRenderer = gameObject.GetComponent<Renderer>();
            if (null == _targetRenderer)
            {
                //持ってないなら消す
                Destroy(this);
            }
        }
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //経過時間分の回転を加える
        _waitSec_Now = ((_waitSec_Now + Time.deltaTime) % _waitSec_All);
        //描画のON/OFF
        SetEnableFlag(_waitSec_Now > _waitSec_Invisible);
    }
    /// <summary>
    /// 表示/非表示切り替え
    /// </summary>
    private void SetEnableFlag(bool enable)
    {
        //変わってなければ無視
        if (_visibleFlag == enable)
        {
            //無視
            return;
        }
        //更新
        _visibleFlag = enable;

        //反映
        _targetRenderer.enabled = _visibleFlag;
    }



}
