using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndButtonBehaviour : MonoBehaviour, IButtonEvent
{
    public MAINSCENE TargetSceneManager = MAINSCENE.END;
    public ENDSCENE End_NextPhaseName = ENDSCENE.INIT;

    public SoundEnum SE = SoundEnum.SE_STAGESELECT;

	public Image buttonImage;


	void Start() {
		buttonImage = gameObject.GetComponent<Image>();
	}

	void OnEnable(){
		DefaultImageSet();
	}

    // ボタンのイベント処理
    private void GotoNextPhase()
    {
        if (SE != SoundEnum.SE_NONE)
        {
            Audio_Manage.Play(SE);
        }

        if (TargetSceneManager == MAINSCENE.END)
        {
            switch (End_NextPhaseName)
            {
                case ENDSCENE.FINISH:
                    EndSceneManager.Goto("End_Finish");
                    break;
            }
        }
    }

	private void DefaultImageSet() {
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(1.0f,1.0f,1.0f,1.0f);
	}

    #region IButtonEvent implementation

    public void FireButtonEvent()
	{		
		if(buttonImage == null){
			buttonImage = gameObject.GetComponent<Image>();
		}		
		buttonImage.color = new Color(0.55f,0.55f,0.55f,1.0f);	
    }

	public void ReleaseButtonEvent ()
	{
		GotoNextPhase();
		//DefaultImageSet();
	}

	public void ReleaseOutButtonEvent ()
	{
		DefaultImageSet();
	}

    #endregion
}