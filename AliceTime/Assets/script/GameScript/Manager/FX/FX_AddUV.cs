using UnityEngine;
using System.Collections;




/// <summary>
/// 一定量ずつUVを更新し続けるスクリプト
/// </summary>
public class FX_AddUV : MonoBehaviour
{



    public Vector2 addUvPerSecond = new Vector2(1, 0);          //加算するUVの量
    private Vector2 uvOffsetNow = Vector2.zero;                 //UVオフセット

    private Material targetMaterial = null;                     //更新するマテリアル



    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        //レンダラー取得
        Renderer tempRend = gameObject.GetComponent<Renderer>();
        if (null == tempRend)
        {
            //なければ子供からも検索
            foreach (Transform child1 in transform)
            {
                //レンダラー取得
                tempRend = child1.gameObject.GetComponent<Renderer>();
                if (tempRend)
                {
                    //発見した
                    break;
                }
            }
        }
        if (null == tempRend)
        {
            //レンダラーが見つからなければ消す
            Destroy(gameObject);
            return;
        }

        //レンダラーからマテリアルを取得
        targetMaterial = tempRend.material;
        //マテリアルのオフセットを取得
        uvOffsetNow = targetMaterial.mainTextureOffset;
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //経過時間分の移動を加える
        uvOffsetNow += (addUvPerSecond * Time.deltaTime);
        //マテリアルのオフセットを更新
        targetMaterial.mainTextureOffset = uvOffsetNow;
    }



}
