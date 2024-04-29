using UnityEngine;
using System.Collections;

public class AspectOK : MonoBehaviour
{
    //------------------------------------------------------------- 
    //! @name	画面の位置関係. 
    //------------------------------------------------------------- 
    //@{ 
    public enum ALIGN
    {
        CENTER = 0, //!< 中央寄せ. 
        UP,         //!< 上寄せ. 
        DOWN,       //!< 下寄せ. 
        LEFT,       //!< 左寄せ. 
        RIGTH,      //!< 右寄せ. 
    };
    //@} 

    //------------------------------------------------------------- 
    //! @name	メンバー変数. 
    //------------------------------------------------------------- 
    //@{ 
    public float baseHeight = 960.0f;           //!< 基準となるスクリーン高さ. 
    public float baseWidth = 540.0f;            //!< 基準となるスクリーン幅. 
    public float offsetSafeAreaHeight = 0.04f;  //!< SafeArea用の高さオフセット
    public float offsetSafeAreaWidth = 0.0f;    //!< SafeArea用の幅オフセット
    public float screenScaleSafeArea = 0.95f;   //!< SafeArea用のスクリーン倍率
    public ALIGN screenAligh = ALIGN.CENTER;    //!< 画面寄せ 
    private float aspectRate;
    public Camera _backCamera;

    //@} 
    Camera _camera;
    //------------------------------------------------------------- 
    //! 開始. 
    //------------------------------------------------------------- 
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        aspectRate = (float)(baseWidth / baseHeight);
    }

    void CreateBackgroundCamera()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
            return;
#endif

        if (_backCamera != null)
            return;

        var backGroundCameraObject = new GameObject("Background Color Camera");
        _backCamera = backGroundCameraObject.AddComponent<Camera>();
        _backCamera.depth = -99;
        _backCamera.fieldOfView = 1;
        _backCamera.farClipPlane = 1.1f;
        _backCamera.nearClipPlane = 1;
        _backCamera.cullingMask = 0;
        _backCamera.depthTextureMode = DepthTextureMode.None;
        _backCamera.backgroundColor = Color.black;
        _backCamera.renderingPath = RenderingPath.VertexLit;
        _backCamera.clearFlags = CameraClearFlags.SolidColor;
        _backCamera.useOcclusionCulling = false;
        backGroundCameraObject.hideFlags = HideFlags.NotEditable;
    }


    bool CheckChangeAspect()
    {
        return _camera.aspect == aspectRate;
    }

    void Update()
    {
        if (CheckChangeAspect()) return;
        UpdateAspect();
        _camera.ResetAspect();
    }

    public void AspectChangeOff()
    {
        _camera.rect = new Rect(0, 0, 1, 1);
        this.enabled = false;
    }

    public void AspectChangeOn()
    {
        this.enabled = true;
        UpdateAspect();
    }

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

    void UpdateAspect()
    {
        var hasSafeArea = HasSafeArea();

        float baseAspect = baseHeight / baseWidth;
        float nowAspect = (float)Screen.height / (float)Screen.width;
        float changeAspect;
        if (baseAspect > nowAspect)
        {
            float y = hasSafeArea ? offsetSafeAreaHeight : 0;
            float height = hasSafeArea ? screenScaleSafeArea : 1f;

            // 横基準 
            changeAspect = nowAspect / baseAspect;
            changeAspect = hasSafeArea ? changeAspect * screenScaleSafeArea : changeAspect;
            switch (screenAligh)
            {
                case ALIGN.CENTER:
                    _camera.rect = new Rect((1 - changeAspect) * 0.5f, y, changeAspect, height);
                    break;
                case ALIGN.LEFT:
                    _camera.rect = new Rect((1 - changeAspect) * 0.0f, y, changeAspect, height);
                    break;
                case ALIGN.RIGTH:
                    _camera.rect = new Rect((1 - changeAspect) * 1.0f, y, changeAspect, height);
                    break;
            }
        }
        else
        {
            float x = hasSafeArea ? offsetSafeAreaWidth : 0;
            float width = hasSafeArea ? screenScaleSafeArea : 1f;

            // 縦基準 
            changeAspect = baseAspect / nowAspect;
            changeAspect = hasSafeArea ? changeAspect * screenScaleSafeArea : changeAspect;
            switch (screenAligh)
            {
                case ALIGN.CENTER:
                    _camera.rect = new Rect(x, (1 - changeAspect) * 0.5f, width, changeAspect);
                    break;
                case ALIGN.UP:
                    _camera.rect = new Rect(x, (1 - changeAspect) * 1.0f, width, changeAspect);
                    break;
                case ALIGN.DOWN:
                    _camera.rect = new Rect(x, (1 - changeAspect) * 0.0f, width, changeAspect);
                    break;
            }
        }
    }
}
