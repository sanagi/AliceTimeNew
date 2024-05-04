using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AreaSelectDisplayDialog : AreaSelectButtonBehaviour {
	public GameObject dialog;

	void Start() {
		buttonImage = gameObject.GetComponent<Image>();
	}

    #region IButtonEvent implementation

    public virtual void Update()
    {
        if (AreaSelectUIManager.uiPlayer != null)
        {
            if ((AreaSelectUIManager.uiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
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
