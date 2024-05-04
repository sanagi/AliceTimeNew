using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveSpeedButtonBehavior : OptionButtonBehaviour
{
    public override void ReleaseButtonEvent()
    {
        if(moveSpeed != OptionManager.Instance.GetMoveSpeed())
        {
            SoundManager.Instance.PlaySound(SoundId.System_Decide);
        }
        OptionManager.Instance.ChangeMoveSpeed(moveSpeed);

        var moveSpeedSkipButtonBehaviors = transform.parent.GetComponentsInChildren<MoveSpeedButtonBehavior>();
        foreach (var moveSpeedSkipButtonBehavior in moveSpeedSkipButtonBehaviors)
        {
            moveSpeedSkipButtonBehavior.SwitchColor();
        }
    }
}
