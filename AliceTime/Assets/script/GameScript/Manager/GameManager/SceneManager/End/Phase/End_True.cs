using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class End_True : PhaseBase
{
    public override string PhaseName
    {
        get
        {
            return this.GetType().FullName;
        }
    }

    public override void OnEnter(PhaseBase prevPhase)
    {
        base.OnEnter(prevPhase);

        //var tipsPanel = EndUIManager.GetPanelObject(ENDSCENE.TRUE);
        //tipsPanel.transform.localScale = Vector3.zero;
        //DisableRaycastEvent(tipsPanel);

        /*EndUIManager.DisplayPanel(ENDSCENE.TRUE);
        
        UIAnimationUtil.Scale(tipsPanel, 0f, 1f, 0.4f, () =>
        {
            EnableRaycastEvent(tipsPanel);
        });
        */
		//SaveManager.Instance.ClearAllStory();
        EndSceneManager.Goto("End_Finish");
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        base.OnExit(nextPhase);
        
        /*var tipsPanel = EndUIManager.GetPanelObject(ENDSCENE.TRUE);
        tipsPanel.transform.localScale = Vector3.zero;
        DisableRaycastEvent(tipsPanel);
        
        UIAnimationUtil.Scale(tipsPanel, 1f, 0f, 0.4f, () =>
        {
            EndUIManager.HidePanel(ENDSCENE.TRUE);           
        });*/
    }
    
    private void DisableRaycastEvent(GameObject obj)
    {
        var children = obj.GetComponentsInChildren<Image>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "goTitle")
            {
                children[i].raycastTarget = false;
                break;
            }
        }
    }

    private void EnableRaycastEvent(GameObject obj)
    {
        var children = obj.GetComponentsInChildren<Image>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "goTitle")
            {
                children[i].raycastTarget = true;
                break;
            }
        }
    }
}