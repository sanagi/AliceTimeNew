using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveSpeedButtonBehavior : OptionButtonBehaviour
{
    public override void ReleaseButtonEvent()
    {
        if(moveSpeed != OptionManager.Instance.GetMoveSpeed())
        {
            Audio_Manage.Play(SoundEnum.SE_OK);
        }
        OptionManager.Instance.ChangeMoveSpeed(moveSpeed);

        var moveSpeedSkipButtonBehaviors = transform.parent.GetComponentsInChildren<MoveSpeedButtonBehavior>();
        foreach (var moveSpeedSkipButtonBehavior in moveSpeedSkipButtonBehaviors)
        {
            moveSpeedSkipButtonBehavior.SwitchColor();
        }
    }
}
