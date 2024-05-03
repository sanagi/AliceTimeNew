using UnityEngine;
using System.Collections;
using Cinemachine;
using Cinemachine.Editor;
using DG.Tweening;

public class Game_Init : PhaseBase
{
    public override string PhaseName
    {
        get
        {
            return this.GetType().FullName;
        }
    }

    public override void OnEnter(PhaseBase prevPhase)
    {
        // シーンが開始されたタイミングで不要なオブジェクトが存在していないかの確認

        //ステージ生成(プレイヤー含む)
        //var currentGameMode = GameManager.GameMode;
        //var currentStageId = GameManager.StageID;
        //var currentAreaId = GameManager.AreaID;
        //canvas.layer = LayerMask.NameToLayer("UI");
        //canvas.GetComponent<CanvasInitializer>().SetRenderMode(RenderMode.ScreenSpaceOverlay);
        
        //必要なBGMの読み込み
        ReadAudio();
        
        //カメラ設定
        CameraManager.Instance.CrateMainGameGearCamera();

        //AibouManager.MoveToPosition(GameManager.RespawnPosition + Vector3.up, 0);
        //Camera_Move_Dangion.Instance.PlayerSet();
        //Camera_Move_Dangion.Instance.PosSetP();
        //Camera_Move_Dangion.Instance.PlayerCamPosSet();

        //コントローラーはつけるがまだ動かさない
        PlayerManager.Instance.EnablePhysics();
        PlayerManager.Instance.DisableControllable();
        
        // 生成情報からGameManagerを構築してゲームを開始
        MainGameManager.SetPlayStage(GameDefine.DEBUG_STAGE);
        
        //フェードインしてゲーム開始
        CameraManager.Instance.StartCoroutine(CameraManager.Instance.FadeIn(() =>
        {
            //SaveManager.Instance.NowPlayingStage = GameManager.StageID;
            
            //イベントシーンが始まる条件などがあればここで記述
            
            GameSceneManager.Goto(GameDefine.GAME_START);
            
        }));
    }

    private void ReadAudio()
    {
        //BGMを読み込んでおく
        //Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_WORLD));
        //Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KAISOU));
        /*switch (GameManager.StageID)
        {

            case 0:
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_CHINATSU));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_SHRINE));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_FUON));
                break;

            case 4:
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_SETSUNA));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_FUON));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KINPAKU));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_FUAN));
                break;

            case 8:
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_FUAN));
                break;

            case 12:
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_FUAN));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_ORIGAMI));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KINPAKU));
                break;

            case 17:
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KENEN));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_IMPACT));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KANASHIMI));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KETSUI));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_YASASHISA));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_ORIGAMI));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_TRUE));
                Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_BADEND));
                break;
        }
        

        if (0 < GameManager.FragmentNumProp && GameManager.FragmentNumProp < 4)
        {
            Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_KANASHIMI));
        }
        if (1 < GameManager.FragmentNumProp && GameManager.FragmentNumProp < 5)
        {
            Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_TSUMARAN));
        }
        if (6 < GameManager.FragmentNumProp && GameManager.FragmentNumProp < 10)
        {
            Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_BOURYOKU));
        }
        if (8 < GameManager.FragmentNumProp && GameManager.FragmentNumProp < 12)
        {
            Audio_Manage.eventAudioList.Add(Audio_Manage.GetBGM(SoundEnum.BGM_ORIGAMI));
        }
        */

        /*if(GameManager.StageID == 6 || GameManager.StageID == 7 || GameManager.StageID == 8){
                        Audio_Manage.Play(SoundEnum.SE_WATERLOOP);
                    }
                    if(GameManager.StageID == 14 || GameManager.StageID == 15 || GameManager.StageID == 16){
                        Audio_Manage.Play(SoundEnum.SE_WINDLOOP);
                    }*/
    }

    public override void OnExit(PhaseBase nextPhase)
    {
        
    }
}
