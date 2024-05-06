using UnityEngine;
using System.Collections.Generic;
using Rewired;
using UnityEngine.UI;

public class GameUIManager : UIManager
{
    public static GamePanelBehaviour CurrentPanel;
    private static List<GamePanelBehaviour> panels;

    IButtonEvent touchedButton;

    public static Player uiPlayer = null;
    
    private RawImage _3dImage;

    public static void RegistedPanel(GamePanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<GamePanelBehaviour>();
        }
        panels.Add(panel);
    }

    public static void UnregistedPanel(GAMESCENE targetScene, GamePanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<GamePanelBehaviour>();
        }
        panels.Remove(panels.Find(p => p.TargetScene() == targetScene));
    }

    void Awake()
    {
        CurrentPanel = null;
        if (panels == null)
        {
            panels = new List<GamePanelBehaviour>();
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
            panels = new List<GamePanelBehaviour>();
        }
    }
    
    private void Update()
    {
    }

    public static void DisplayPanel(GAMESCENE scene)
    {
        if (panels == null)
        {
            panels = new List<GamePanelBehaviour>();
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

    public static void HidePanel(GAMESCENE scene)
    {
        if (panels == null)
        {
            panels = new List<GamePanelBehaviour>();
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

    public void Set3DRawImage(RawImage rawImage)
    {
        _3dImage = rawImage;
        Set3DScale(_3dImage);
    }
    
    public override int MyOrder()
    {
        return 400;
    }
}
