using System.Diagnostics;
using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト用スクリプト　カーブ再生系の基底
    /// </summary>
    [AddComponentMenu("")]
    public class ClockworkCurveBase : MonoBehaviour
    {
        /// <summary>全体管理の時間で動く</summary>
        [SerializeField, Tooltip("グローバル設定")] protected bool globalFlag = true;
        /// <summary>全体管理の時間</summary>
        public static float GlobalProgressSec = 0;

        /// <summary>ループの種類</summary>
        public enum LoopType
        {
            Loop,               //無限ループ
            OnceAndStop,        //一度だけ再生して止まる
            OnceAndDisable,     //一度だけ再生して消える
        }

        /// <summary>カーブの再生を無限ループさせるか</summary>
        [SerializeField, Tooltip("ループか一回だけ再生か")] protected LoopType loopType = LoopType.Loop;
        /// <summary>再生方法を取得</summary>
        public LoopType GetLoopType()
        {
            return loopType;
        }

        /// <summary>カーブが影響するパラメータ</summary>
        [SerializeField, Tooltip("カーブが影響するパラメータ")] protected Vector3 _curveEffect = new Vector3(0, 0, 0);
        /// <summary>カーブ設定</summary>
        public AnimationCurve AnimationCurve = null;

        /// <summary>1回のサイクルにかかる秒数</summary>
        [SerializeField, Tooltip("1サイクルに かかる秒数")] protected float _curveSecond = 1.0f;
        /// <summary>サイクルのオフセット(0.0～1.0)</summary>
        [SerializeField, Range(0f, 1f), Tooltip("サイクルの開始位置 (0.0～1.0)")] protected float _curveStart = 0.0f;



        /// <summary>
        /// 開始時
        /// </summary>
        protected virtual void Start()
        {
            //処理を開始
            PlayCurve();
        }

        /// <summary>
        /// 設定されている動作を開始する
        /// </summary>
        protected virtual void PlayCurve()
        {
        }

        /// <summary>
        /// アニメカーブをループさせるか？
        /// </summary>
        protected bool IsLoop(float now, float once)
        {
            return (LoopType.Loop == loopType) || (now < once);
        }

        /// <summary>
        /// 終了時の処理
        /// </summary>
        protected virtual void Finish()
        {
            if (LoopType.OnceAndDisable == loopType)
            {
                gameObject.SetActive(false);
            }
        }



        /// <summary>
        /// スクリプトの動きを停止する処理が入った場合、true が返る(クロノスの時止めなど)
        /// </summary>
        protected bool IsPauseUpdate()
        {
            //すべての条件にあてはまらないので動いてOK
            return false;
        }

        /// <summary>
        /// スクリプト内で使用する時間経過の処理用
        /// </summary>
        protected float GetDeltaTime()
        {
            return Time.deltaTime;      //一時停止やスローがあれば ここで制御
        }

        /// <summary>
        /// アニメーションカーブの現在の値を 0～1 で取得
        /// </summary>
        protected float GetCurrentCurveValue(AnimationCurve curve, float cycleNow, float cycleMax)
        {
            float ratio;
            if (LoopType.Loop == loopType)
            {
                //ループあり
                ratio = Mathf.Clamp01(((cycleMax + (cycleNow % cycleMax)) % cycleMax) / cycleMax);
            }
            else
            {
                //ワンショット
                ratio = Mathf.Clamp01(cycleNow / cycleMax);
            }
            return curve.Evaluate(ratio);
        }

        /// <summary>
        /// デバッグ用 停止処理
        /// </summary>
        public void StopCurve()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// デバッグ用 再始動処理
        /// </summary>
        public void ReplayCurve()
        {
            StopCurve();
            PlayCurve();
        }

        /// <summary>
        /// エディタ用 カーブ初期化
        /// </summary>
        public virtual void InitializeCurve()
        {
        }

        public float CurveSecond
        {
            get => _curveSecond;
            set => _curveSecond = value;
        }
    }
}
