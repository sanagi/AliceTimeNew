using UnityEngine;
using System.Collections;




/// <summary>
/// Sin波で動くスクリプト
/// </summary>
public class FX_SinScale : MonoBehaviour
{



    private Vector3 originalScale = Vector3.one;
    public Vector3 sinWave_Scale_Min = new Vector3(0.0f, 0.0f, 0);
    public Vector3 sinWave_Scale_Max = new Vector3(1.0f, 1.0f, 0);
    public float sinWave_RoundSec_Now = 0;
    public float sinWave_RoundSec_Max = 6;


    private void Awake()
    {
        originalScale = transform.localScale;
    }
    private void Update()
    {
        sinWave_RoundSec_Now += Time.deltaTime;

        Vector3 vecScale = originalScale;
        float percent = Mathf.Clamp01((1f + Mathf.Sin((360f * sinWave_RoundSec_Now / sinWave_RoundSec_Max) * Mathf.Deg2Rad)) / 2);
        vecScale += ((sinWave_Scale_Max - sinWave_Scale_Min) * percent) + sinWave_Scale_Min;
        //Transformを更新して角度を反映
        transform.localScale = vecScale;
    }



}
