using System.Collections;
using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト用スクリプト　常に回転を加えるスクリプト
    /// </summary>
    [DisallowMultipleComponent]
    public class ClockworkAddRotation : ClockworkCurveBase
    {
        /// <summary>サイクル完了時に +1 して回転をつなげるフラグ</summary>
        [SerializeField, Tooltip("サイクルが終わった時、波形を引き継ぐか のこぎり波にするか")] private bool _riseWaveFlag = true;


        /// <summary>
        /// 開始時
        /// </summary>
        protected override void Start()
        {
            //処理を開始
            PlayCurve();
        }
        /// <summary>
        /// 設定されている動作を開始する
        /// </summary>
        protected override void PlayCurve()
        {
            //処理を開始
            StartCoroutine(UpdateOffset());
        }

        /// <summary>
        /// 回転処理
        /// </summary>
        IEnumerator UpdateOffset()
        {
            //アニメーションカーブの設定がある？
            if ((null == AnimationCurve) || (_curveSecond <= 0))
            {
                //中断
                Debug.LogError("カーブ設定がおかしいものがあります (" + name + ")\n" + _curveEffect);
                yield break;
            }

            //現在の値を取得
            Vector3 rotationOrigin = transform.localRotation.eulerAngles;
            Vector3 rotationOffset;
            //経過時間
            float progressSec = (_curveStart * _curveSecond);
            //毎フレーム加える
            do
            {
                //停止処理の確認
                if (IsPauseUpdate())
                {
                    //停止中
                    yield return null;
                    continue;
                }

                //経過時間を記録
                if (globalFlag)
                {
                    progressSec = GlobalProgressSec;
                }
                else
                {
                    progressSec += GetDeltaTime();
                }
                //オフセット量
                rotationOffset = (_curveEffect * GetCurrentCurveValue(AnimationCurve, progressSec, _curveSecond));
                //終わったサイクルぶん加算する？
                if (_riseWaveFlag)
                {
                    //周回数ぶん足す
                    rotationOffset += _curveEffect * (int)(progressSec / _curveSecond);
                }
                //反映
                transform.localRotation = Quaternion.Euler(rotationOrigin + rotationOffset);
                //待ち
                yield return null;
            }
            while (IsLoop(progressSec, _curveSecond));

            //終了処理
            Finish();
        }

#if UNITY_EDITOR
        /// <summary>
        /// エディタ用 カーブ初期化
        /// </summary>
        public override void InitializeCurve()
        {
            _curveEffect = new Vector3(0, 60, 0);
            AnimationCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        }
#endif
    }
}
