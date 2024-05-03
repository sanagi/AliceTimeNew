using UnityEngine;
using System.Collections;




/*
    EffectManager.Instance.SetEffect( EffectId.TouchHit, Verctor3.zero);      //エフェクト発行
    EffectManager.Instance.SetEffect( EffectId.TouchHit, Verctor3.zero, LayerKind.Default);      //エフェクト発行
 */
//エフェクトの種類
public enum EffectId
{
    None = -1,
    TouchHit,       //タッチしたときに広がる波紋
    EnumMax
}




/// <summary>
/// 画面上に発行するエフェクト(パーティクルなど)の管理
/// </summary>
public class EffectManager : CommonManagerBase
{
    //外部アクセス用
    public static EffectManager Instance;
    private static int _instanceCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        _instanceCount++;
        if (Instance)
        {
            return;
        }
        //インスタンス設定
        Instance = this;
    }
    /// <summary>
    /// 破棄時
    /// </summary>
    private void OnDisable()
    {
        if (--_instanceCount <= 0)
        {
            //インスタンス破棄
            Instance = null;
        }
    }





    /// <summary>
    /// エフェクト発行	簡易版
    /// </summary>
    public GameObject PlayEffect(EffectId effectId, Vector3 makePos)
    {
        //               種類      位置     回転
        return PlayEffect(effectId, makePos, Quaternion.identity);
    }
    /// <summary>
    /// エフェクト発行	詳細版
    /// </summary>
    public GameObject PlayEffect(EffectId effectId, Vector3 makePos, Quaternion makeRot)
    {
        //プレハブを生成
        GameObject effectObj = Instantiate(Resources.Load("System/Effect/" + effectId), makePos, makeRot) as GameObject;
        //子供にする
        effectObj.transform.parent = transform;
        //オブジェクトを渡す
        return effectObj;
    }





}



