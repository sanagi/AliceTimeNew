using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;

public class LoadManager : SingletonMonoBehaviour<LoadManager>
{
    [SerializeField]
    private Canvas _loadingCanvas; // ロード画面の親Canvas

    [SerializeField] 
    private GameObject _loadPanel;//読み込み中を出すパネル
    
    [SerializeField]
    private Text _loadingMessage;      // 読み込み中の文言を表示するText UI

    private bool isError;           // エラーが発生した場合のフラグ
    private bool _isTransition;    // 同時に複数のシーン遷移を発生させないためのフラグ

    /// <summary>
    /// 初期化処理
    /// ※ MainManagerが初めて参照された時に実行される。つまりawake,start以降に呼ばれる可能性あり
    /// </summary>
    protected override void Init() {
        DontDestroyOnLoad(gameObject);

        if (_loadingCanvas == null || _loadingMessage == null) {
            Debug.LogError("ロード画面の表示に必要なUIが揃っていません");

            isError = true;
            if (_loadingCanvas != null)
                _loadingCanvas.gameObject.SetActive(false);
            if (_loadingMessage != null)
                _loadingMessage.gameObject.SetActive(false);
        }
        _loadingCanvas.worldCamera = CameraManager.Instance.GetUiCamera();
        ResetState();
    }

    /// <summary>
    /// 内部ステータスをリセット
    /// </summary>
    private void ResetState() {
        _isTransition = false;

        if (isError) return;

        //m_loadingMessage.text = "よみこみちゅう";
    }

    /// <summary>
    /// ロードパネルを表示
    /// </summary>
	public void ShowLoadingMessage() {
        if (isError) return;

        _loadPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ロードパネルを非表示に
    /// </summary>
	public void HideLoadingMessage() {
        if (isError) return;
        _loadPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// シーン遷移メソッド
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <param name="mode"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void Transition(string nextSceneName, LoadSceneMode mode, Action callback = null) {
        if (_isTransition) return;
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
        _isTransition = true;

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
            HideLoadingMessage();
            callback();
        }

        _isTransition = false;
    }
}