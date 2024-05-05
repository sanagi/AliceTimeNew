using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField]
    private Image _fadeImage = null;
    
    // フェードアウト
    public IEnumerator FadeOut(Action complete = null, float time = 0)
    {
        _fadeImage.gameObject.SetActive(true);
        float fadeSec = 0f;
        do
        {
            fadeSec += Time.deltaTime;
            _fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(fadeSec));
            yield return null;
        } while (fadeSec < 1f);
        
        complete();
        yield return null;
    }

    // フェードイン
    public IEnumerator FadeIn(Action complete, float time = 0)
    {
        float fadeSec = 1f;
        do
        {
            fadeSec -= Time.deltaTime;
            _fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(fadeSec));
            yield return null;
        } while (fadeSec > 0f);
        
        _fadeImage.gameObject.SetActive(false);
        
        complete();
        yield return null;
    }
}