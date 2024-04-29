using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum SoundType
{
	BGM,
	SE,
}

public enum SoundEnum
{
	BGM_TITLE,

	BGM_EVENT,

	BGM_MAIN_NORMAL,
	
	SE_CLEAR,
	SE_GAMEOVER,
	SE_STAGESELECT,
	SE_NEXT,
	SE_PAUSE,
	SE_Foot,
	SE_JumpUp,
	SE_JumpDown,
	SE_Hashigo,
	SE_Back,
	SE_OK,
	SE_CANCEL,
	SE_GET,

	SE_NONE
}

public class Audio_Manage : MonoBehaviour
{

	//private static List<AudioClip> soundStack = new List<AudioClip>();

	public class AudioData
	{
		public string path;
		public SoundType type;
		public float volume;

		public AudioData(string path, SoundType type,float volume) {
			this.path = path;
			this.type = type;
			this.volume = volume;
		}
	}

	public struct SoundClip{
		public SoundEnum _soundEnum;
		public AudioSource _audioSource;	
	}

	static Dictionary<SoundEnum, AudioData> audioTable = new Dictionary<SoundEnum, AudioData>() {
		{SoundEnum.BGM_TITLE, new AudioData("main_theme", SoundType.BGM,1f)},

		{SoundEnum.BGM_MAIN_NORMAL, new AudioData("dangion_normal", SoundType.BGM,1f)},
        {SoundEnum.SE_CLEAR, new AudioData("clear", SoundType.SE,1f)},
		{SoundEnum.SE_GAMEOVER, new AudioData("GameOver", SoundType.SE,1f)},
		{SoundEnum.SE_STAGESELECT, new AudioData("stage_serect01", SoundType.SE,1f)},
		{SoundEnum.SE_NEXT, new AudioData("next", SoundType.SE,1f)},
		{SoundEnum.SE_PAUSE, new AudioData("pause", SoundType.SE,1f)},

		{SoundEnum.SE_Foot, new AudioData("Foot", SoundType.SE,1f)},
		{SoundEnum.SE_JumpUp, new AudioData("jump", SoundType.SE,1f)},
		{SoundEnum.SE_JumpDown, new AudioData("jump_down", SoundType.SE,1f)},
		{SoundEnum.SE_Back, new AudioData("GUIBack", SoundType.SE,1f)},
		{SoundEnum.SE_OK, new AudioData("se_OK", SoundType.SE,1f)},
		{SoundEnum.SE_CANCEL, new AudioData("se_cancel", SoundType.SE,1f)},
		{SoundEnum.SE_GET, new AudioData("se_flagget", SoundType.SE,1f)},
	};

	/// <summary>現在のスタート値</summary>
	private static float m_startVolume = 0f;
	/// <summary>現在のアルファ値</summary>
	private static float m_currentVolume = 0f;
	/// <summary>目標のアルファ値</summary>
	private static float m_destinationVolume = 0.0f;
	/// <summary>フェード中かどうか</summary>
	private static bool m_isSoundFading = false;
	//次にスタートする曲があるか
	private static bool WaitSound = false;
	private static float nowSoundFadeTime;
	/// <summary>フェードの開始時間</summary>
	private static float m_SoundFadebeginTime = 0.0f;
	/// <summary>フェードの終了時間</summary>
	private static float m_SoundFadeendTime = 0.0f;

	public static AudioSource bgmSource;
	private static SoundEnum pre_Sound;
	private static SoundEnum next_sound;
	private static SoundEnum now_Sound;
	public static int seSourceCount = 4;
	public static SoundClip[] seSource;
	public static float[] seVolumeLoop;

	public static List<AudioClip> eventAudioList = new List<AudioClip>();

	private static float bgmVolume = 0f;
	private static float seVolume = 0f;

	public void Initialization() {
		bgmVolume = OptionManager.Instance.GetBGMVolume();
		seVolume = OptionManager.Instance.GetSEVolume();

		m_currentVolume = bgmVolume;

		GameObject go = new GameObject("BGMSource");
		go.transform.parent = gameObject.transform;
		bgmSource = go.AddComponent<AudioSource>();
		bgmSource.volume = bgmVolume;
		bgmSource.loop = true;
		bgmSource.playOnAwake = false;

		seSource = new SoundClip[seSourceCount];
		seVolumeLoop = new float[seSourceCount];
		for (int i = 0; i < seSourceCount; i++) {
			go = new GameObject("SESource" + i);
			go.transform.parent = gameObject.transform;
			seSource[i]._audioSource = go.AddComponent<AudioSource>();
			seSource[i]._audioSource.volume = seVolume;
			seSource[i]._audioSource.playOnAwake = false;

			seVolumeLoop[i] = 1.0f;
		}
	}

	public void Finalization() {
		Destroy(bgmSource.gameObject);
		bgmSource = null;

		for (var i = 0; i < seSourceCount; i++) {
			Destroy(seSource[i]._audioSource.gameObject);
			seSource[i]._audioSource = null;
		}
	}

	public static void ChangeVolume_BGM(float value) {
		bgmVolume = value;
		bgmSource.volume = value;
	}

    public static void ChangeVolume_BGMFromNow(float value)
    {
        bgmSource.volume *= value;
    }

    public static void BGMVolumeUserDefault()
    {
        bgmSource.volume = bgmVolume;
    }

    public static void ChangeVolume_SE(float value) {
		seVolume = value;
		for (int i = 0; i < seSourceCount; i++) {
			seSource[i]._audioSource.volume = seVolume;
		}
	}

	public static void Back_BGM() {
		Play(pre_Sound);
	}

	public static AudioClip GetBGM(SoundEnum sound){
		return Resources.Load("Sound/BGM/" + audioTable[sound].path) as AudioClip;
	}

	public static void Play(SoundEnum sound) {
		return;
		if (audioTable[sound].type == SoundType.BGM) {
			if (!m_isSoundFading) {
				if (bgmSource != null) {
					pre_Sound = now_Sound;
					bgmSource.Stop();
				}
				now_Sound = sound;
				int count = eventAudioList.Count;
				bool read = false;
				for(int i=0; i < count; i++){
					if(audioTable[sound].path == eventAudioList[i].name){
						bgmSource.clip = eventAudioList[i];
						read = true;
					}
				}
				if(!read){
					bgmSource.clip = Resources.Load("Sound/BGM/" + audioTable[sound].path) as AudioClip;
				}
				bgmSource.volume = bgmVolume*audioTable[sound].volume;
				bgmSource.Play();
				WaitSound = false;
			} else {
				next_sound = sound;
				WaitSound = true;
			}
		} else {

            AudioClip tmpaudio = Resources.Load("Sound/SE/" + audioTable[sound].path) as AudioClip;
            //int j = 0;
			int k = 0;
            for (int i = 0; i < seSource.Length; i++) {
                //再生終了してたら箱を開ける
				if (!seSource[i]._audioSource.isPlaying)
                {
					seSource[i]._audioSource.clip = null;
                }
            }
            for (int i = 0; i < seSource.Length; i++)
            {
				if (seSource[i]._audioSource.clip == null)
                {
                    k = i;
                    break;
                }
            }
			
            for (int i = 0; i < seSource.Length; i++)
            {
                //同じ音を鳴らそうとしてたら現在なっている音をストップ
				if (seSource[i]._audioSource.clip == tmpaudio)
                {
					seSource[i]._audioSource.Stop();
                    k = i;
                }
            }
			seSource[k]._audioSource.clip = tmpaudio;
			if (LoopSECheck(sound)) {
				seSource[k]._audioSource.loop = true;
				seVolumeLoop[k] = seVolume * audioTable[sound].volume;
			}
			else{
				seSource[k]._audioSource.loop = false;
			}
			seSource[k]._audioSource.volume = seVolume * audioTable[sound].volume;
			seSource[k]._audioSource.Play();
			seSource[k]._soundEnum = sound;
        }
	}

	public static SoundEnum GetSound(){
		return now_Sound;
	}

    private static bool LoopSECheck(SoundEnum se)
    {
        if(se == SoundEnum.SE_Foot)
        {
            return true;
        }

        return false;
    }

	public static void StopBGM() {
		bgmSource.Stop();
	}

    public static void StopLoopSE(SoundEnum se)
    {
        int k = 0;
        AudioClip tmpaudio = Resources.Load("Sound/SE/" + audioTable[se].path) as AudioClip;
        for (int i = 0; i < seSource.Length; i++)
         {
            if (seSource[i]._audioSource != null)
            {
                if (seSource[i]._audioSource.clip == tmpaudio)
                {
                    seSource[i]._audioSource.Stop();
                    seSource[i]._audioSource.clip = null;
                }
            }
        }
    }

	public static void LoopSEVolumeZero(){
		for (int i = 0; i < seSource.Length; i++)
		{
			if (seSource[i]._audioSource.loop)
			{
				seSource[i]._audioSource.volume = 0f;
			}
		}	
	}

	public static void LoopSEVolumeSet(){
		for (int i = 0; i < seSource.Length; i++)
		{
			if (seSource[i]._audioSource.loop)
			{
				seSource[i]._audioSource.volume = seVolumeLoop[i];
			}
		}	
	}

	/// <summary>
	/// 全てのSEをストップ
	/// </summary>
	public static void StopLoopAllSE(){
		for(int i = 0; i < seSource.Length;i++){
			//画面遷移系以外であれば
			if(seSource[i]._soundEnum != SoundEnum.SE_STAGESELECT){
				seSource[i]._audioSource.Stop();
				seSource[i]._audioSource.clip= null;
			}
		}
	}

    public static void FadeinPlayBGM(float duration, SoundEnum n) {
		next_sound = n;
		Play(next_sound);
		bgmSource.volume = 0.0f;
		SoundFadeTo(bgmVolume, duration);
	}

	public static void FadeoutStopBGM(float duration) {
		m_currentVolume = bgmSource.volume;
		SoundFadeTo(0.0f, duration);
	}

	static void SoundFadeTo(float destinationVolume, float duration) {
		m_destinationVolume = destinationVolume;
		m_SoundFadebeginTime = Time.time;
		m_SoundFadeendTime = m_SoundFadebeginTime + duration;
		m_isSoundFading = true;

		nowSoundFadeTime = m_SoundFadebeginTime;
		m_currentVolume = bgmSource.volume;
		m_startVolume = bgmSource.volume;
	}



	// Update is called once per frame
	void Update() {
		/*if (soundStack.Count > 0 && soundStack != null) {
			bgmSource.clip = Resources.Load("Sound/BGM/" + audioTable[soundStack[0]].path) as AudioClip;
			bgmSource.volume = bgmVolume;
			bgmSource.Play();
			soundStack.Clear();
		}*/

		if (m_isSoundFading) {
			if (nowSoundFadeTime >= m_SoundFadeendTime) {
				// フェード時間経過後初めての Update

				// フェード終了
				m_isSoundFading = false;

				// アルファ値を目的値に変更
				bgmSource.volume = m_destinationVolume;
				if (bgmSource.volume == 0) {
					StopBGM();
				}
				if (WaitSound) {
					Play(next_sound);
				}
			} else {
				// 進行度 (0.0～1.0)
				float ratio = Mathf.InverseLerp(m_SoundFadebeginTime, m_SoundFadeendTime, Time.fixedTime);

				if (m_destinationVolume == 0.0f) {
					m_currentVolume = m_startVolume * (1.0f - ratio);
				} else {
					m_currentVolume = ratio * m_destinationVolume;
				}

				bgmSource.volume = m_currentVolume;
				nowSoundFadeTime += Time.deltaTime;
			}
		}
	}
}