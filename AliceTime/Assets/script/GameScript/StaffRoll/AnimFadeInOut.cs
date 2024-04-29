using UnityEngine;
using System.Collections;

public class AnimFadeInOut : MonoBehaviour {
    [SerializeField]
    private float animationTime; //フェードイン,アウトにかかる時間

    private bool isFadeIn;
    private bool isFadeOut;

    
    [SerializeField]
    private float _startTime; //アニメーション開始時刻キャッシュ
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        animationTime = 0f;
        isFadeIn = false;
        isFadeOut = false;

        this._startTime = Time.time;
        this._rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    public void SetFadeIn(float animationTime)
    {
        this.animationTime = animationTime;
        this.isFadeIn = true;
        this._canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        this._canvasGroup.alpha = 0f;
    }

    public void SetFadeOut(float animationTime)
    {
        this.animationTime = animationTime;
        this.isFadeOut = true;
        this._canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        this._canvasGroup.alpha = 1f;
    }
    

    public void SetPosition(Vector2 pos)
    {
        _rectTransform.localPosition = pos;
    }

    private void FixedUpdate()
    {
        if (_canvasGroup == null)
        {
            return;
        }

        var diffTime = Time.time - _startTime;
        if ((isFadeOut && _canvasGroup.alpha == 0) || (isFadeIn && _canvasGroup.alpha == 1f) || diffTime > animationTime)
        {
            if (isFadeOut)
            {
                this.gameObject.SetActive(false);
            }
            _canvasGroup.alpha = 0f;
            GameObject.Destroy(this._canvasGroup);
            this._canvasGroup = null;

            GameObject.Destroy(this);
            return;
        }

        if (this.isFadeIn)
        {
            _canvasGroup.alpha = diffTime / animationTime;
        }
        else if(this.isFadeOut)
        {
            _canvasGroup.alpha = 1 - diffTime / animationTime;
        }
    }
}

