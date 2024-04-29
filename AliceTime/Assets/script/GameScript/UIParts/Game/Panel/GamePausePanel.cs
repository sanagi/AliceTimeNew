using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GamePausePanel : GamePanelBehaviour
{
    [SerializeField]
    protected GameButtonBehaviour backButton = null;

    private EventSystem currentEventSystem;
    private PointerEventData pointer;
    private List<RaycastResult> result;
    private GameObject goToWorldButton;

    //public PauseMap Map;

    protected override void Init()
    {
        base.Init();

        currentEventSystem = EventSystem.current;
        pointer = new PointerEventData(currentEventSystem);
        result = new List<RaycastResult>();

        gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        if (goToWorldButton == null)
        {
            goToWorldButton = GameObject.Find("Go_SelectButton").gameObject;
            if (goToWorldButton == null)
            {
                return;
            }
        }

        /*if (GameManager.GameMode == GAMEMODE.TRIAL)
        {
            goToWorldButton.SetActive(false);
        }
        else
        {
            goToWorldButton.SetActive(true);
        }*/
        //リスト更新
        for (int i = activeButtonArray.Count - 1; i >= 0; i--)
        {
            selectIndex = 0;
            activeButtonArray[i].isSelectedActive = false;
            activeButtonArray[i].SelectedAnimation(0f);
            if (!activeButtonArray[i].gameObject.activeSelf)
            {
                activeButtonArray.Remove(activeButtonArray[i]);
            }
        }
        InitSelect();
    }

    private void Update()
    {
        if (GameUIManager.CurrentPanel == this)
        {
            DefaultOperate();
            if (GameUIManager.uiPlayer.GetButton(CANCEL) || GameUIManager.uiPlayer.GetButton(PLUS))
            {
                if (backButton != null)
                {
                    backButton.GotoNextPhase();
                }
            }
        }
    }

    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
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

    #region implemented abstract members of GamePanelBehaviour

    public override GAMESCENE TargetScene()
    {
        return GAMESCENE.PAUSE;
    }

    #endregion
}
