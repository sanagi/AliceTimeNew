﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LanguageTypeButtonBehaviour : OptionButtonBehaviour
{
	public OptionManager.LanguageType language;

	void OnEnable() {
        this.enabled = true;
		buttonImage = gameObject.GetComponent<Image>();
        //同じ言語ボタンを押せない
        if(OptionManager.Instance.GetLanguateType() == language)
        {
            if (buttonImage == null)
            {
                buttonImage = gameObject.GetComponent<Image>();
            }
            buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
        }
        else
        {
            DefaultImageSet();
        }
	}
    #region IButtonEvent implementation
    public override void FireButtonEvent()
    {
        if (language != OptionManager.Instance.GetLanguateType())
        {
            if (buttonImage == null)
            {
                buttonImage = gameObject.GetComponent<Image>();
            }
            buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
        }
    }
	public override void ReleaseButtonEvent ()
	{
        if (language != OptionManager.Instance.GetLanguateType())
        {
            //言語設定を変えて再起動
            OptionManager.Instance.ChangeLanguage(language);
            Audio_Manage.Play(SoundEnum.SE_STAGESELECT);
            CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeOut(() =>
            {
                Audio_Manage.StopBGM();
                MainSceneManager.Goto("Title");
            }));
        }
	}

	public override void ReleaseOutButtonEvent ()
	{
        if (language != OptionManager.Instance.GetLanguateType())
        {
            DefaultImageSet();
        }
	}

	private void DefaultImageSet(){
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(1.0f,1.0f,1.0f,1.0f);	
	}

	#endregion
}
