using UnityEngine;
using System.Collections;




/// <summary>
/// 即死スクリプト
/// </summary>
public class FX_Kill : MonoBehaviour
{


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Awake()
    {
        Destroy(gameObject);
    }


}
