using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rewired;

public class TitleButtonBehaviour : MonoBehaviour, IButtonEvent
{
    public enum ButtonType
    {
        OK,
        Cancel
    }
    public MAINSCENE TargetSceneManager = MAINSCENE.TITLE;
    public TITLESCENE Title_NextPhaseName = TITLESCENE.START;

    public bool ResetStart = false;

    public int Stage = 0;
    public int Area = 0;

    public SoundId SE = SoundId.System_Decide;

    public Image buttonImage;
    public Text buttonText;
    [SerializeField]
    private Color FontDefaultColor;

    //ボタンが選ばれてるかどうか
    public bool isSelectedActive = false;
    const string SUBMIT = "UISubmit";
    const string PLUS = "UIPlus";
    const float SELECTSCALE = 1.15f;
    const float SELECTSCALETIME = 0.15f;

    public ButtonType MyButtonType = ButtonType.OK;

    // 初期化
    void Awake()
    {
        buttonImage = gameObject.GetComponent<Image>();

#if UNITY_ANDROID || UNITY_IOS
        if (Title_NextPhaseName == TITLESCENE.DIALOG)
        {
            gameObject.SetActive(false);
        }
#endif
    }

    void OnEnable()
    {
        if (!TitleManager.Instance.wait && TitleManager.Instance.init)
        {
            
        }
        if (isSelectedActive)
        {

        }
        else
        {
            
        }
    }
    
    public void Update()
    {
        if(TitleUIManager.UiPlayer != null)
        {
            if (!TitleUIManager.IsContorollable)
            {
                return;
            }
            //特定の動作のときは操作できないように
            if ((TitleSceneManager.CurrentPhase.GetType() == typeof(Title_Start) && (TitleUIManager.UiPlayer.GetButtonUp(PLUS)) || TitleUIManager.UiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
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

    // ボタンのイベント処理
    public void GotoNextPhase()
    {
        SoundManager.Instance.PlaySound(SE);
        if (TargetSceneManager == MAINSCENE.TITLE)
        {
            switch (Title_NextPhaseName)
            {
                case TITLESCENE.START:
                    TitleSceneManager.Goto(GameDefine.TITLE_START);
                    break;
                case TITLESCENE.STORY:
                    TitleSceneManager.Goto(GameDefine.TITLE_STORY);
                    break;
                case TITLESCENE.DIALOG:
                    TitleSceneManager.Goto(GameDefine.TITLE_DIALOG);
                    break;
                case TITLESCENE.OPTION:
                    TitleManager.Instance.BackGroundLogoEng.SetActive(false);
                    TitleManager.Instance.BackGroundLogoJpg.SetActive(false);
                    TitleSceneManager.Goto(GameDefine.TITLE_OPTION);
                    break;
                case TITLESCENE.LOGO:
                    TitleSceneManager.Goto(GameDefine.TITLE_LOGO);
                    break;
            }
        }
        //ゲームシーンにうつるとき
        else if (TargetSceneManager == MAINSCENE.AREASELECT)
        { // はじめからボタン
            
            //ステージ読み込み先変更
            //今はいったんデバッグへ
            /*int mode = TitleSceneManager.CurrentPhase.GetType() == typeof(Title_Story) ? 1 : 0;
            if (TitleSceneManager.CurrentPhase.GetType() == typeof(Title_Dialog))
            {
                mode = 1;
            }
            string selectedID = string.Format("{0}{1:D2}{2:D2}", mode, Stage, Area);
            */
            ((Title)MainSceneManager.CurrentPhase).SelectedID = GameDefine.AREA_INIT_ID;

            if (ResetStart)
            {
                //データがあれば警告を出す
                if (SaveManager.Instance.Check_ClearStoryStage(0) && MyButtonType == ButtonType.OK)
                {
                    TitleSceneManager.Goto(GameDefine.TITLE_DIALOG);
                    return;
                }
                else {
                    SaveManager.Instance.Story_Hajime();
                }
            }
            FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeOut(() =>
            {
                //マップ選択シーンへ遷移
                MainSceneManager.Goto(GameDefine.AreaSelect);
                //プロローグみたいなイベントが入るならイベントシーン遷移かも
                //MainSceneManager.Goto(GameDefine.GAME);
            }));
        }
        /*else if (TargetSceneManager == MAINSCENE.AREASELECT)
        { // つづきからボタン
            /*
             var continueData = SaveManager.Instance.Read_ContinueData();
             
            if (continueData.stageNumber != -1)
            {
                //ステージ続きデータがあればそちらへ
                Stage = continueData.stageNumber;
                Area = continueData.areaNumber;
                int mode = TitleSceneManager.CurrentPhase.GetType() == typeof(Title_Story) ? 1 : 0;
                string selectedID = string.Format("{0}{1:D2}{2:D2}", mode, Stage, Area);
                ((Title)MainSceneManager.CurrentPhase).SelectedID = selectedID;

                TitleSceneManager.Goto("Title_Final");
                CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
                    {
                        MainSceneManager.Goto("Game");
                    }));
            }
            else
            {
                //無ければマップ選択画面へ
                CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
                    {
                        TitleSceneManager.Goto("Title_Final");
                        MainSceneManager.Goto("World");
                    }));
            }
            
        }*/
    }

    private void DefaultImageSet()
    {
        if (buttonImage == null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        if (buttonImage != null)
        {
            buttonImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponent<Text>();
        }
        if (buttonText != null)
        {
            buttonText.color = FontDefaultColor;
        }
    }
    
    #region IButtonEvent implementation

    public void FireButtonEvent()
    {
        if (buttonImage == null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        if (buttonImage != null)
        {
            buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
        }
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponent<Text>();
        }
        if (buttonText != null)
        {
            buttonText.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
        }
    }

    public void ReleaseButtonEvent()
    {
        GotoNextPhase();
        //DefaultImageSet();
    }

    public void ReleaseOutButtonEvent()
    {
        if (!TitleManager.Instance.openAnimation && !TitleManager.Instance.trialNextOpen)
        {
            DefaultImageSet();
        }
    }
    
    #endregion
    
}
