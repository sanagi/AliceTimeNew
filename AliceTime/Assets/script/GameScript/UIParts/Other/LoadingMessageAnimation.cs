using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingMessageAnimation : MonoBehaviour {
    private string message = "Now Loading";
    [SerializeField]
    private float switchingTime = 0.015f;

    private Text m_text;
    private int count;

    private IEnumerator coroutine;

    protected void Init() {
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            message = "よみこみちゅう";
        }
        else
        {
            message = "Now Loading";
        }
            
        m_text = GetComponent<Text>();
        m_text.text = message;
        count = 0;
        coroutine = null;
    }

    protected void Deinit() {
        m_text.text = message;
        count = 0;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    void OnEnable() {
        Init();
        coroutine = Animation();
        StartCoroutine(coroutine);
    }

    void OnDisable() {
        Deinit();
    }

    IEnumerator Animation() {
        while(true) {
            var startTime = Time.time;
            while(Time.time - startTime < switchingTime) {
                yield return null;
            }
            count++;
            var isInc = (((count / 4) % 2) == 0);
            var _count = count % 4;
            var _message = message;
            if (isInc) {
                for (; _count > 0; _count--) {
                    _message += '.';
                }
            } else {
                for (; 3-_count > 0; _count++) {
                    _message += '.';
                }
            }
            m_text.text = _message;
            yield return null;
        }
    }

}
