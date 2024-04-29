using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AreaSelectDialogPanel : AreaSelectPanelBehaviour
{
    [SerializeField]
    protected AreaSelectButtonBehaviour backButton = null;

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
        InitSelect();
    }

    protected void Update()
    {
        if (AreaSelectUIManager.CurrentPanel == this)
        {
            DefaultOperate();
            if (AreaSelectUIManager.uiPlayer.GetButton(CANCEL))
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

    public override AREASELECT TargetScene()
    {
        return AREASELECT.PAUSE;
    }

    #endregion
}
