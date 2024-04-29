using UnityEngine;
using System.Collections;

public class TitleAllClearPanel : TitlePanelBehaviour
{
    private IButtonEvent startButton;

    protected override void Init()
    {
        base.Init();

        startButton = transform.Find("goTitle").GetComponent<IButtonEvent>();

        gameObject.SetActive(false);
    }

    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        return startButton;
    }
}
