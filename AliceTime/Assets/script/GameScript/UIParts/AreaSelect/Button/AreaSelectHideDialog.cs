using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AreaSelectHideDialog : MonoBehaviour, IButtonEvent {
	public GameObject dialog;

	public Image buttonImage;

	void Start() {
		buttonImage = gameObject.GetComponent<Image>();
	}

	#region IButtonEvent implementation
	public void FireButtonEvent ()
	{
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(0.55f,0.55f,0.55f,1.0f);
	}

	public void ReleaseButtonEvent ()
	{
		dialog.SetActive (false);
		Audio_Manage.Play(SoundEnum.SE_CANCEL);
		/*switch(dialog.name){
		case "Go_TitleDialog":
			
			break;
		case "Go_SelectDialog":
			break;
		}*/
		DeafaultImageSet();
	}

	private void DeafaultImageSet(){
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(1.0f,1.0f,1.0f,1.0f);	
	}

	public void ReleaseOutButtonEvent ()
	{
		DeafaultImageSet();
	}

	#endregion
}
