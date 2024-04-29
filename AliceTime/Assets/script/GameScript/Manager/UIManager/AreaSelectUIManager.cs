
using UnityEngine;
using System.Collections.Generic;
using KamioriInput;
using Rewired;


public class AreaSelectUIManager : UIManager
{
    public static AreaSelectPanelBehaviour CurrentPanel;
    private static List<AreaSelectPanelBehaviour> panels;

    IButtonEvent touchedButton;

    public static Player uiPlayer = null;

    public static void RegistedPanel(AreaSelectPanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
        panels.Add(panel);
    }

    public static void UnregistedPanel(AREASELECT targetScene, AreaSelectPanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
        panels.Remove(panels.Find(p => p.TargetScene() == targetScene));
    }

    void Awake()
    {
        CurrentPanel = null;
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
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
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
    }

#if UNITY_SWITCH
    private void Update()
    {
        if (uiPlayer == null)
        {
            uiPlayer = ReInput.players.GetPlayer(0);
        }
    }
#endif

    public static void DisplayPanel(AREASELECT scene)
    {
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
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

    public static void HidePanel(AREASELECT scene)
    {
        if (panels == null)
        {
            panels = new List<AreaSelectPanelBehaviour>();
        }
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
