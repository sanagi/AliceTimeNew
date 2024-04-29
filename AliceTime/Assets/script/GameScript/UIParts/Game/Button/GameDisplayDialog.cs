using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameDisplayDialog : GameButtonBehaviour {
	public GameObject dialog;

	void Start() {
		buttonImage = gameObject.GetComponent<Image>();
	}

    #region IButtonEvent implementation

    public virtual void Update()
    {
        if (GameUIManager.uiPlayer != null)
        {
            if ((GameUIManager.uiPlayer.GetButtonUp(SUBMIT)) && isSelectedActive)
            {
                ReleaseButtonEvent();
                isSelectedActive = false;
            }
        }

    }

    public override void ReleaseButtonEvent ()
	{
		Audio_Manage.Play(SE);
		dialog.SetActive (true);
		DefaultImageSet();
	}

	#endregion
}
