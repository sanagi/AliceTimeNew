using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EndingAnimationManager : MonoBehaviour {
    [System.Serializable]
    public class StaffRollItem {
        [NonSerialized]
        public bool Already = false; // 既に再生済みかどうかのフラグ   

        public float AppearanceTime; // スタッフロールが開始されてからの出現時間
        public AnimationType AnimationType = AnimationType.BOTTOM_TOP; // アニメーションタイプ
        public RectTransform rectTransform;
        [Range(0.1f, 2.0f)]
        public float AnimationSpeed = 1.0f; //アニメーションスピード

        public Vector2 InitPosForFadeIn; //出現座標（フェードのみ）
        public float value;
    }

    public enum AnimationType
    {
        BOTTOM_TOP,
        BOTTOM_CENTER,
        FADE_IN,
        FADE_OUT,
        EXPAND_IN,
        EXPAND_OUT,
    }


    // 再生時間
    public float animationTime = 0f;
    // BGMソース
    public AudioSource bgm;

    // TextUIリスト
    public List<StaffRollItem> items;


    public void StartStaffRoll(Action callback)
    {
        StartCoroutine(StartStaffRollCoroutine(callback));
    }

    private IEnumerator StartStaffRollCoroutine (Action callback)
    {
        // 各種UIリストのコピー
        StaffRollItem[] _items = new StaffRollItem[items.Count];
        items.CopyTo(_items, 0);

        // BGMの再生
        if(bgm != null)
        {
            bgm.Play();
        }

        // スタッフロールの開始
        var startTime = Time.time;
        LoadManager.Instance.HideLoadingMessage();
        while (true)
        {
            var deltaTime = Time.time - startTime;
            if (deltaTime > animationTime)
            {
                break;
            }

            DrawUI(ref _items, deltaTime);
            
            yield return null;
        }

        callback();
        yield break;
    }

    private void DrawUI(ref StaffRollItem[] _items, float deltaTime)
    {
        int count = _items.Length;
        for(int i=0; i<count; i++)
        {
            if (_items[i].Already || _items[i].rectTransform == null)
                continue;

            if(_items[i].AppearanceTime < deltaTime)
            {
                switch (_items[i].AnimationType)
                {
                    case AnimationType.BOTTOM_TOP:
                        var bottomToTopComponent = _items[i].rectTransform.gameObject.AddComponent<AnimBottomToTop>();
                        bottomToTopComponent.SetVector(Vector2.up * _items[i].AnimationSpeed);
                        bottomToTopComponent.SetDelayDeleteTime(this.animationTime);
                        bottomToTopComponent.SetPosition(Vector2.down * 300f);
                        break;
                    case AnimationType.BOTTOM_CENTER:
                        var bottomToCenterComponent = _items[i].rectTransform.gameObject.AddComponent<AnimBottomToTop>();
                        bottomToCenterComponent.SetVector(Vector2.up * _items[i].AnimationSpeed);
                        bottomToCenterComponent.SetDelayDeletePosition(Vector2.zero);
                        bottomToCenterComponent.SetPosition(Vector2.down * 300f);
                        break;
                    case AnimationType.FADE_IN:
                        var fadeInComponent = _items[i].rectTransform.gameObject.AddComponent<AnimFadeInOut>();
                        fadeInComponent.SetFadeIn(_items[i].AnimationSpeed);
                        fadeInComponent.SetPosition(_items[i].InitPosForFadeIn);
                        break;
                    case AnimationType.FADE_OUT:
                        var fadeOutComponent = _items[i].rectTransform.gameObject.AddComponent<AnimFadeInOut>();
                        fadeOutComponent.SetFadeOut(_items[i].AnimationSpeed);
                        break;
                    case AnimationType.EXPAND_IN:
                        var expandInComponent = _items[i].rectTransform.gameObject.AddComponent<AnimExpandInOut>();
                        expandInComponent.SetExpandHeight(_items[i].AnimationSpeed, _items[i].value);
                        break;
                    case AnimationType.EXPAND_OUT:
                        var expandOutComponent = _items[i].rectTransform.gameObject.AddComponent<AnimExpandInOut>();
                        expandOutComponent.SetExpandHeight(_items[i].AnimationSpeed, -_items[i].value);
                        break;
                }
                _items[i].Already = true;
            }
        }
    }
}
