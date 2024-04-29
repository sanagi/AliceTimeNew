using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class AreaSelectMainPanel : AreaSelectPanelBehaviour
{
    private const string RETRY = "Retry";

    private EventSystem currentEventSystem;
    private PointerEventData pointer;
    private List<RaycastResult> result;

    private List<RectTransform> rectButtons = new List<RectTransform>();

    private bool isOverlap;

    [SerializeField]
    private AreaSelectButtonBehaviour menuButton = null;
    [SerializeField]
    private AreaSelectButtonBehaviour retryButton = null;

    private bool isPauseUpAvailable = false;

    protected override void Init()
    {
        base.Init();

        currentEventSystem = EventSystem.current;
        pointer = new PointerEventData(currentEventSystem);
        result = new List<RaycastResult>();

        gameObject.SetActive(false);

        // Instantiate UI's prefab
#if UNITY_IOS || UNITY_ANDROID
        var prefabPauseButton = Resources.Load<GameObject>("Prefabs/UI/Game/Main/PauseButton");
        var pauseButton = Instantiate(prefabPauseButton, transform);
        rectButtons.Add(pauseButton.GetComponent<RectTransform>());

        var prefabRetryButton = Resources.Load<GameObject>("Prefabs/UI/Game/Main/RetryButton");
        var retryButton = Instantiate(prefabRetryButton, transform);
        rectButtons.Add(retryButton.GetComponent<RectTransform>());
#else
        var prefabMenuButton = Resources.Load<GameObject>("Prefabs/UI/Game/Main/MenuButton");
        menuButton = Instantiate(prefabMenuButton, transform).GetComponent<AreaSelectButtonBehaviour>();
        menuButton.GetComponent<Image>().enabled = false;
        
        var prefabRetryButton = Resources.Load<GameObject>("Prefabs/UI/Game/Main/RetrySButton");
        retryButton = Instantiate(prefabRetryButton, transform).GetComponent<AreaSelectButtonBehaviour>();
        retryButton.GetComponent<Image>().enabled = false;
#endif
    }

    void Update()
    {
        //CheckOverlap();
        //TransparentButton();
        //メニューチェック
        CheckMenu();
    }

    void CheckMenu()
    {
        if (GameUIManager.uiPlayer.GetButtonDown(OPENMENU))
        {
            isPauseUpAvailable = true;
        }
        else if (GameUIManager.uiPlayer.GetButtonUp(OPENMENU) && isPauseUpAvailable)
        {
            isPauseUpAvailable = false;
            menuButton.GotoNextPhase();
        }
        else if (GameUIManager.uiPlayer.GetButtonUp(RETRY))
        {
            if (retryButton.gameObject.activeSelf)
            {
                retryButton.GotoNextPhase();
            }
        }
    }

    Vector3 origin;
    Vector3 halfExtents;


    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        if (isOverlap) return null;

        pointer.position = screenPosition;
        currentEventSystem.RaycastAll(pointer, result);

        if (result.Count > 0)
        {
            return result[0].gameObject.GetComponent<IButtonEvent>();
        }
        return null;
    }

#region implemented abstract members of GamePanelBehaviour

    public override AREASELECT TargetScene()
    {
        return AREASELECT.MAIN;
    }

#endregion
}