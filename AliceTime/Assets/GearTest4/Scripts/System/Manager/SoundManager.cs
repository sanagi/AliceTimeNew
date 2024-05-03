using UnityEngine;
using System.Collections;


/*
    SoundManager.Instance.PlayMusic( MusicId.Hogehoge);           //BGM 再生
    SoundManager.Instance.StopMusic();	                          //BGM 停止

    SoundManager.Instance.PlaySound( SoundId.Hogehoge);           //効果音 再生
*/
//音楽の種類
public enum MusicId
{
    None = -1,
    Title,          //タイトル画面
    Main,           //本編
    EnumMax
}
//効果音の種類
public enum SoundId
{
    None = -1,
    System_Decide,              //決定音
    System_Cancel,              //キャンセル音
    System_NG,                  //禁止操作

    EnumMax
}







/// <summary>
/// サウンド管理
/// </summary>
public class SoundManager : CommonManagerBase
{
    public static MusicId GetMusicId(AudioClip clip)
    {
        return GetMusicId(clip.name);
    }
    public static MusicId GetMusicId(string name)
    {
        MusicId id;
        for (int i = 0; i < (int)MusicId.EnumMax; i++)
        {
            id = (MusicId)i;
            if (name == id.ToString())
            {
                return id;
            }
        }
        return MusicId.None;
    }



    //外部アクセス用
    public static SoundManager Instance;
    private static int _instanceCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        _instanceCount++;
        if (Instance)
        {
            return;
        }
        //インスタンス設定
        Instance = this;

        //BGM再生用のコンポーネント取得
        _musicSource = gameObject.GetComponent<AudioSource>();
        //音量
        _volume_Music =
        _volume_Sound = _volume_Default;

        //サウンドファイルのロード
        {
            //BGM
            _audioClip_Music = new AudioClip[(int)MusicId.EnumMax];
            for (int i = 0; i < _audioClip_Music.Length; i++)
            {
                _audioClip_Music[i] = Resources.Load("System/Sounds/Music/BGM_" + (MusicId)i + "_Loop") as AudioClip;
            }
        }
        {
            //効果音
            _audioClip_Sound = new AudioClip[(int)SoundId.EnumMax];
            for (int i = 0; i < _audioClip_Sound.Length; i++)
            {
                _audioClip_Sound[i] = Resources.Load("System/Sounds/Sound/" + (SoundId)i) as AudioClip;
            }
        }
    }
    /// <summary>
    /// 破棄時
    /// </summary>
    private void OnDisable()
    {
        if (--_instanceCount <= 0)
        {
            //インスタンス破棄
            Instance = null;
        }
    }



    private static readonly float _volume_Default = 0.5f;       //音量	初期値
    private float _volume_Music = 0.0f;                         //音量	BGM
    private float _volume_Sound = 0.5f;                         //音量	効果音



    private AudioSource _musicSource = null;                    //BGM再生用のAudioSource
    private AudioClip[] _audioClip_Music = null;                //音楽
    private AudioClip[] _audioClip_Sound = null;                //効果音


    private MusicId _musicId_Log = MusicId.None;          //最近再生したBGMの種類



    /// <summary>
    /// 音楽を再生する
    /// </summary>
    public void PlayMusic(MusicId id, float volume = 1f)
    {

        //前と同じ？
        if (_musicId_Log == id)
        {
            //無視
            return;
        }
        //前のBGMを停止
        StopMusic();
        //履歴を更新
        _musicId_Log = id;

        //BGMの番号が 指定なし
        if (MusicId.None == id)
        {
            //ここで帰る
            return;
        }

        //クリップ指定
        _musicSource.clip = _audioClip_Music[(int)id];
        //ループなし
        _musicSource.loop = true;
        //音量
        _musicSource.volume = (_volume_Music * volume);
        //再生開始
        _musicSource.Play();
    }
    /// <summary>
    /// 音楽を停止する
    /// </summary>
    public void StopMusic()
    {
        //履歴を更新
        _musicId_Log = MusicId.None;
        //停止
        _musicSource.Stop();
    }




    /// <summary>
    /// 効果音を再生する(簡易版)
    /// </summary>
    public void PlaySound(SoundId enumId, float volume = 1f)
    {
        //鳴らさない音声？
        if (SoundId.None == enumId)
        {
            //無視
            return;
        }

        //この番号の音声はない
        int id = (int)enumId;
        if ((id < 0) || (_audioClip_Sound.Length <= id))
        {
            //読み込み失敗
            Debug.LogError("SoundId : id error (" + enumId + ", " + id + " / " + _audioClip_Sound.Length + ")");
            return;
        }
        //この音声データはない
        if (null == _audioClip_Sound[id])
        {
            //読み込み失敗
            Debug.LogError("SoundFile : file not found (" + enumId + ")");
            return;
        }

        //再生
        PlaySound(_audioClip_Sound[id], (_volume_Sound * volume));
    }
    /// <summary>
    /// 効果音を再生する(Clip指定)
    /// </summary>
    public void PlaySound(AudioClip tempClip, float volume = 1f)
    {
        Transform sourceNode = transform;
        //再生用プレハブを生成
        GameObject soundObj = Instantiate(Resources.Load("System/Sounds/SoundOneshot"), sourceNode.position, sourceNode.rotation) as GameObject;
        //オブジェクトは子供にしておく
        soundObj.transform.parent = transform;

        //AudioSource取得
        AudioSource soundSource = soundObj.GetComponent<AudioSource>();
        //クリップ指定
        soundSource.clip = tempClip;
        //ループなし
        soundSource.loop = false;
        //音量
        soundSource.spatialBlend = 0;
        soundSource.volume = volume;
        //再生開始
        soundSource.Play();
    }
}



