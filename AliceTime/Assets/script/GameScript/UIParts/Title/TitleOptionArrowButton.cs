using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleOptionArrowButton : MonoBehaviour, IButtonEvent
{
	public TitleOptionSelectPanel titleOptionSelectPanel;

	public bool isRight = false;
	public bool isLeft = false;
	public Image buttonImage;

	void Start()
	{
		buttonImage = gameObject.GetComponent<Image>();
	}

	public void FireButtonEvent()
	{
		if(!TitleManager.Instance.openAnimation && !TitleManager.Instance.trialNextOpen){
			if(buttonImage == null){
				buttonImage = gameObject.GetComponent<Image>();
			}			
			buttonImage.color = new Color(0.55f,0.55f,0.55f,1.0f);
		}
	}

	public void ReleaseButtonEvent ()
	{
		if(!TitleManager.Instance.openAnimation && !TitleManager.Instance.trialNextOpen){
			if (isRight)
			{
				titleOptionSelectPanel.NextPage ();
			}

			if (isLeft)
			{
				titleOptionSelectPanel.PrevPage ();
			}
			DefaultImageSet();
		}
	}

    public void ReleaseButtonEvent(System.Action callbak)
    {
        if (!TitleManager.Instance.openAnimation && !TitleManager.Instance.trialNextOpen)
        {
            if (isRight)
            {
                titleOptionSelectPanel.NextPage();
            }

            if (isLeft)
            {
                titleOptionSelectPanel.PrevPage();
            }
            DefaultImageSet();
        }
        if (callbak != null)
        {
            callbak();
        }
    }

    public void ReleaseOutButtonEvent ()
	{
		if(!TitleManager.Instance.openAnimation && !TitleManager.Instance.trialNextOpen){		
			DefaultImageSet();
		}
	}

	public void PressImageSet(){
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}			
		buttonImage.color = new Color(0.55f,0.55f,0.55f,1.0f);	
	}

	public void DefaultImageSet(){
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(1.0f,1.0f,1.0f,1.0f);	
	}

}
