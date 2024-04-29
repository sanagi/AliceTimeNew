using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UIがどういう初期化したいかを定義しておく
/// </summary>
public class CanvasInitializer : MonoBehaviour
{
    Canvas canvas;
    [SerializeField]
    private bool useSubCamera;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Destroy(this);
            return;
        }
        if (!useSubCamera)
        {
            canvas.worldCamera = CameraManager.Instance.MainCamera;
        }
        else
        {
            canvas.worldCamera = CameraManager.Instance.SubCamera;
        }
    }

    public void SetRenderMode(RenderMode mode)
    {
        canvas.renderMode = mode;
    }
}
