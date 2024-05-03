using UnityEngine;
using System.Collections;


/// <summary>
/// 汎用マネージャ基底
/// </summary>
public class CommonManagerBase : MonoBehaviour
{
    protected void Awake()
    {
        //更新禁止
        Initialize();
    }
    protected void Start()
    {
        //更新禁止
    }
    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Initialize()
    {
    }
}
