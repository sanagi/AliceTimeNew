using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FoldAnimationSkipButtonBehavior : OptionButtonBehaviour
{
	public OptionManager.FoldAnimationSkip foldAnimatinSkip;

    public override void SwitchColor()
	{
		if (buttonImage == null) {
			buttonImage = gameObject.GetComponent<Image> ();
		}

		if (OptionManager.Instance.GetFoldAnimationSkip () == foldAnimatinSkip) {
			buttonImage.color = new Color (0.55f, 0.55f, 0.55f, 1.0f);
		} else {
			buttonImage.color = new Color (1f, 1f, 1f, 1f);
		}
	}

	#region IButtonEvent implementation

	public override void ReleaseButtonEvent ()
	{
        if (foldAnimatinSkip != OptionManager.Instance.GetFoldAnimationSkip())
        {
            Audio_Manage.Play(SoundEnum.SE_OK);
        }
        OptionManager.Instance.ChangeFoldAnimationSkip (foldAnimatinSkip);

		var foldAnimationSkipButtonBehaviors = transform.parent.GetComponentsInChildren<FoldAnimationSkipButtonBehavior> ();
		foreach (var foldAnimationSkipButtonBehavior in foldAnimationSkipButtonBehaviors) {
			foldAnimationSkipButtonBehavior.SwitchColor ();
		}
	}

	#endregion
}
