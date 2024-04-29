using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KamioriInput;


// ゲーム中のバーチャルコントローラを除くUIを管理
public class EndUIManager : UIManager
{
    public static EndPanelBehaviour CurrentPanel;
    private static List<EndPanelBehaviour> panels;

	IButtonEvent touchedButton;
    private ParticleSystem effectTouch;
    private ParticleSystem TouchEffect
    {
        get
        {
            if (effectTouch != null)
            {
                return effectTouch;
            }

            var effect = Resources.Load<GameObject>("Input/TouchEffect") as GameObject;
            var effectObj = Instantiate(effect) as GameObject;
            effectTouch = effectObj.GetComponent<ParticleSystem>();
            effectTouch.loop = false;
            return effectTouch;
        }
    }

    private ParticleSystem effectDrag;
    private ParticleSystem DragEffect
    {
        get
        {
            if (effectDrag != null)
            {
                return effectDrag;
            }

            var effect = Resources.Load<GameObject>("Input/DragEffect") as GameObject;
            var effectObj = Instantiate(effect) as GameObject;
            effectDrag = effectObj.GetComponent<ParticleSystem>();
            effectDrag.loop = false;
            return effectDrag;
        }
    }

    public static void RegistedPanel(EndPanelBehaviour panel)
    {
        panels.Add(panel);
    }

    public static void UnregistedPanel(ENDSCENE targetScene, EndPanelBehaviour panel)
    {
        panels.Remove(panels.Find(p => p.TargetScene() == targetScene));
    }

    void Awake()
    {
        CurrentPanel = null;
        panels = new List<EndPanelBehaviour>();
    }

    void Start()
    {
        foreach (var panel in FindObjectsOfType<Transform>())
        {
            if (!panel.gameObject.activeSelf)
            {
                Debug.LogError(panel + "is not active");
            }
        }
    }

    public static void DisplayPanel(ENDSCENE scene)
    {
        var panel = panels.Find(p => p.TargetScene() == scene);
        if (panel == null)
        {
            Debug.Log("Not found " + scene + "'s panel");
            return;
        }
        
        CurrentPanel = panel;
        CurrentPanel.Display();
        EnableInput();
    }

    public static void HidePanel(ENDSCENE scene)
    {
        var panel = panels.Find(p => p.TargetScene() == scene);
        if (panel == null)
        {
            Debug.Log("Not found " + scene + "'s panel");
            return;
        }
        if (CurrentPanel != null)
        {
            CurrentPanel.Hide();
            CurrentPanel = null;
        }
        DisableInput();
    }

    public static GameObject GetPanelObject(ENDSCENE scene)
    {
        var panel = panels.Find(p => p.TargetScene() == scene);
        if (panel == null)
        {
            Debug.Log("Not found " + scene + "'s panel");
            return null;
        }
        return panel.gameObject;
    }


    #region implemented abstract members of UIManager
    public override void DoCrossKeyEvent(KeyInfo info) { }

    public override void DoJumpKey(KeyInfo info) { }

    public override bool DoTouchBegan(TouchInfo[] info)
    {
        return false;
    }

    public override bool DoTouchMoved(TouchInfo[] info)
    {
        return false;
    }

    public override bool DoTouchEnded(TouchInfo[] info)
    {
        return false;
    }
    #endregion

    public override int MyOrder()
    {
        return 400;
    }
}
