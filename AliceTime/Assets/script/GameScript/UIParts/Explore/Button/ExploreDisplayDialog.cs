using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExploreDisplayDialog : ExploreButtonBehaviour {
	public GameObject dialog;

	void Start() {
		buttonImage = gameObject.GetComponent<Image>();
	}

    #region IButtonEvent implementation

    public virtual void Update()
    {
        if (ExploreUIManager.uiPlayer != null)
        {
            if ((ExploreUIManager.uiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
            {
                ReleaseButtonEvent();
                isSelectedActive = false;
            }
        }

    }

    public override void ReleaseButtonEvent ()
	{
		SoundManager.Instance.PlaySound(SE);
		dialog.SetActive (true);
		DefaultImageSet();
	}

	#endregion
}
