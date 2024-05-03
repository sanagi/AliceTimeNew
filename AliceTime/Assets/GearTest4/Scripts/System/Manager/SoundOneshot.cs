using UnityEngine;
using System.Collections;


/// <summary>
/// ワンショットの効果音の再生用
/// </summary>
public class SoundOneshot : MonoBehaviour
{
    private AudioSource _audioSource = null;        //再生用のAudioSource

    /// <summary>
    /// 起動時
    /// </summary>
    protected void Awake()
    {
        //再生用のコンポーネント取得
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    protected void Update()
    {
        //再生終了したか確認
        if (!_audioSource.isPlaying)
        {
            //音が止まったら削除
            Destroy(gameObject);
        }
    }



}

