using UnityEngine;

/// <summary>
/// UI Monobehaviour.
/// </summary>
public abstract class UIMonobehaviour : MonoBehaviour
{
    protected abstract void Init();
    protected abstract void Deinit();

#if UNITY_IOS
    const float SCREEN_SCALE_FOR_SAFEAREA = 0.95f;

    private bool HasSafeArea()
    {
        var safeArea = Screen.safeArea;
#if UNITY_EDITOR
        if (Screen.width == 1125 && Screen.height == 2436)
        {
            safeArea.y = 102;
            safeArea.height = 2202;
        }
        if (Screen.width == 2436 && Screen.height == 1125)
        {
            safeArea.x = 132;
            safeArea.y = 63;
            safeArea.height = 1062;
            safeArea.width = 2172;
        }
#endif
        return safeArea.y > 0 || safeArea.x > 0;
    }
#endif

    protected virtual void Awake()
    {
#if UNITY_IOS
        if (HasSafeArea())
        {
            // Fix UI scale
            var rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one * SCREEN_SCALE_FOR_SAFEAREA;
            }
        }
#endif
        Init();
    }

    protected virtual void Start() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDestroy()
    {
        Deinit();
    }
}
