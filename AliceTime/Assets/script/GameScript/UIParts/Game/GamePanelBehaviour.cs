using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class GamePanelBehaviour : UIMonobehaviour
{
    public abstract GAMESCENE TargetScene();

    public List<GameButtonBehaviour> activeButtonArray = new List<GameButtonBehaviour>();
    public const string ACTION_MOVE_HORIZONTAL = "UIHorizontal";
    public const string ACTION_MOVE_VERTICAL = "UIVertical";
    public const string CANCEL = "UICancel";
    public const string OPENMENU = "Open Menu";
    public const string PLUS = "UIPlus";

    protected const float MARGINTIME = 0.15f;
    protected const float BORDER = 0.75f;
    public int selectIndex = 0;

    private float uiTimer = 0.0f;
    private bool selectedMove = false;

    protected override void Init()
    {
        GameUIManager.RegistedPanel(this);
    }

    protected virtual void InitSelect() {
        for(int i = 0; i < activeButtonArray.Count; i++)
        {
            activeButtonArray[i].isSelectedActive = false;
            activeButtonArray[i].SelectedAnimation(0f);
        }
        if (activeButtonArray.Count != 0)
        {
            activeButtonArray[0].isSelectedActive = true;
            activeButtonArray[0].SelectedAnimation();
        }
    }

    protected void DefaultOperate()
    {
        float xValue = GameUIManager.uiPlayer.GetAxis(ACTION_MOVE_HORIZONTAL);
        if (xValue > BORDER && !selectedMove)
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
        else if (xValue < -1.0f * BORDER && !selectedMove)
        {
            activeButtonArray[selectIndex].isSelectedActive = false;
            activeButtonArray[selectIndex].SelectedAnimation();
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = activeButtonArray.Count - 1;
            }
            selectedMove = true;

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
    }

    protected override void Deinit()
    {
        GameUIManager.UnregistedPanel(TargetScene(), this);
    }

    public void Display()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public virtual IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {

        return null;
    }
}
