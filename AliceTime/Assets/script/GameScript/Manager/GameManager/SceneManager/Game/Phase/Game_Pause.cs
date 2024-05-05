using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game_Pause : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}
    
    GameObject MapObj;
    GameObject FragObj;

    Sprite m_FGet;
    Sprite m_FUnGet;

    [SerializeField]
    Image FragImage1;
    [SerializeField]
    Image FragImage2;
    [SerializeField]
    Image FragImage3;

    GameObject PauseButton;

    public override void OnEnter (PhaseBase prevPhase)
	{
		
		GameUIManager.DisplayPanel (GAMESCENE.PAUSE); //ポーズ画面UIの表示
        if (MapObj == null)
        {
            MapObj = GameObject.Find("Map").gameObject;
        }

        if (FragObj == null)
        {
            FragObj = GameObject.Find("Fragment").gameObject;
        }

        if(m_FGet == null)
        {
            //フェーズ内でやらないほうがよい
        }

        if(m_FUnGet == null)
        {
        }

        if (FragImage1 == null)
        {
            //ポーズ中のキオクの勾玉の取得数を表すUI
            FragImage1 = FragObj.transform.Find("FragmentImage1").gameObject.GetComponent<Image>();
        }
        if (FragImage2 == null)
        {
            FragImage2 = FragObj.transform.Find("FragmentImage2").gameObject.GetComponent<Image>();
        }
        if (FragImage3 == null)
        {
            FragImage3 = FragObj.transform.Find("FragmentImage3").gameObject.GetComponent<Image>();
        }

        /*
        if (MainGameManager.CurrentStageID % 4 == 0)
        {
            MapObj.SetActive(true);
            if (GameManager.StageID != 0)
            {
                FragObj.SetActive(true);
            }
            else
            {
                FragObj.SetActive(false);
            }
            FSpriteSet(FragImage1, 1);
            FSpriteSet(FragImage2, 2);
            FSpriteSet(FragImage3, 3);

        }
        else
        {
            MapObj.SetActive(false);
            FragObj.SetActive(false);
        }
        
        if(GameManager.GameMode == GAMEMODE.STORY)
        {
            if (PauseButton == null)
            {
                PauseButton = GameObject.Find("Go_SelectButton").gameObject;
            }

            if (GameManager.StageID == 0)
            {
                PauseButton.SetActive(false);
            }
            else
            {
                PauseButton.SetActive(true);
            }
        }
        */

        GameUIManager.uiPlayer.controllers.maps.SetAllMapsEnabled(false);
        GameUIManager.uiPlayer.controllers.maps.SetMapsEnabled(true, GameDefine.SYSTEM_REWIRED);
    }

    public override void OnExit (PhaseBase nextPhase)
	{
        if (nextPhase.PhaseName != "Game_TitleDialog" && nextPhase.PhaseName != "Game_WorldDialog")
        {
            GameUIManager.HidePanel(GAMESCENE.PAUSE); //ポーズ画面UIの非表示
        }
		MapObj.SetActive(false);
		FragObj.SetActive(false);
    }

    //キオクのかけら取得数表示
    void FSpriteSet(Image I, int str)
    {
        /*if (GameManager.GameMode == GAMEMODE.TRIAL)
        {
            I.enabled = false;
        }

        else {
            if (GameManager.StageID == 0)
            {
                I.enabled = false;
            }
            else {
                if (SaveManager.Instance.Check_GotFragment(GameManager.StageID, str))
                {
                    I.sprite = m_FGet;
                }
                else {
                    I.sprite = m_FUnGet;
                }
            }
        }*/

    }

}
