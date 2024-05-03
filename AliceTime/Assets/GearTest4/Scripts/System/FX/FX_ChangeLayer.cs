using UnityEngine;
using System.Collections;




/// <summary>
/// 貼り付けたオブジェクト以下のレイヤーを変更するスクリプト
/// </summary>
public class FX_ChangeLayer : MonoBehaviour
{
    /// <summary>
    /// レイヤーを指定のものに変更する
    /// </summary>
    public void SetLayer(LayerId id)
    {
        //レイヤーを更新
        SetLayerSub(transform, id);
    }
    /// <summary>
    /// 指定したTransform以下のレイヤーを変更する
    /// </summary>
    private void SetLayerSub(Transform parent, LayerId id)
    {
        //まず親のレイヤーを更新
        parent.gameObject.layer = (int)id;

        //その子供を検索
        foreach (Transform child1 in parent)
        {
            //子供にも反映
            SetLayerSub(child1, id);
        }
    }



}
