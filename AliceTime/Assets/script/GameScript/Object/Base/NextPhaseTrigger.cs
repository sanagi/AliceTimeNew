using System;
using System.Security.Cryptography.X509Certificates;
using R3;
using R3.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 次に進むオブジェクト
/// </summary>
public class NextPhaseTrigger : MonoBehaviour
{
    [SerializeField]
    private MAINSCENE _targetScene = MAINSCENE.AREASELECT;
    [SerializeField]
    private string _nextStageId = "0";
    
    [SerializeField]
    private string displayNextId = String.Empty;

    [SerializeField]
    private Text _displayText;

    [SerializeField]
    private Canvas _displayUICanvas;
    void Start()
    {
        _displayUICanvas.worldCamera = CameraManager.Instance.MainCamera;
        _displayText.text = displayNextId;
        
        // foward(z軸)の方を向けることで文字が反転するのを修正
        _displayText.transform.localScale = new Vector3(-1, 1, 1);
        
        { //TriggerEnter
            var OnTriggerEnterNext = this.OnTriggerEnterAsObservable().Select(collision => collision.gameObject).Where(_ => _.tag == GameDefine.PlayerTag);
            OnTriggerEnterNext.Subscribe(_ =>
            {
                switch (MainSceneManager.CurrentPhase.PhaseName)
                {
                    case GameDefine.AreaSelect:
                        AreaSelectSceneManager.NextID = _nextStageId;
                        AreaSelectSceneManager.NextMainScene = _targetScene;
                        AreaSelectSceneManager.Goto(GameDefine.AREASELECT_NEXT);                        
                        break;
                    case GameDefine.Explore:
                        ExploreSceneManager.NextID = _nextStageId;
                        ExploreSceneManager.NextMainScene = _targetScene;
                        ExploreSceneManager.Goto(GameDefine.EXPLORE_NEXT);                             
                        break;
                }
                Destroy(this); //2回反応しないようにする
            });
        }
    }
}
