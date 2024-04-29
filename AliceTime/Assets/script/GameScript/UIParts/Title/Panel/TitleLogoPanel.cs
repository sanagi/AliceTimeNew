using UnityEngine;

public class TitleLogoPanel : TitlePanelBehaviour
{
    protected override void Init()
    {
        base.Init();

        gameObject.SetActive(false);
    }

    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        return null;
    }
}
