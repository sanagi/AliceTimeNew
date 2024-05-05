using UnityEngine;
using System.Collections.Generic;
using KamioriInput;
using Rewired;
using UnityEngine.UI;

// ゲーム中のバーチャルコントローラを除くUIを管理
//Game画面だとSetActiveになる瞬間があるけど直接Worldに行っちゃうと(つづきから)だとSetActiveされずにこのクラスにアクセスされる時があった
//ので初期化されていなかったら初期化(Monobehavior継承いらない気がしたけど時間かかりそう&これでも不具合でなさそうなので放置)
public class ExploreUIManager : UIManager
{
    public static ExplorePanelBehaviour CurrentPanel;
    private static List<ExplorePanelBehaviour> panels;

    IButtonEvent touchedButton;

    public static Player uiPlayer = null;
    
    private RawImage _3dImage;

    public static void RegistedPanel(ExplorePanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<ExplorePanelBehaviour>();
        }
        panels.Add(panel);
    }

    public static void UnregistedPanel(EXPLORESCENE targetScene, ExplorePanelBehaviour panel)
    {
        if (panels == null)
        {
            panels = new List<ExplorePanelBehaviour>();
        }
        panels.Remove(panels.Find(p => p.TargetScene() == targetScene));
    }

    void Awake()
    {
        CurrentPanel = null;
        if (panels == null)
        {
            panels = new List<ExplorePanelBehaviour>();
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
            panels = new List<ExplorePanelBehaviour>();
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

    public static void DisplayPanel(EXPLORESCENE scene)
    {
        if (panels == null)
        {
            panels = new List<ExplorePanelBehaviour>();
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

    public static void HidePanel(EXPLORESCENE scene)
    {
        if (panels == null)
        {
            panels = new List<ExplorePanelBehaviour>();
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
