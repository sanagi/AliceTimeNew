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
public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    private string FOLDER_EFFECT = "Effect/";
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
        GameObject effectObj = Instantiate(Resources.Load(FOLDER_EFFECT + effectId), makePos, makeRot) as GameObject;
        //子供にする
        effectObj.transform.parent = transform;
        //オブジェクトを渡す
        return effectObj;
    }
    
    /// <summary>
    /// エフェクト発行	詳細 + 親指定版
    /// </summary>
    public GameObject PlayEffect(EffectId effectId, Vector3 makePos, Quaternion makeRot, Transform parent)
    {
        //プレハブを生成
        GameObject effectObj = Instantiate(Resources.Load(FOLDER_EFFECT + effectId), makePos, makeRot) as GameObject;
        //子供にする
        effectObj.transform.parent = parent;
        //オブジェクトを渡す
        return effectObj;
    }





}



