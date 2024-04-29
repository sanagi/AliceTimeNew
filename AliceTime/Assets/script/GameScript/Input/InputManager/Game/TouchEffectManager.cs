using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KamioriInput;

/*
バックキーの対応もここで(新しく作るエンバグ回避)
*/
public class TouchEffectManger : UIManager
{

    public bool dragEffect = true;

    private ParticleSystem effectTouch;
    public ParticleSystem TouchEffect
    {
        get
        {
            if (effectTouch != null)
            {
                return effectTouch;
            }

            var effect = Resources.Load<GameObject>("Input/TouchEffect") as GameObject;
            var effectObj = Instantiate(effect) as GameObject;
            DontDestroyOnLoad(effectObj);
            effectObj.transform.SetParent(transform);

            effectTouch = effectObj.GetComponent<ParticleSystem>();
            effectTouch.loop = false;
            return effectTouch;
        }
    }

    private ParticleSystem effectDrag;
    private ParticleSystem DragEffect
    {
        get
        {
            if (effectDrag != null)
            {
                return effectDrag;
            }

            var effect = Resources.Load<GameObject>("Input/DragEffect") as GameObject;
            var effectObj = Instantiate(effect) as GameObject;
            DontDestroyOnLoad(effectObj);
            effectObj.transform.SetParent(transform);

            effectDrag = effectObj.GetComponent<ParticleSystem>();
            effectDrag.loop = false;
            return effectDrag;
        }
    }

    static private TouchEffectManger _instance;
    static public TouchEffectManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TouchEffectManger>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (MainSceneManager.CurrentPhase.PhaseName)
            {
                case "Title":
                    switch (TitleSceneManager.CurrentPhase.PhaseName)
                    {
                        case "Title_Story":
                        case "Title_Trial":
                        case "Title_Option":
                            Audio_Manage.Play(SoundEnum.SE_CANCEL);
                            TitleSceneManager.Goto("Title_SelectMode");
                            break;
                        case "Title_Dialog":
                            Audio_Manage.Play(SoundEnum.SE_CANCEL);
                            TitleSceneManager.Goto("Title_Story");
                            break;
                    }
                    break;
                case "Game":
                    switch (GameSceneManager.CurrentPhase.PhaseName)
                    {
                        case "Game_Main":
                            Audio_Manage.Play(SoundEnum.SE_PAUSE);
                            GameSceneManager.Goto("Game_Pause");
                            break;
                        case "Game_Pause":
                            Audio_Manage.Play(SoundEnum.SE_OK);
                            GameSceneManager.Goto("Game_Main");
                            break;
                        default:
                            break;
                    }
                    break;
                case "Gallery":
                    switch (GallerySceneManager.CurrentPhase.PhaseName)
                    {
                        case "Gallery_Top":
                            Audio_Manage.Play(SoundEnum.SE_STAGESELECT);
                            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
                            {
                                GallerySceneManager.Goto("Gallery_Final");
                                MainSceneManager.Goto("Title");
                            }));
                            break;
                        case "Gallery_Detail":
                            Audio_Manage.Play(SoundEnum.SE_TouchCancel);
                            GallerySceneManager.Goto("Gallery_Top");
                            break;
                    }
                    break;
                case "World":
                    switch (WorldSceneManager.CurrentPhase.PhaseName)
                    {
                        case "World_Main":
                            Audio_Manage.Play(SoundEnum.SE_OK);
                            WorldSceneManager.Goto("World_Dialog");
                            break;
                        case "World_Dialog":
                        case "World_StageDialog":
                            Audio_Manage.Play(SoundEnum.SE_CANCEL);
                            WorldSceneManager.Goto("World_Main");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
#endif
    }

#region implemented abstract members of UIManager

    public override void DoCrossKeyEvent(KeyInfo info)
    {

    }

    public override void DoJumpKey(KeyInfo info)
    {

    }

    public override bool DoTouchBegan(TouchInfo[] info)
    {
#if UNITY_ANDROID
        if (dragEffect)
        {
            // エフェクトの処理
            var pos = Camera.main.ScreenToWorldPoint(info[0].currentScreenPosition); pos.z = -8f;
            TouchEffect.transform.position = pos;
            TouchEffect.Play();
        }
#endif
        return false;
    }

    public override bool DoTouchMoved(TouchInfo[] info)
    {
#if UNITY_ANDROID
        if (dragEffect)
        {
            // エフェクト処理
            var pos = Camera.main.ScreenToWorldPoint(info[0].currentScreenPosition); pos.z = -8f;
            DragEffect.transform.position = pos;
            DragEffect.Emit(1);
        }
#endif
        return false;
    }

    public override bool DoTouchEnded(TouchInfo[] info)
    {
#if UNITY_ANDROID
        if (DragEffect.isPlaying)
        {
            DragEffect.Stop();
        }
#endif
        return false;
    }

#endregion

    public int MyOrder
    {
        get
        {
            return 10000;
        }
    }
}