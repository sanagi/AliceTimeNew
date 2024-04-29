using UnityEngine;
using UnityEngine.UI;

public class ExploreButtonBehaviour : MonoBehaviour, IButtonEvent
{
    public MAINSCENE TargetSceneManager = MAINSCENE.GAME;
    public GAMESCENE Game_NextPhaseName = GAMESCENE.INIT;

    public SoundEnum SE = SoundEnum.SE_STAGESELECT;

    public bool isSelectedActive = false;
    public const string SUBMIT = "UISubmit";
    public const string PLUS = "UIPlus";
    const float SELECTSCALE = 1.15f;
    const float SELECTSCALETIME = 0.15f;

    public Image buttonImage;
    void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();
    }

    void OnEnable()
    {
        DefaultImageSet();
    }

    public void GotoNextPhase()
    {
        Audio_Manage.Play(SE);

        var continueData = SaveManager.Instance.Read_ContinueData();
        if (TargetSceneManager == MAINSCENE.GAME)
        {
            switch (Game_NextPhaseName)
            {
                case GAMESCENE.INIT:
                    GameSceneManager.Goto(GameDefine.GAME_INIT);
                    break;
                case GAMESCENE.START:
                    GameSceneManager.Goto(GameDefine.GAME_START);
                    break;
                case GAMESCENE.MAIN:
                    GameSceneManager.Goto(GameDefine.GAME_MAIN);
                    break;
                case GAMESCENE.DEATH:
                    GameSceneManager.Goto(GameDefine.GAME_DEATH);
                    break;
                case GAMESCENE.EVENT:
                    GameSceneManager.Goto(GameDefine.GAME_EVENT);
                    break;
                case GAMESCENE.PAUSE:
                    GameSceneManager.Goto(GameDefine.GAME_PAUSE);
                    break;
                case GAMESCENE.CAMERA:
                    GameSceneManager.Goto(GameDefine.GAME_CAMERA);
                    break;
            }
        }
        else if (TargetSceneManager == MAINSCENE.TITLE)
        {
            Audio_Manage.Play(SE);
            GameUIManager.HidePanel(GAMESCENE.PAUSE);
            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
            {
                MainSceneManager.Goto(GameDefine.TITLE);
            }));
        }
        else if (TargetSceneManager == MAINSCENE.AREASELECT)
        {
            Audio_Manage.Play(SE);
            GameUIManager.HidePanel(GAMESCENE.PAUSE);
            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
            {
                continueData.ResetStageData();
                SaveManager.Instance.clearNewStage = false;
                MainSceneManager.Goto(GameDefine.AreaSelect);
            }));
        }
    }

    public virtual void Update()
    {
        if (GameUIManager.uiPlayer != null)
        {
            if ((GameUIManager.uiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
            {
                GotoNextPhase();
                isSelectedActive = false;
            }
        }

    }

    public void SelectedAnimation(float time = SELECTSCALETIME)
    {
        if (isSelectedActive)
        {
        }
        else
        {
            if (time == 0)
            {
                gameObject.transform.localScale = Vector3.one;
            }
            else
            {
            }
        }
    }

    #region IButtonEvent implementation

    public void FireButtonEvent()
    {
        if (buttonImage == null && gameObject != null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
    }

    public virtual void ReleaseButtonEvent()
    {
        GotoNextPhase();
        //DefaultImageSet();
    }

    public void ReleaseOutButtonEvent()
    {
        DefaultImageSet();
    }

    protected void DefaultImageSet()
    {
        if (buttonImage == null && gameObject != null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        buttonImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    #endregion
}
