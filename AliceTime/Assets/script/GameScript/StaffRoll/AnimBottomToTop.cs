using UnityEngine;
using System.Collections;

public class AnimBottomToTop : MonoBehaviour {

    [SerializeField]
    private float delayDeleteTime; //コンポーネントが消滅する時間
    private bool isDelayDeleteTime;

    [SerializeField]
    private Vector3 delayDeletePos; //コンポーネントが消滅する位置
    private bool isDelayDeletePos;

    [SerializeField]
    private Vector3 vec; //UIのアニメーション方向・速度


    [SerializeField]
    private float _startTime; //アニメーション開始時刻キャッシュ
    private Transform _transform;
    private RectTransform _rectTransform;

    private void Awake()
    {
        isDelayDeleteTime = false;
        isDelayDeletePos = false;

        delayDeleteTime = float.MaxValue;
        delayDeletePos = Vector2.one * float.MaxValue;
        vec = Vector3.zero;

        _startTime = Time.time;
        _transform = gameObject.transform;
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }


    public void SetDelayDeleteTime(float delayDeleteTime)
    {
        this.delayDeleteTime = delayDeleteTime;
        this.isDelayDeleteTime = true;
    }

    public void SetDelayDeletePosition(Vector2 delayDeletePos)
    {
        this.delayDeletePos = (Vector3)delayDeletePos;
        this.isDelayDeletePos = true;
    }


    public void SetVector(Vector2 vec)
    {
        this.vec = (Vector3)vec * 100;
    }

    public void SetPosition(Vector2 pos)
    {
        _rectTransform.localPosition = pos;
    }


    private void Update ()
    {
        if(isDelayDeleteTime && Time.time - _startTime > delayDeleteTime)
        {
            GameObject.Destroy(this);
        }
	}

    private void FixedUpdate()
    {
        if (_transform == null)
        {
            return;
        }

        if (isDelayDeletePos && Vector3.Distance(_rectTransform.localPosition, delayDeletePos) < 1f)
        {
            _rectTransform.localPosition = delayDeletePos;
            GameObject.Destroy(this);
        }
        _transform.localPosition += vec * Time.fixedDeltaTime;
    }
}
