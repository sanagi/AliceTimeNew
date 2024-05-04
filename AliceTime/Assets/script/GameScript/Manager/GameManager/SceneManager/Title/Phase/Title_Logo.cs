using UnityEngine;
using System;
using System.Collections;

public class Title_Logo : PhaseBase
{
	private const float FADE_TIME = 0.4f;

    protected TitlePanelBehaviour panel;

    public override string PhaseName
    {
        get
        {
            return this.GetType().FullName;
        }
    }

    public override void OnEnter(PhaseBase prevPhase)
    {
        var canvas = GameObject.Find(GameDefine.FRONT_CANVAS);
        if (canvas == null)
        {
            Debug.Log("\"FrontCanvas\" is missing");
            return;
        }

        var objPanel = canvas.transform.Find(GameDefine.LOGO_PANEL);
        if (objPanel == null)
        {
            Debug.Log("\"Start_Panel\" is missing");
            return;
        }

        // Start関数の関係で0.0001秒(1フレーム)待機しないと正常に動作しない
	    UIAnimationUtil.Wait(0.0001f, () =>
	    {
		    panel = objPanel.GetComponent<TitlePanelBehaviour>();
		    panel.Enable();
	   
		    var firstPanel = panel.transform.Find(GameDefine.TEAM_LOGO_OBJ).gameObject;

		    ShowLogo(firstPanel, () =>
		    {
			    LoadManager.Instance.StartCoroutine(LoadManager.Instance.FadeIn(() =>
			    {
				    UIAnimationUtil.Wait(1f, () =>
				    {
					    HideLogo(firstPanel, () =>
					    {
						    UIAnimationUtil.Wait(0.2f, () =>
						    {
							    LoadManager.Instance.StartCoroutine(LoadManager.Instance.FadeOut(() => {
                                    //TouchEffectManger.Instance.dragEffect = true;
                                    TitleManager.Instance.BackGround.SetActive(true);
                                    TitleSceneManager.Goto(GameDefine.TITLE_START);
                                }, FADE_TIME));
						    });
					    }, FADE_TIME);
				    });

			    }, FADE_TIME));
		    });		    
	    });
	}

	public void ShowLogo(GameObject obj, Action callback, float animationTime=0)
	{
		obj.SetActive(true);
		UIAnimationUtil.FadeIn(obj, animationTime, callback);
	}

	public void HideLogo(GameObject obj, Action callback, float animationTime=0)
	{
		UIAnimationUtil.FadeOut(obj, animationTime, () =>
		{
			callback();
			obj.SetActive(false);
		});
	}

    public override void OnExit(PhaseBase nextPhase)
    {
        if (panel == null) return;
	    
	    /*
	    if (nextPhase.GetType () == typeof(Title_Dialog)) {
		    TitleUIManager.DisableInput ();
	    } else {
		    panel.Disable ();
	    }
	    */
        panel.Disable();
    }
}
