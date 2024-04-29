using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TitleOptionPanel : TitlePanelBehaviour
{
    public enum OptionLabel
    {
        BGM,
        SE,
        LANG,

        TEXTSPEED,
        COPYSKIP,
        MOVESPEED
    }
    private EventSystem currentEventSystem;
    private PointerEventData pointer;
    private List<RaycastResult> result;
    const string SUBMIT = "UISubmit";

    [SerializeField]
    private TitleOptionArrowButton rightButton = null;
    [SerializeField]
    private TitleOptionArrowButton leftButton = null;

    [SerializeField]
    private VolumeButtonBehaviour bgmVolume = null;
    [SerializeField]
    private VolumeButtonBehaviour seVolume = null;
    [SerializeField]
    private VolumeButtonBehaviour textSpeedVolume = null;

    [SerializeField]
    private List<OptionButtonBehaviour> moveButtonList = new List<OptionButtonBehaviour>();
    [SerializeField]
    private List<OptionButtonBehaviour> skipButtonList = new List<OptionButtonBehaviour>();
    [SerializeField]
    private List<OptionButtonBehaviour> languageButtonList = new List<OptionButtonBehaviour>();

    private float uiTimer = 0.0f;
    private bool selectedMove = false;
    public List<Text> activeLabelArray = new List<Text>();
    private TitleOptionSelectPanel optionPanelController = null;
    private int oldIndex = 0;

    private VolumeButtonBehaviour operateVolumeButton = null;
    private const float sliderMulValue = 0.05f;
    private List<OptionButtonBehaviour> operateButtonList = new List<OptionButtonBehaviour>();
    private int selectIndexHorizon = 0;

    const float SELECTSCALE = 1.15f;
    const float SELECTSCALETIME = 0.15f;

    protected override void Init()
    {
        base.Init();

        currentEventSystem = EventSystem.current;
        pointer = new PointerEventData(currentEventSystem);
        result = new List<RaycastResult>();

        gameObject.SetActive(false);

        optionPanelController = gameObject.transform.Find("OptionGroup").GetComponent<TitleOptionSelectPanel>();

        SelectedAnimation(activeLabelArray[selectIndex].gameObject, SELECTSCALE);
        operateVolumeButton = bgmVolume;
        operateButtonList = languageButtonList;
    }

    protected override void OnEnable()
    {
        switch (optionPanelController.currentPage)
        {
            case 0:
                selectIndex = (int)OptionLabel.BGM;
                break;

            case 1:
                selectIndex = (int)OptionLabel.TEXTSPEED; 
                break;
        }
        SelectedAnimation(activeLabelArray[selectIndex].gameObject, SELECTSCALE);
        ChangeOperateParameter();
    }

    protected override void OnDisable()
    {
        ScaleReset();
    }

    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        // Awakeが呼ばれていない場合があるので対応
        if (currentEventSystem == null)
        {
            currentEventSystem = EventSystem.current;
            pointer = new PointerEventData(currentEventSystem);
        }

        pointer.position = screenPosition;
        currentEventSystem.RaycastAll(pointer, result);

        if (result.Count > 0)
        {
            if (result.Count > 1)
            {
                Debug.LogWarning("There is a possibility that the image is overlapped");
            }
            return result[0].gameObject.GetComponent<IButtonEvent>();
        }

        return null;
    }

    public void OperateParameter()
    {
        /*
        float xValue = TitleUIManager.uiPlayer.GetAxis(ACTION_MOVE_HORIZONTAL_UI);
        switch (selectIndex)
        {
            case (int)OptionLabel.BGM:
            case (int)OptionLabel.SE:
            case (int)OptionLabel.TEXTSPEED:
                if(xValue > BORDER && !selectedMove)
                {
                    operateVolumeButton.slider.value += sliderMulValue;
                }
                else if(xValue < BORDER * -1.0f && !selectedMove)
                {
                    operateVolumeButton.slider.value -= sliderMulValue;
                }
                operateVolumeButton.ValueChangeCheck();
                break;

            case (int)OptionLabel.LANG:
            case (int)OptionLabel.COPYSKIP:
            case (int)OptionLabel.MOVESPEED:
                if (TitleUIManager.uiPlayer.GetButtonUp(SUBMIT))
                {
                    operateButtonList[selectIndexHorizon].ReleaseButtonEvent();
                }
                else if (xValue > BORDER && !selectedMove)
                {
                    if (selectIndexHorizon < operateButtonList.Count-1)
                    {
                        selectIndexHorizon++;
                    }
                    SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, SELECTSCALE);
                    SelectedAnimation(operateButtonList[(operateButtonList.Count - 1) - selectIndexHorizon].gameObject, 1.0f);
                }
                else if (xValue < BORDER * -1.0f && !selectedMove)
                {
                    if (0 < selectIndexHorizon)
                    {
                        selectIndexHorizon--;
                    }
                    SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, SELECTSCALE);
                    SelectedAnimation(operateButtonList[(operateButtonList.Count - 1) - selectIndexHorizon].gameObject, 1.0f);
                }
                break;
        }
        */
    }

    private void ChangeOperateParameter()
    {
        switch (selectIndex)
        {
            case (int)OptionLabel.BGM:
                operateVolumeButton = bgmVolume;
                break;
            case (int)OptionLabel.SE:
                operateVolumeButton = seVolume;
                break;
            case (int)OptionLabel.LANG:
                operateButtonList = languageButtonList;
                selectIndexHorizon = 0;
                SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, SELECTSCALE);
                break;
            case (int)OptionLabel.TEXTSPEED:
                operateVolumeButton = textSpeedVolume;
                break;
            case (int)OptionLabel.COPYSKIP:
                operateButtonList = skipButtonList;
                selectIndexHorizon = 0;
                SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, SELECTSCALE);
                break;
            case (int)OptionLabel.MOVESPEED:
                operateButtonList = moveButtonList;
                selectIndexHorizon = 0;
                SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, SELECTSCALE);
                break;
        }
    }

    public void Update()
    {
#if UNITY_SWITCH
        OperateParameter();

        float yValue = TitleUIManager.uiPlayer.GetAxis(ACTION_MOVE_VERTICAL);
        oldIndex = selectIndex;

        if (yValue > BORDER && !selectedMove)
        {
            selectedMove = true;
            selectIndex--;

            switch (optionPanelController.currentPage)
            {
                case 0:
                    if (selectIndex < 0)
                    {
                        selectIndex = (int)OptionLabel.LANG;
                    }
                    break;
                case 1:
                    if (selectIndex < (int)OptionLabel.TEXTSPEED)
                    {
                        selectIndex = (int)OptionLabel.MOVESPEED;
                    }
                    break;
            }
            SelectedAnimation(activeLabelArray[selectIndex].gameObject, SELECTSCALE);
            SelectedAnimation(activeLabelArray[oldIndex].gameObject,1.0f);
            SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, 1.0f);
            ChangeOperateParameter();
        }
        else if (yValue < -1.0f * BORDER && !selectedMove)
        {
            selectedMove = true;
            selectIndex++;

            switch (optionPanelController.currentPage)
            {
                case 0:
                    if (selectIndex > (int)OptionLabel.LANG)
                    {
                        selectIndex = 0;
                    }
                    break;
                case 1:
                    if (selectIndex > (int)OptionLabel.MOVESPEED)
                    {
                        selectIndex = (int)OptionLabel.TEXTSPEED;
                    }
                    break;
            }
            SelectedAnimation(activeLabelArray[selectIndex].gameObject, SELECTSCALE);
            SelectedAnimation(activeLabelArray[oldIndex].gameObject,1.0f);
            SelectedAnimation(operateButtonList[selectIndexHorizon].gameObject, 1.0f);
            ChangeOperateParameter();
        }

        if (selectedMove)
        {
            uiTimer += Time.deltaTime;
            if (uiTimer > MARGINTIME)
            {
                selectedMove = false;
                uiTimer = 0f;
            }
        }

        if (TitleUIManager.uiPlayer.GetButtonUp(UIRIGHT))
        {
            if (rightButton != null && rightButton.gameObject.activeSelf)
            {
                rightButton.ReleaseButtonEvent(() =>
                {
                    selectIndex = (int)OptionLabel.TEXTSPEED;
                    SelectedAnimation(activeLabelArray[selectIndex].gameObject, SELECTSCALE);
                }
                );
            }
        }
        else if (TitleUIManager.uiPlayer.GetButtonUp(UILEFT))
        {
            if (leftButton != null && leftButton.gameObject.activeSelf)
            {
                leftButton.ReleaseButtonEvent(() =>
                {
                    selectIndex = (int)OptionLabel.TEXTSPEED;
                    SelectedAnimation(activeLabelArray[selectIndex].gameObject,SELECTSCALE);
                }
                );
            }
        }
        else if (TitleUIManager.uiPlayer.GetButton(CANCEL) && TitleSceneManager.CurrentPhase.GetType() == typeof(Title_Option))
        {
            if (backButton != null)
            {
                operateButtonList[selectIndexHorizon].gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
                backButton.GotoNextPhase();
            }
        }
#endif
    }

    public void ScaleReset()
    {
        for (int i = 0; i < activeLabelArray.Count; i++)
        {
            activeLabelArray[i].gameObject.transform.localScale = Vector3.one;
        }
    }

    public void SelectedAnimation(GameObject target,float targetScale)
    {
        
    }
}
