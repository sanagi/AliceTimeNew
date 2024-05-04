using UnityEngine;
using UnityEngine.UI;

public class AreaSelectButtonBehaviour : MonoBehaviour, IButtonEvent
{
    public MAINSCENE TargetSceneManager = MAINSCENE.AREASELECT;
    public AREASELECT AreaSelect_NextPhaseName = AREASELECT.INIT;

    public SoundId SE = SoundId.System_Decide;

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
        SoundManager.Instance.PlaySound(SE);

        var continueData = SaveManager.Instance.Read_ContinueData();
        if (TargetSceneManager == MAINSCENE.AREASELECT)
        {
            switch (AreaSelect_NextPhaseName)
            {
                case AREASELECT.INIT:
                    GameSceneManager.Goto(GameDefine.GAME_INIT);
                    break;
                case AREASELECT.START:
                    GameSceneManager.Goto(GameDefine.GAME_START);
                    break;
                case AREASELECT.MAIN:
                    GameSceneManager.Goto(GameDefine.GAME_MAIN);
                    break;
                case AREASELECT.EVENT:
                    GameSceneManager.Goto(GameDefine.GAME_EVENT);
                    break;
                case AREASELECT.PAUSE:
                    GameSceneManager.Goto(GameDefine.GAME_PAUSE);
                    break;
                case AREASELECT.CAMERA:
                    GameSceneManager.Goto(GameDefine.GAME_CAMERA);
                    break;
            }
        }
        else if (TargetSceneManager == MAINSCENE.TITLE)
        {
            AreaSelectUIManager.HidePanel(AREASELECT.PAUSE);
            LoadManager.Instance.StartCoroutine(LoadManager.Instance.FadeOut(() =>
            {
                MainSceneManager.Goto(GameDefine.TITLE);
            }));
        }
        else if (TargetSceneManager == MAINSCENE.AREASELECT)
        {
            AreaSelectUIManager.HidePanel(AREASELECT.PAUSE);
            LoadManager.Instance.StartCoroutine(LoadManager.Instance.FadeOut(() =>
            {
                continueData.ResetStageData();
                SaveManager.Instance.clearNewStage = false;
                MainSceneManager.Goto(GameDefine.AreaSelect);
            }));
        }
    }

    public virtual void Update()
    {
        if (AreaSelectUIManager.uiPlayer != null)
        {
            if ((AreaSelectUIManager.uiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
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
