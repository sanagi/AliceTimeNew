using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*** エンディングの種類 ***/
public enum ENDMODE
{
    TRUE, NORMAL, BAD
}

/*** エンディングを管理するマネージャ ***/
public class EndManager : SingletonMonoBehaviour<EndManager>
{

    /****************
     * Inner Cache
     ****************/
    private EndSceneManager sceneManager;
    private EndUIManager uiManager;
    private EndAudioController audioController;

    public Text badText;
    public Text normalText;

    public EndingAnimationManager endingController;

    public static ENDMODE EndMode
    {
        get
        {
            return ENDMODE.NORMAL;
        }
    }

    void Awake()
    {
        sceneManager = (EndSceneManager)FindObjectOfType<EndSceneManager>();
        audioController = (EndAudioController)FindObjectOfType<EndAudioController>();
        uiManager = (EndUIManager)FindObjectOfType<EndUIManager>();
    }

    void Start()
    {
        StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        // シーンに必要なものが揃っているかのチェック
        var applicationManager = (ApplicationManager)FindObjectOfType<ApplicationManager>();
        if (applicationManager == null)
            (new GameObject("MainManager")).AddComponent<ApplicationManager>();

        if (sceneManager == null)
        {
            sceneManager = (new GameObject("SceneManager")).AddComponent<EndSceneManager>();
            sceneManager.transform.SetParent(transform);
        }
        if (audioController == null)
        {
            audioController = (new GameObject("AudioController")).AddComponent<EndAudioController>();
            audioController.transform.SetParent(transform);
        }
        if (uiManager == null)
        {
            uiManager = (new GameObject("UIManager")).AddComponent<EndUIManager>();
            uiManager.transform.SetParent(transform);
        }

        sceneManager.Initialization();
        uiManager.Initialization();

        EndSceneManager.Goto("End_Init");

        yield break;
    }

    void OnDestroy()
    {
        sceneManager.Finalization();
        uiManager.Finalization();

        sceneManager = null;
        audioController = null;
        uiManager = null;
    }
}