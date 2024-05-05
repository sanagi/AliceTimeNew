using UnityEngine;
using System.Collections.Generic;
using Rewired;

// ゲーム中のバーチャルコントローラを除くUIを管理
//Game画面だとSetActiveになる瞬間があるけど直接Worldに行っちゃうと(つづきから)だとSetActiveされずにこのクラスにアクセスされる時があった
//ので初期化されていなかったら初期化(Monobehavior継承いらない気がしたけど時間かかりそう&これでも不具合でなさそうなので放置)
public class GameUIManager : UIManager
{
    public static GamePanelBehaviour CurrentPanel;
    private static List<GamePanelBehaviour> panels;

    IButtonEvent touchedButton;

    public static Player uiPlayer = null;

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

#if UNITY_SWITCH
    private void Update()
    {
        if (uiPlayer == null)
        {
            uiPlayer = ReInput.players.GetPlayer(0);
        }
    }
#endif

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

    public override int MyOrder()
    {
        return 400;
    }
}
