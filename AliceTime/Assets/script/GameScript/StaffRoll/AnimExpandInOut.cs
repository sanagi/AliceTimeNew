using UnityEngine;
using System.Collections;

public class AnimExpandInOut : MonoBehaviour {
    [SerializeField]
    private float animationTime; //イン,アウトにかかる時間
    private float animationSize; //アニメーションする量
    private float defaultSize;

    private bool isExpandHeight;
    private bool isExpandWidth;
    

    [SerializeField]
    private float _startTime; //アニメーション開始時刻キャッシュ
    private RectTransform _rectTransform;

    private void Awake()
    {
        animationTime = 0f;
        animationSize = 0f;
        defaultSize = 0f;
        isExpandWidth = false;
        isExpandHeight = false;

        this._startTime = Time.time;
        this._rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    public void SetExpandHeight(float animationTime, float height)
    {
        this.animationTime = animationTime;
        this.animationSize = height;
        this.isExpandHeight = true;
        this.defaultSize = this._rectTransform.rect.height;
    }

    public void SetExpandWidth(float animationTime, float width)
    {
        this.animationTime = animationTime;
        this.animationSize = width;
        this.isExpandWidth = true;
        this.defaultSize = this._rectTransform.rect.width;
    }

    public void SetPosition(Vector2 pos)
    {
        _rectTransform.localPosition = pos;
    }

    private void FixedUpdate()
    {
        var diffTime = Time.time - _startTime;
        if (diffTime > animationTime)
        {
            if (this.isExpandHeight)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, animationSize > 0 ? animationSize : 0);
            }
            else if (this.isExpandWidth)
            {
                _rectTransform.sizeDelta = new Vector2(animationSize > 0 ? animationSize : 0, _rectTransform.rect.height);
            }
            GameObject.Destroy(this);
            return;
        }

        var deltaSize = Vector2.zero;
        if (this.isExpandHeight)
        {
            if(animationSize >= 0)
            {
                deltaSize = new Vector2(_rectTransform.rect.width, animationSize * diffTime / animationTime);
            } else
            {
                deltaSize = new Vector2(_rectTransform.rect.width, -animationSize * (1 - diffTime / animationTime));
            }
        }
        else if (this.isExpandWidth)
        {
            if(animationSize >= 0)
            {
                deltaSize = new Vector2(animationSize * diffTime / animationTime, _rectTransform.rect.height);
            }
            else
            {
                deltaSize = new Vector2(-animationSize * (1 - diffTime / animationTime), _rectTransform.rect.width);
            }
        }
        _rectTransform.sizeDelta = deltaSize;
    }
}
