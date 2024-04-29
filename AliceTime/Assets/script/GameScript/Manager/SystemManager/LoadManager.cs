using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;

public class LoadManager : SingletonMonoBehaviour<LoadManager>
{
    public static float FADEIN_TIME = 0.2f;
    public static float FADEOUT_TIME = 0.2f;


    [SerializeField]
    private Canvas m_loadingCanvas; // ロード画面の親Canvas
    [SerializeField]
    private Text m_loadingMessage;      // 読み込み中の文言を表示するText UI
    
    private bool isError;           // エラーが発生した場合のフラグ
    private bool m_isTransition;    // 同時に複数のシーン遷移を発生させないためのフラグ

    /// <summary>
    /// 初期化処理
    /// ※ MainManagerが初めて参照された時に実行される。つまりawake,start以降に呼ばれる可能性あり
    /// </summary>
    protected override void Init() {
        DontDestroyOnLoad(gameObject);

        if (m_loadingCanvas == null || m_loadingMessage == null) {
            Debug.LogError("ロード画面の表示に必要なUIが揃っていません");

            isError = true;
            if (m_loadingCanvas != null)
                m_loadingCanvas.gameObject.SetActive(false);
            if (m_loadingMessage != null)
                m_loadingMessage.gameObject.SetActive(false);
        }
        m_loadingCanvas.worldCamera = CameraManager.Instance.SubCamera;
        ResetState();
    }

    /// <summary>
    /// 内部ステータスをリセット
    /// </summary>
    private void ResetState() {
        m_isTransition = false;

        if (isError) return;

        //m_loadingMessage.text = "よみこみちゅう";
    }

    /// <summary>
    /// ロード画面を表示
    /// </summary>
	public void ShowLoadingMessage() {
        if (isError) return;

        m_loadingCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// ロード画面を非表示に
    /// </summary>
	public void HideLoadingMessage() {
        if (isError) return;
		m_loadingCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// シーン遷移メソッド
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <param name="mode"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void Transition(string nextSceneName, LoadSceneMode mode, Action callback = null) {
        if (m_isTransition) return;
        ShowLoadingMessage();
        StartCoroutine(AsyncTransition(nextSceneName, mode, callback));
    }

    /// <summary>
    /// シーン遷移のための画面効果用ルーチン
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <param name="mode"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
	IEnumerator AsyncTransition(string nextSceneName, LoadSceneMode mode, Action callback) {
        m_isTransition = true;

        var request = SceneManager.LoadSceneAsync(nextSceneName, mode);
        request.allowSceneActivation = false;   //読み込み後すぐ展開されないように

        do {
            yield return new WaitForEndOfFrame();
        } while (request.progress < 0.9f);

        request.allowSceneActivation = true;    //展開

        if (mode == LoadSceneMode.Additive) {
            var scene = Array.Find<Scene>(SceneManager.GetAllScenes(), (s) => s.name == nextSceneName);
            SceneManager.SetActiveScene(scene);
        }

        if (callback != null) {
            //HideLoadingMessage();
            callback();
        }

        m_isTransition = false;
    }
}