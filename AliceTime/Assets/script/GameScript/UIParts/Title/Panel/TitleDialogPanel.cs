using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TitleDialogPanel : TitlePanelBehaviour
{
    private EventSystem currentEventSystem;
    private PointerEventData pointer;
    private List<RaycastResult> result;
    private float uiTimer = 0.0f;
    private bool selectedMove = false;

    protected override void Init()
    {
        base.Init();

        currentEventSystem = EventSystem.current;
        pointer = new PointerEventData(currentEventSystem);
        result = new List<RaycastResult>();

        gameObject.SetActive(false);
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

    public void Update()
    {
        /*ボタン操作
        float xValue = TitleUIManager.uiPlayer.GetAxis(ACTION_MOVE_HORIZONTAL);
        if (xValue > BORDER && !selectedMove)
        {
            activeButtonArray[selectIndex].isSelectedActive = false;
            activeButtonArray[selectIndex].SelectedAnimation();
            selectedMove = true;
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = activeButtonArray.Count - 1;
            }
            activeButtonArray[selectIndex].isSelectedActive = true;
            activeButtonArray[selectIndex].SelectedAnimation();
        }
        else if (xValue < -1.0f * BORDER && !selectedMove)
        {
            activeButtonArray[selectIndex].isSelectedActive = false;
            activeButtonArray[selectIndex].SelectedAnimation();
            selectedMove = true;
            selectIndex++;
            if (selectIndex >= activeButtonArray.Count)
            {
                selectIndex = 0;
            }
            activeButtonArray[selectIndex].isSelectedActive = true;
            activeButtonArray[selectIndex].SelectedAnimation();
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

        if (TitleUIManager.uiPlayer.GetButton(CANCEL))
        {
            if (backButton != null)
            {
                backButton.GotoNextPhase();
            }
        }
        */
    }
}
