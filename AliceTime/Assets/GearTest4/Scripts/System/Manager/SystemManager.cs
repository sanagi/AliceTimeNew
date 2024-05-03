using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;




/*
    SystemManager.Instance
    SystemManager.Instance.ChangeMenuRequest( MenuId.Game);             //メニュー切り替え
 */

//モードの種類
public enum MenuId
{
    None = -1,
    Logo,                       //ロゴ
    Title,                      //タイトル
    Game,                       //ゲーム本編
};
//レイヤーの種類
public enum LayerId
{
    Ignore = -1,				//無視
    Default = 0,                //3D画面など
};


/// <summary>
/// メニュー管理
/// </summary>
public class SystemManager : MonoBehaviour
{
    //外部アクセス用
    public static SystemManager Instance;
    private static int _instanceCount = 0;

    private MenuId _menuIdNow = MenuId.None;                 //現在選択中のメニュー
    [SerializeField]
    private MenuId _menuIdReq = MenuId.None;                 //遷移のリクエスト
    private bool _firstChange = true;

    [SerializeField]
    private Renderer _fadeRenderer = null;

    private Coroutine _changeMenuLog = null;


    /// <summary>
    /// 初期化
    /// </summary>
    public static void Initialize()
    {
        //まだなければ作る
        if (null != Instance)
        {
            return;
        }
        GameObject sysMngObj = Instantiate(Resources.Load("System/__SystemManager")) as GameObject;
    }



    /// <summary>
    /// 起動時
    /// </summary>
    private void Awake()
    {
        _instanceCount++;
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        //シーン切替時に削除しないものに登録
        DontDestroyOnLoad(gameObject);
        Instance = this;
        //通常の再生
        Time.timeScale = 1;
        ChangeMenuRequest(Instance._menuIdReq);
    }
    /// <summary>
    /// オブジェクト破棄時
    /// </summary>
    private void OnDestroy()
    {
        if (--_instanceCount <= 0)
        {
            //インスタンス破棄
            Instance = null;
        }
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
#if UNITY_EDITOR
        //デバッグ機能
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //一時停止を呼ぶ
            Debug.LogError("[BREAK]");
        }
        else
        if (Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 0f;
        }
        else
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 0.2f;
        }
        else
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Time.timeScale = 4f;
        }
        else
        {
            //通常の再生
            Time.timeScale = 1;
        }
#endif
    }
    /// <summary>
    /// メニュー遷移のリクエストを設定
    /// </summary>
    public bool Waiting()
    {
        return (null != _changeMenuLog);
    }
    /// <summary>
    /// メニュー遷移のリクエストを設定
    /// </summary>
    public void ChangeMenuRequest(MenuId reqId)
    {
        if (MenuId.None == reqId)
        {
            return;
        }
        //すでに他にリクエストが来ている？
        if (null != _changeMenuLog)
        {
            //先約があるなら拒否
            Debug.LogError("SystemManager : 多重リクエスト " + _menuIdNow + ", " + _menuIdReq + ", " + reqId);
            return;
        }

        //リクエスト受理
        _changeMenuLog = StartCoroutine(CoChangeMenu(reqId));
    }
    /// <summary>
    /// メニュー遷移を実行
    /// </summary>
    private IEnumerator CoChangeMenu(MenuId reqId)
    {
        _menuIdReq = reqId;

        _fadeRenderer.gameObject.SetActive(true);
        var fadeMat = _fadeRenderer.material;
        float fadeSec = 0f;
        if (!_firstChange)
        {
            do
            {
                fadeSec += Time.deltaTime;
                fadeMat.color = new Color(0, 0, 0, Mathf.Clamp01(fadeSec));
                yield return null;
            } while (fadeSec < 1f);
        }
        _firstChange = false;

        SceneManager.LoadScene("" + reqId.ToString());
        yield return null;

        fadeSec = 1f;
        do
        {
            fadeSec -= Time.deltaTime;
            fadeMat.color = new Color(0, 0, 0, Mathf.Clamp01(fadeSec));
            yield return null;
        } while (fadeSec > 0f);
        _fadeRenderer.gameObject.SetActive(false);

        _menuIdNow = _menuIdReq;
        _changeMenuLog = null;
        yield return null;
    }
}
